﻿using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using MonoMod.Utils.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace MonoMod.RuntimeDetour {
    public static class HarmonyDetourBridge {

        public enum Type {
            Auto,
            Basic,
            AsOriginal,
            Override
        }

        public static bool Initialized { get; private set; }
        private static Type CurrentType;

        private static Assembly _HarmonyASM;
        private static readonly HashSet<IDisposable> _Detours = new HashSet<IDisposable>();

        private static readonly Dictionary<System.Type, MethodInfo> _Emitters = new Dictionary<System.Type, MethodInfo>();

        [ThreadStatic] private static DynamicMethodDefinition _LastWrapperDMD;
        private static Assembly _SharedStateASM;

        private static DetourConfig _DetourConfig;

        static HarmonyDetourBridge() {
            System.Type t_OpCode = typeof(System.Reflection.Emit.OpCode);
            System.Type t_Proxy = ILGeneratorShim.GetProxyType<CecilILGenerator>();
            foreach (MethodInfo method in t_Proxy.GetMethods()) {
                if (method.Name != "Emit")
                    continue;

                ParameterInfo[] args = method.GetParameters();
                if (args.Length != 2)
                    continue;

                if (args[0].ParameterType != t_OpCode)
                    continue;

                System.Type argType = args[1].ParameterType;
                if (_Emitters.ContainsKey(argType) && method.DeclaringType != t_Proxy)
                    continue;

                _Emitters[argType] = method;
            }
        }

        public static bool Init(bool forceLoad = true, Type type = Type.Auto) {
            if (_HarmonyASM == null)
                _HarmonyASM = _FindHarmony();
            if (_HarmonyASM == null && forceLoad)
                _HarmonyASM = Assembly.Load(new AssemblyName() {
                    Name = "0Harmony"
                });
            if (_HarmonyASM == null)
                return false;

            if (Initialized)
                return true;
            Initialized = true;

            if (type == Type.Auto)
                type = Type.AsOriginal;

            _DetourConfig = new DetourConfig() {
                Priority =
                    type == Type.AsOriginal ? int.MinValue / 4 :
                    type == Type.Override ? int.MaxValue / 4 :
                    0
            };

            CurrentType = type;

            try {
                foreach (MethodInfo methodRD in typeof(HarmonyDetourBridge).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)) {
                    bool critical = methodRD.GetCustomAttributes(typeof(CriticalAttribute), false).Any();

                    foreach (DetourToRDAttribute info in methodRD.GetCustomAttributes(typeof(DetourToRDAttribute), false))
                        foreach (MethodInfo methodH in GetHarmonyMethod(methodRD, info.Type, info.SkipParams, info.Name)) {
                            critical = false;
                            _Detours.Add(new Hook(methodH, methodRD));
                        }

                    foreach (DetourToHAttribute info in methodRD.GetCustomAttributes(typeof(DetourToHAttribute), false))
                        foreach (MethodInfo methodH in GetHarmonyMethod(methodRD, info.Type, info.SkipParams, info.Name)) {
                            critical = false;
                            _Detours.Add(new Detour(methodRD, methodH));
                        }

                    foreach (TranspileAttribute info in methodRD.GetCustomAttributes(typeof(TranspileAttribute), false)) {
                        foreach (MethodInfo methodH in GetHarmonyMethod(methodRD, info.Type, -1, info.Name)) {
                            using (DynamicMethodDefinition dmd = new DynamicMethodDefinition(methodH)) {
                                critical = false;
                                ILContext il = new ILContext(dmd.Definition) {
                                    ReferenceBag = RuntimeILReferenceBag.Instance
                                };
                                _Detours.Add(il);
                                il.Invoke((ILContext.Manipulator) methodRD.CreateDelegate<ILContext.Manipulator>());
                                if (il.IsReadOnly) {
                                    il.Dispose();
                                    _Detours.Remove(il);
                                    continue;
                                }
                                _Detours.Add(new Detour(methodH, dmd.Generate()));
                            }
                        }
                    }

                    if (critical)
                        throw new Exception($"Cannot apply HarmonyDetourBridge rule {methodRD.Name}");
                }
            } catch {
                _EarlyReset();
                throw;
            }
 
            return true;
        }

        private static bool _EarlyReset() {
            foreach (IDisposable detour in _Detours)
                detour.Dispose();
            _Detours.Clear();
            return false;
        }

        public static void Reset() {
            if (!Initialized)
                return;
            Initialized = false;

            _EarlyReset();
        }

        private static System.Type GetHarmonyType(string typeName) {
            return
                _HarmonyASM.GetType(typeName) ??
                _HarmonyASM.GetType("HarmonyLib." + typeName) ??
                _HarmonyASM.GetType("Harmony." + typeName) ??
                _HarmonyASM.GetType("Harmony.ILCopying." + typeName);
        }

        private static IEnumerable<MethodInfo> GetHarmonyMethod(MethodInfo ctx, string typeName, int skipParams, string name) {
            System.Type type = GetHarmonyType(typeName);
            if (type == null)
                return null;

            if (string.IsNullOrEmpty(name))
                name = ctx.Name;

            if (skipParams < 0)
                return type
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .Where(method => method.Name == name);

            return new MethodInfo[] {
                type.GetMethod(
                    name,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                    null,
                    ctx.GetParameters().Skip(skipParams).Select(p => p.ParameterType).ToArray(),
                    null
                )
            };
        }

        // Replacement for DynamicTools.CreateDynamicMethod
        private static DynamicMethodDefinition CreateDMD(MethodBase original, string suffix) {
            if (original == null)
                throw new ArgumentNullException(nameof(original));

            ParameterInfo[] args = original.GetParameters();
            System.Type[] argTypes;
            if (!original.IsStatic) {
                argTypes = new System.Type[args.Length + 1];
                argTypes[0] = original.GetThisParamType();
                for (int i = 0; i < args.Length; i++)
                    argTypes[i + 1] = args[i].ParameterType;
            } else {
                argTypes = new System.Type[args.Length];
                for (int i = 0; i < args.Length; i++)
                    argTypes[i] = args[i].ParameterType;
            }

            return new DynamicMethodDefinition(
                (original.Name + suffix).Replace("<>", ""),
                (original as MethodInfo)?.ReturnType ?? typeof(void),
                argTypes
            );
        }

        [DetourToRD("Memory")]
        private static long GetMethodStart(MethodBase method, out Exception exception) {
            exception = null;
            try {
                _Detours.Add(new LazyDisposable<MethodBase>(method, m => DetourHelper.Unpin(m)));
                return (long) method.Pin().GetNativeStart();
            } catch (Exception e) {
                exception = e;
                return 0;
            }
        }

        [DetourToRD("Memory"), Critical]
        private static string WriteJump(long memory, long destination) {
            _Detours.Add(new NativeDetour((IntPtr) memory, (IntPtr) destination));
            return null;
        }

        [DetourToRD("Memory"), Critical]
        private static string DetourMethod(MethodBase original, MethodBase replacement) {
            if (replacement == null) {
                replacement = _LastWrapperDMD.Generate();
                _LastWrapperDMD.Dispose();
                _LastWrapperDMD = null;
            }

            _Detours.Add(new Detour(original, replacement, ref _DetourConfig));
            return null;
        }

        [DetourToRD("MethodBodyReader", 1)]
        private static MethodInfo EmitMethodForType(object self, System.Type type) {
            foreach (KeyValuePair<System.Type, MethodInfo> entry in _Emitters)
                if (entry.Key == type)
                    return entry.Value;

            foreach (KeyValuePair<System.Type, MethodInfo> entry in _Emitters)
                if (entry.Key.IsAssignableFrom(type))
                    return entry.Value;

            return null;
        }

        [DetourToRD("PatchProcessor", 2), Critical]
        private static List<System.Reflection.Emit.DynamicMethod> Patch(Func<object, List<System.Reflection.Emit.DynamicMethod>> orig, object self) {
            orig(self);

            // We can't instantiate DMs.
            // Even if we could return generated DMDs, they don't always result in DMs.
            // Thus, return an empty list and hope for the best.
            return new List<System.Reflection.Emit.DynamicMethod>();
        }

        // Patch and both Unpatch methods run UpdateWrapper, which then runs CreatePatchedMethod.

        [Transpile("PatchFunctions"), Critical]
        private static void UpdateWrapper(ILContext il) {
            ILCursor c = new ILCursor(il);

            // Pop the MissingMethodException.
            // CreatePatchedMethod returns null, deal with it.
            c.GotoNext(i => i.MatchThrow());
            c.Next.OpCode = OpCodes.Pop;
        }

        [Transpile("MethodPatcher"), Critical]
        private static void CreatePatchedMethod(ILContext il) {
            ILCursor c = new ILCursor(il);

            // The original method uses System.Reflection.Emit.

            System.Type t_DynamicTools = GetHarmonyType("DynamicTools");

            // Find and replace DynamicTools.CreateDynamicMethod
            if (!c.TryGotoNext(i => i.MatchCall(t_DynamicTools, "CreateDynamicMethod"))) {
                // Not the right method. Harmony defines two CreatePatchedMethod methods.
                il.MakeReadOnly();
                return;
            }
            c.Next.OpCode = OpCodes.Call;
            c.Next.Operand = il.Import(typeof(HarmonyDetourBridge).GetMethod("CreateDMD", BindingFlags.NonPublic | BindingFlags.Static));
            
            // Find the variable holding the "dynamic method" and update it.
            int varDMDi = -1;
            c.GotoNext(i => i.MatchStloc(out varDMDi));
            VariableDefinition varDMD = il.Body.Variables[varDMDi];
            varDMD.VariableType = il.Import(typeof(DynamicMethodDefinition));

            // Find and replace patch.GetILGenerator
            c.GotoNext(i => i.MatchCallvirt<System.Reflection.Emit.DynamicMethod>("GetILGenerator"));
            c.Next.OpCode = OpCodes.Call;
            c.Next.Operand = il.Import(typeof(DynamicMethodDefinition).GetMethod("GetILGenerator", BindingFlags.Public | BindingFlags.Instance));

            // Find and remove DynamicTools.PrepareDynamicMethod
            c.GotoNext(i => i.MatchCall(t_DynamicTools, "PrepareDynamicMethod"));
            c.Next.OpCode = OpCodes.Pop;
            c.Next.Operand = null;

            // Go to the next ldloc that loads the DynamicMethod.
            // No matter if it gets stored into a local variable or returned immediately,
            // grab it, store it separately and push null as a replacement.
            c.GotoNext(i => i.MatchLdloc(varDMDi));
            c.Index++;
            c.EmitDelegate<Func<DynamicMethodDefinition, System.Reflection.Emit.DynamicMethod>>(dmd => {
                _LastWrapperDMD = dmd;
                return null;
            });
        }

        [DetourToRD("HarmonySharedState", 1)]
        private static Assembly SharedStateAssembly(Func<Assembly> orig) {
            Assembly asm = orig();
            if (asm != null)
                return asm;

            if (_SharedStateASM != null)
                return _SharedStateASM;

            string name = (string) GetHarmonyType("HarmonySharedState").GetField("name", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
#if !CECIL0_9
            using (
#endif
            ModuleDefinition module = ModuleDefinition.CreateModule(
                $"MonoMod.RuntimeDetour.{name}",
                new ModuleParameters() {
                    Kind = ModuleKind.Dll,
#if !CECIL0_9
                    ReflectionImporterProvider = MMReflectionImporter.Provider
#endif
                }
            )
#if CECIL0_9
            ;
#else
            )
#endif
            {
                TypeDefinition type = new TypeDefinition(
                    "", name,
                    Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Abstract | Mono.Cecil.TypeAttributes.Sealed | Mono.Cecil.TypeAttributes.Class
                ) {
                    BaseType = module.TypeSystem.Object
                };
                module.Types.Add(type);

                type.Fields.Add(new FieldDefinition(
                    "state",
                    Mono.Cecil.FieldAttributes.Public | Mono.Cecil.FieldAttributes.Static,
                    module.ImportReference(typeof(Dictionary<MethodBase, byte[]>))
                ));

                type.Fields.Add(new FieldDefinition(
                    "version",
                    Mono.Cecil.FieldAttributes.Public | Mono.Cecil.FieldAttributes.Static,
                    module.ImportReference(typeof(int))
                ));

                return _SharedStateASM = ReflectionHelper.Load(module);
            }
        }

        private class DetourToRDAttribute : Attribute {
            public string Type { get; }
            public int SkipParams { get; }
            public string Name { get; }
            public DetourToRDAttribute(string type, int skipParams = 0, string name = null) {
                Type = type;
                SkipParams = skipParams;
                Name = name;
            }
        }

        private class DetourToHAttribute : Attribute {
            public string Type { get; }
            public int SkipParams { get; }
            public string Name { get; }
            public DetourToHAttribute(string type, int skipParams = 0, string name = null) {
                Type = type;
                SkipParams = skipParams;
                Name = name;
            }
        }

        private class TranspileAttribute : Attribute {
            public string Type { get; }
            public string Name { get; }
            public TranspileAttribute(string type, string name = null) {
                Type = type;
                Name = name;
            }
        }

        private class CriticalAttribute : Attribute {
        }

        private static Assembly _FindHarmony() {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies()) {
                if (asm.GetName().Name == "0Harmony" || asm.GetName().Name == "Harmony" ||
                    asm.GetType("Harmony.HarmonyInstance") != null ||
                    asm.GetType("HarmonyLib.Harmony") != null) {
                    return asm;
                }
            }
            return
                System.Type.GetType("Harmony.HarmonyInstance", false, false)?.Assembly ??
                System.Type.GetType("HarmonyLib.Harmony", false, false)?.Assembly;
        }

    }
}
