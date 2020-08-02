using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat.Hooks
{
    class Tracing
    {

        public static readonly string file = "hidden_log.txt";

        public static void ApplyHooks()
        {
            new Hook(typeof(Type).GetMethod("GetProperty", BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder,
                new Type[] { typeof(string), typeof(Type), typeof(Type[]) }, null), typeof(Tracing).GetMethod("Type_GetProperty"));
            new Hook(typeof(Type).GetMethod("GetMethod", BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder,
                new Type[] { typeof(string), typeof(BindingFlags), typeof(Binder), typeof(CallingConventions), typeof(Type[]), typeof(ParameterModifier[]) }, 
                null), typeof(Tracing).GetMethod("Type_GetMethod"));
            new Hook(typeof(Type).GetMethod("GetType", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
                new Type[] { typeof(string) }, null), typeof(Tracing).GetMethod("Type_GetType"));
            new Hook(typeof(MethodBase).GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder,
                new Type[] { typeof(object), typeof(object[]) }, null), typeof(Tracing).GetMethod("MethodBase_Invoke"));
            //new Hook(typeof(string).GetMethod("op_Equality", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
            //    new Type[] { typeof(string), typeof(string) }, null), typeof(Tracing).GetMethod("String_op_Equality"));
            //new Hook(typeof(string).GetMethod("op_Inequality", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
            //    new Type[] { typeof(string), typeof(string) }, null), typeof(Tracing).GetMethod("String_op_Inequality"));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static PropertyInfo Type_GetProperty(Func<Type, string, Type, Type[], PropertyInfo> orig, 
            Type self, string name, Type returnType, Type[] types)
        {
            PropertyInfo pi = orig(self, name, returnType, types);
            Log.Dump(file, "Type_GetProperty: " + pi.ToString());
            return pi;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static MethodInfo Type_GetMethod(Func<Type, string, BindingFlags, Binder, CallingConventions, Type[], ParameterModifier[], MethodInfo> orig,
            Type self, string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            MethodInfo mi = orig(self, name, bindingAttr, binder, callConvention, types, modifiers);
            Log.Dump(file, "Type_GetMethod: " + mi.ToString());
            return mi;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Type Type_GetType(Func<string, Type> orig, string typeName)
        {
            Type t = orig(typeName);
            Log.Dump(file, "Type_GetType: " + (t != null ? t.ToString() : "[null](" + typeName + ")"));
            return t;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static object MethodBase_Invoke(Func<MethodBase, object, object[], object> orig, MethodBase self, object obj, object[] parameters)
        {
            object ret = orig(self, obj, parameters);
            Log.Dump(file, "Invoke: " + 
                (obj != null ? obj.ToString() + " -> " : "") + 
                self.ToString() + 
                (parameters != null ? "(" + string.Join(", ", parameters.Select(x => x.ToString()).ToArray()) + ")" : "") + 
                (ret != null ? ": " + ret.ToString() : ""));
            return ret;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool String_op_Equality(Func<string, string, bool> orig, string s1, string s2)
        {
            Log.Dump(file, "String: \"" + s1 + "\" == \"" + s2 + "\"");
            return orig(s1, s2);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool String_op_Inequality(Func<string, string, bool> orig, string s1, string s2)
        {
            Log.Dump(file, "String: \"" + s1 + "\" != \"" + s2 + "\"");
            return orig(s1, s2);
        }

    }
}
