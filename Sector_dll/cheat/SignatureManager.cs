using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public static ResolvedType Vec4 = new ResolvedType("Vec4", new ClassSignature()
        {
            nameLength = 23,

            publicClass = true,
            abstractClass = false,

            privateMethods = 0,
            publicMethods = 7,
            staticMethods = 1,

            publicFields = 5,
            privateFields = 0,
            staticFields = 1,
            readonlyFields = 0,

            boolFields = 0,
            byteFields = 0,
            shortFields = 0,
            intFields = 0,
            longFields = 0,
            floatFields = 0,
            doubleFields = 4,
            enumFields = 0,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 1
        });

        public static ResolvedType Font = new ResolvedType("Font", new ClassSignature()
        {
            nameLength = 15,

            publicClass = true,
            abstractClass = false,

            privateMethods = 0,
            publicMethods = 13,
            staticMethods = 0,

            publicFields = 0,
            privateFields = 3,
            staticFields = 0,
            readonlyFields = 0,

            boolFields = 0,
            byteFields = 0,
            shortFields = 0,
            intFields = 1,
            longFields = 0,
            floatFields = 0,
            doubleFields = 1,
            enumFields = 0,
            stringFields = 1,
            ArrayFields = 0,
            OtherFields = 0
        });

        public static ResolvedType RequestHelper = new ResolvedType("RequestHelper", new ClassSignature()
        {
            nameLength = 39,

            publicClass = true,
            abstractClass = true,

            privateMethods = 1,
            publicMethods = 6,
            staticMethods = 3,

            publicFields = 0,
            privateFields = 0,
            staticFields = 0,
            readonlyFields = 0,

            boolFields = 0,
            byteFields = 0,
            shortFields = 0,
            intFields = 0,
            longFields = 0,
            floatFields = 0,
            doubleFields = 0,
            enumFields = 0,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 0
        });

        public static ResolvedType ConnectionHelper = new ResolvedType("ConnectionHelper", new ClassSignature() {
            nameLength = 27,

            publicClass = true,
            abstractClass = true,

            privateMethods = 3,
            publicMethods = 40,
            staticMethods = 39,

            publicFields = 3,
            privateFields = 2,
            staticFields = 5,
            readonlyFields = 1,

            boolFields = 0,
            byteFields = 1,
            shortFields = 0,
            intFields = 0,
            longFields = 0,
            floatFields = 0,
            doubleFields = 0,
            enumFields = 0,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 4
        });

        //public static ResolvedType XXX = new ResolvedType("XXX", new ClassSignature());

        public static ResolvedType[] ResolvedTypes = new ResolvedType[]
        {
            Player,
            GClass49,
            DrawingHelper,
            Vec4,
            Font,
            RequestHelper,
            ConnectionHelper
        };

        public static Type PlayerBase;

        public static Type LocalPlayer;

        public static FieldInfo PlayerBase_origin;

        public static FieldInfo PlayerBase_health;

        public static FieldInfo PlayerBase_crouching;

        public static MethodInfo PLayerBase_EitherMod;

        public static MethodInfo GClass49_vmethod_4;

        public static MethodInfo GClass49_getPlayersToXray;

        public static FieldInfo GClass49_player_list;

        public static FieldInfo GClass49_Base_matrix1;

        public static FieldInfo GClass49_Base_matrix2;

        public static MethodInfo GClass49_Base_GetPlayerColor;

        public static FieldInfo GClass49_Base_LocalPlayer;

        public static FieldInfo GClass49_Base_Base_ScreenWidth;

        public static FieldInfo GClass49_Base_Base_ScreenHeight;

        public static ConstructorInfo Font_Constructor;

        public static MethodInfo RequestHelper_POST;

        public static MethodInfo RequestHelper_GET;

        public static MethodInfo ConnectionHelper_GetProcAddress;

        public static Type Drawing;

        public static MethodInfo Drawing_DrawFilledRect;

        public static MethodInfo Drawing_DrawString;

        public static MethodInfo DrawingHelper_DrawRect;

        public static Type Rect;

        public static ConstructorInfo Rect_Constructor_vec2;

        public static ConstructorInfo Rect_Constructor_standalone;

        public static Type Vec2;

        public static ConstructorInfo Vec2_Constructor;

        public static Type Size;

        public static ConstructorInfo Size_Constructor;

        public static Type Color;

        public static ConstructorInfo Color_Constructor_uint;

        public static ConstructorInfo Color_Constructor_byte;

        public static ConstructorInfo Color_Constructor_float;

        public static Type Vec3;

        public static ConstructorInfo Vec3_Constructor_double;

        public static MethodInfo Vec4_Multiply_Matrix4;

        public static ConstructorInfo Vec4_Constructor_standalone;

        public static ConstructorInfo Vec4_Constructor_Vec3;

        public static Type Matrix4;

        public static MethodInfo Matrix4_Multiply_Matrix4;

        public static Type ModType;

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

            foreach (FieldInfo f in PlayerBase.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (f.IsPublic && f.Name.Length == 11 && IsStruct(f.FieldType))
                {
                    PlayerBase_origin = f;
                    Vec3 = f.FieldType;
                    Log.Info("Found PlayerBase.origin as: " + PlayerBase_origin.ToString());
                    Log.Info("Found class Vec3 as: " + Vec3.ToString());
                }
                if (PlayerBase_health == null && f.IsPublic && f.Name.Length == 15 && Type.GetTypeCode(f.FieldType) == TypeCode.Double)
                {
                    PlayerBase_health = f;
                    Log.Info("Found PlayerBase_health as: " + PlayerBase_health.ToString());
                }
                if (f.IsPrivate && !f.IsFamily && f.FieldType.IsArray && f.Name.Length == 23)
                {
                    PlayerBase_crouching = f;
                    Log.Info("Found PlayerBase_crouching as: " + PlayerBase_crouching.ToString());
                }
            }

            foreach (MethodInfo mi in  PlayerBase.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (mi.IsPublic && mi.Name.Length == 23 && mi.GetParameters().Length == 1 &&
                    mi.ReturnType == typeof(bool) && mi.GetParameters()[0].ParameterType.Name.Length == 39)
                {
                    PLayerBase_EitherMod = mi;
                    ModType = mi.GetParameters()[0].ParameterType;
                    Log.Info("Found PLayerBase_EitherMod method as: " + PLayerBase_EitherMod.ToString());
                    Log.Info("Found PModType as: " + ModType.ToString());
                }
            }

            foreach (ConstructorInfo ci in Vec3.GetConstructors())
            {
                if(ci.GetParameters().Length == 3)
                {
                    Vec3_Constructor_double = ci;
                    Log.Info("Found Vec3_Constructor_double as: " + Vec3_Constructor_double.ToString());
                }
            }

            foreach (MethodInfo m in GClass49.Type.GetMethods(bindingFlags))
            {
                if (m.IsPublic && m.IsVirtual && !m.IsConstructor && m.ReturnType == typeof(void) && m.GetParameters().Length == 1
                    && m.Name.Length == 15 && m.GetParameters()[0].ParameterType.Name.Length == 31)
                {
                    GClass49_vmethod_4 = m;
                    Drawing = m.GetParameters()[0].ParameterType;
                    Log.Info("Found GClass49.vmethod_4 method as: " + m.ToString());
                    Log.Info("Found class Drawing as: " + Drawing.ToString());
                }
                if(m.IsPrivate && m.GetParameters().Length == 0 && m.ReturnType.IsGenericType && m.Name.Length == 27
                    && m.ReturnType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    GClass49_getPlayersToXray = m;
                    Log.Info("Found GClass49.getPlayersToXray method as: " + m.ToString());
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
                    Log.Info("Found class Rect as: " + Rect.ToString());
                    Log.Info("Found class Color as: " + Color.ToString());
                    break;
                }
            }

            foreach (MethodInfo mi in Drawing.GetMethods(bindingFlags))
            {
                if (mi.IsPublic && mi.Name.Length == 15 && mi.GetParameters().Length == 2 
                    && mi.GetParameters()[0].ParameterType == Rect && mi.GetParameters()[1].ParameterType == Color)
                {
                    Drawing_DrawFilledRect = mi;
                    Log.Info("Found Drawing_DrawFilledRect method as: " + Drawing_DrawFilledRect.ToString());
                }
                if(mi.IsPublic && mi.Name.Length == 11 && mi.GetParameters().Length == 7 && mi.GetParameters()[0].ParameterType == typeof(string))
                {
                    Drawing_DrawString = mi;
                    Log.Info("Found Drawing_DrawString method as: " + Drawing_DrawString.ToString());
                }
            }

            foreach(ConstructorInfo ci in Font.Type.GetConstructors())
            {
                if (ci.IsPublic && ci.GetParameters().Length == 3 && ci.GetParameters()[2].ParameterType == typeof(int))
                {
                    Font_Constructor = ci;
                    Log.Info("Found Font_Constructor constructor as: " + Font_Constructor.ToString());
                }
            }

            foreach(MethodInfo mi in RequestHelper.Type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if(mi.GetParameters().Length == 2)
                {
                    RequestHelper_POST = mi;
                    Log.Info("Found RequestHelper_POST as: " + RequestHelper_POST.ToString());
                }
                if(mi.GetParameters().Length == 1 && mi.GetParameters()[0].ParameterType == typeof(string))
                {
                    RequestHelper_GET = mi;
                    Log.Info("Found RequestHelper_GET as: " + RequestHelper_GET.ToString());
                }
            }

            foreach(MethodInfo mi in ConnectionHelper.Type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if(mi.Name.Length == 15 && mi.ReturnType == typeof(IntPtr) && mi.GetParameters().Length == 2 &&
                    mi.GetParameters()[0].ParameterType == typeof(IntPtr) && mi.GetParameters()[1].ParameterType == typeof(string))
                {
                    ConnectionHelper_GetProcAddress = mi;
                    Log.Info("Found ConnectionHelper_GetProcAddress as: " + ConnectionHelper_GetProcAddress.ToString());
                }
            }

            foreach (ConstructorInfo ci in Rect.GetConstructors())
            {
                if (ci.GetParameters().Length == 2)
                {
                    Rect_Constructor_vec2 = ci;
                    Vec2 = ci.GetParameters()[0].ParameterType;
                    Size = ci.GetParameters()[1].ParameterType;
                    Log.Info("Found Rect_Constructor_vec2 as: " + Rect_Constructor_vec2.ToString());
                    Log.Info("Found class Vec2 as: " + Vec2.ToString());
                    Log.Info("Found class Size as: " + Size.ToString());
                }
                else if (ci.GetParameters().Length == 4)
                {
                    Rect_Constructor_standalone = ci;
                    Log.Info("Found Rect_Constructor_standalone as: " + Rect_Constructor_standalone.ToString());
                }
            }

            Vec2_Constructor = Vec2.GetConstructors()[0];
            Size_Constructor = Size.GetConstructors()[0];

            Log.Info("Found Vec2_Constructor as: " + Vec2_Constructor.ToString());
            Log.Info("Found Size_Constructor as: " + Size_Constructor.ToString());

            foreach (ConstructorInfo ci in Color.GetConstructors())
            {
                if (ci.GetParameters().Length == 1)
                {
                    Color_Constructor_uint = ci;
                    Log.Info("Found Color_Constructor_uint as: " + Color_Constructor_uint.ToString());
                }
                else if (ci.GetParameters().Length == 4)
                {
                    if(ci.GetParameters()[0].ParameterType == typeof(byte))
                    {
                        Color_Constructor_byte = ci;
                        Log.Info("Found Color_Constructor_byte as: " + Color_Constructor_byte.ToString());
                    }
                    else if (ci.GetParameters()[0].ParameterType == typeof(float))
                    {
                        Color_Constructor_float = ci;
                        Log.Info("Found Color_Constructor_float as: " + Color_Constructor_float.ToString());
                    }
                }
            }

            foreach(MethodInfo mi in Vec4.Type.GetMethods())
            {
                if(mi.Name == "op_Multiply")
                {
                    Vec4_Multiply_Matrix4 = mi;
                    Matrix4 = mi.GetParameters()[1].ParameterType;
                    Log.Info("Found Vec4_Multiply_Matrix4 as: " + Vec4_Multiply_Matrix4.ToString());
                    Log.Info("Found class Matrix4 as: " + Matrix4.ToString());
                }
            }

            foreach(MethodInfo mi in Matrix4.GetMethods())
            {
                if(mi.Name == "op_Multiply")
                {
                    Matrix4_Multiply_Matrix4 = mi;
                    Log.Info("Found Matrix4_Multiply_Matrix4 as: " + Matrix4_Multiply_Matrix4.ToString());
                }
            }

            foreach (ConstructorInfo ci in Vec4.Type.GetConstructors())
            {
                if(ci.GetParameters().Length == 4)
                {
                    Vec4_Constructor_standalone = ci;
                    Log.Info("Found Vec4_Constructor_standalone as: " + Vec4_Constructor_standalone.ToString());
                }
                if (ci.GetParameters()[0].ParameterType == Vec3)
                {
                    Vec4_Constructor_Vec3 = ci;
                    Log.Info("Found Vec4_Constructor_Vec3 as: " + Vec4_Constructor_Vec3.ToString());
                }
            }

            foreach (FieldInfo f in GClass49.Type.BaseType.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (f.IsPublic && f.Name.Length == 11 && f.FieldType == Matrix4)
                {
                    if (GClass49_Base_matrix1 == null)
                    {
                        GClass49_Base_matrix1 = f;
                        Log.Info("Found GClass49_Base_matrix1 field as: " + GClass49_Base_matrix1.ToString());
                    }
                    else if (GClass49_Base_matrix2 == null)
                    {
                        GClass49_Base_matrix2 = f;
                        Log.Info("Found GClass49_Base_matrix2 field as: " + GClass49_Base_matrix2.ToString());
                    }
                    else
                    {
                        Log.Info("Found unexpected third matrix4 field as: " + f.ToString());
                    }
                }
                if(f.IsPublic && f.FieldType.BaseType != null && f.FieldType.BaseType == PlayerBase && f.Name.Length == 15)
                {
                    LocalPlayer = f.FieldType;
                    GClass49_Base_LocalPlayer = f;
                    Log.Info("Found class LocalPlayer as: " + LocalPlayer.ToString());
                    Log.Info("Found field GClass49_Base_LocalPlayer as: " + GClass49_Base_LocalPlayer.ToString());
                }
            }

            foreach (MethodInfo mi in GClass49.Type.BaseType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (mi.IsPublic && mi.ReturnType == Color && mi.GetParameters().Length == 1 && mi.GetParameters()[0].ParameterType == PlayerBase)
                {
                    GClass49_Base_GetPlayerColor = mi;
                    Log.Info("Found method GClass49_Base_GetPlayerColor as: " + GClass49_Base_GetPlayerColor.ToString());
                }
            }

            foreach (FieldInfo fi in GClass49.Type.BaseType.BaseType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if(fi.IsPublic && fi.Name.Length == 15 && Type.GetTypeCode(fi.FieldType) == TypeCode.Double)
                {
                    if (GClass49_Base_Base_ScreenWidth == null)
                    {
                        GClass49_Base_Base_ScreenWidth = fi;
                        Log.Info("Found GClass49_Base_Base_ScreenWidth field as: " + GClass49_Base_Base_ScreenWidth.ToString());
                    }
                    else if (GClass49_Base_Base_ScreenHeight == null)
                    {
                        GClass49_Base_Base_ScreenHeight = fi;
                        Log.Info("Found GClass49_Base_Base_ScreenHeight field as: " + GClass49_Base_Base_ScreenHeight.ToString());
                    }
                    else
                    {
                        Log.Info("Found unexpected third double(screensize) field as: " + fi.ToString());
                    }
                }
            }
        }

        private static bool IsNumeric(Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsStruct(Type t)
        {
            return t.IsValueType && !t.IsEnum && !IsNumeric(t);
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
