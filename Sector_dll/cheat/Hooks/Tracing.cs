using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat.Hooks
{
    class Tracing
    {

        public static PropertyInfo Type_GetProperty(string name, Type returnType, Type[] types)
        {
            return null;
        }

        public static MethodInfo Type_GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, 
            Type[] types, ParameterModifier[] modifiers)
        {
            return null;
        }

        public static Type Type_GetType(string typeName)
        {
            return null;
        }

        public static bool String_op_Equality(string s1, string s2)
        {
            return false;
        }

        public static bool String_op_Inequality(string s1, string s2)
        {
            return false;
        }

    }
}
