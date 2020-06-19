using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sector_dll.cheat
{
    class SignatureManager
    {

        public static ResolvedType Player = new ResolvedType("Player", new ClassSignature() 
        {
            nameLength = 31,

            publicClass = true,
            abstractClass = false,

            privateMethods = 2,
            publicMethods = 120,
            staticMethods = 0,

            publicFields = 143,
            privateFields = 5,
            staticFields = 0,
            readonlyFields = 5,

            boolFields = 28,
            byteFields = 3,
            shortFields = 0,
            intFields = 26,
            longFields = 0,
            floatFields = 1,
            doubleFields = 9,
            enumFields = 11,
            stringFields = 5,
            ArrayFields = 4,
            OtherFields = 64
        });

        public static ResolvedType GClass49 = new ResolvedType("GClass49", new ClassSignature() 
        {
            nameLength = 31,

            publicClass = true,
            abstractClass = false,

            privateMethods = 48,
            publicMethods = 329,
            staticMethods = 0,

            publicFields = 155,
            privateFields = 42,
            staticFields = 5,
            readonlyFields = 6,

            boolFields = 39,
            byteFields = 1,
            shortFields = 0,
            intFields = 13,
            longFields = 1,
            floatFields = 0,
            doubleFields = 24,
            enumFields = 9,
            stringFields = 0,
            ArrayFields = 10,
            OtherFields = 130
        });

        public static ResolvedType DrawingHelper = new ResolvedType("DrawingHelper", new ClassSignature()
        {
            nameLength = 27,

            publicClass = true,
            abstractClass = true,

            privateMethods = 0,
            publicMethods = 16,
            staticMethods = 12,

            publicFields = 78,
            privateFields = 1,
            staticFields = 79,
            readonlyFields = 0,

            boolFields = 0,
            byteFields = 0,
            shortFields = 0,
            intFields = 0,
            longFields = 0,
            floatFields = 0,
            doubleFields = 2,
            enumFields = 0,
            stringFields = 0,
            ArrayFields = 29,
            OtherFields = 48
        });

        public static ResolvedType[] ResolvedTypes = new ResolvedType[]
        {
            Player,
            GClass49,
            DrawingHelper
        };

        public static Type PlayerBase;

        public static MethodInfo GClass49_vmethod_4;

        public static FieldInfo GClass49_player_list;

        public static Type Drawing;

        public static MethodInfo DrawingHelper_DrawRect;

        public static Type Rect;

        public static ConstructorInfo Rect_Constructor_vec2;

        public static ConstructorInfo Rect_Constructor_standalone;

        public static Type Color;

        public static ConstructorInfo Color_Constructor_uint;

        public static ConstructorInfo Color_Constructor_byte;

        public static ConstructorInfo Color_Constructor_float;

        public static void FindSignatures()
        {
            //Console.WriteLine("Waiting for debugger to attach");
            //while (!Debugger.IsAttached)
            //{
            //    Thread.Sleep(100);
            //}
            //Console.WriteLine("Debugger attached");
            //Debugger.Break();

            Assembly aassembly = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "sectorsedge");
            foreach (Type t in aassembly.GetTypes())
            {
                ClassSignature sig = ClassSignature.GenerateSignature(t);
                foreach(ResolvedType rt in ResolvedTypes)
                {
                    rt.Update(sig, t);
                }
            }

            foreach (ResolvedType rt in ResolvedTypes)
            {
                Log.Info(rt.ToString());
            }

            PlayerBase = Player.Type.BaseType;

            BindingFlags bindingFlags =
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly;

            foreach (MethodInfo m in GClass49.Type.GetMethods(bindingFlags))
            {
                if (m.IsPublic && m.IsVirtual && !m.IsConstructor && m.ReturnType == typeof(void) && m.GetParameters().Length == 1
                    && m.Name.Length == 15 && m.GetParameters()[0].ParameterType.Name.Length == 31)
                {
                    GClass49_vmethod_4 = m;
                    Drawing = m.GetParameters()[0].ParameterType;
                    Log.Info("Found GClass49.vmethod_4 method as: " + m.ToString());
                    break;
                }
            }

            foreach (FieldInfo f in GClass49.Type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (f.IsPublic && f.Name.Length == 23 && f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(List<>)
                    && f.FieldType.GetGenericArguments().Length == 1 && f.FieldType.GetGenericArguments()[0] == Player.Type)
                {
                    GClass49_player_list = f;
                    Log.Info("Found GClass49.player_list field as: " + f.ToString());
                    break;
                }
            }

            foreach (MethodInfo m in DrawingHelper.Type.GetMethods(bindingFlags))
            {
                if (m.IsPublic && m.IsStatic && m.ReturnType == typeof(void) && m.GetParameters().Length == 4 
                    && m.GetParameters()[0].ParameterType == Drawing.BaseType && IsStruct(m.GetParameters()[1].ParameterType)
                    && m.GetParameters()[2].ParameterType == typeof(double) && IsStruct(m.GetParameters()[3].ParameterType))
                {
                    DrawingHelper_DrawRect = m;
                    Rect = m.GetParameters()[1].ParameterType;
                    Color = m.GetParameters()[3].ParameterType;
                    Log.Info("Found DrawingHelper.DrawRect method as: " + m.ToString());
                    break;
                }
            }

            foreach (ConstructorInfo ci in Rect.GetConstructors())
            {
                if (ci.GetParameters().Length == 2)
                    Rect_Constructor_vec2 = ci;
                else if (ci.GetParameters().Length == 4)
                    Rect_Constructor_standalone = ci;
            }

            foreach (ConstructorInfo ci in Color.GetConstructors())
            {
                if (ci.GetParameters().Length == 1)
                    Color_Constructor_uint = ci;
                else if (ci.GetParameters().Length == 4)
                {
                    if(ci.GetParameters()[0].ParameterType == typeof(byte))
                        Color_Constructor_byte = ci;
                    else if (ci.GetParameters()[0].ParameterType == typeof(float))
                        Color_Constructor_float = ci;
                }
            }

        }

        private static bool IsStruct(Type t)
        {
            return t.IsValueType && !t.IsEnum;
        } 

        public static string GenerateSig(String className)
        {
            Assembly aassembly = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "sectorsedge");
            Type t = aassembly.GetType(className, true);
            if(t != null) 
                return ClassSignature.GenerateSignature(t).ToString();
            return "Type not found!";
        }

    }
}
