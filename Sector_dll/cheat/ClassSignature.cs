using Microsoft.Build.Utilities;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat
{
    class ClassSignature
    {
        public int nameLength = -1;

        public bool publicClass = true;
        public bool abstractClass = false;

        public int privateMethods = -1;
        public int publicMethods = -1;
        public int staticMethods = -1;

        public int publicFields = -1;
        public int privateFields = -1;
        public int staticFields = -1;
        public int readonlyFields = -1;

        public int boolFields = -1;
        public int byteFields = -1;
        public int shortFields = -1;
        public int intFields = -1;
        public int longFields = -1;
        public int floatFields = -1;
        public int doubleFields = -1;
        public int enumFields = -1;
        public int stringFields = -1;
        public int ArrayFields = -1;
        public int OtherFields = -1;

        public int CalculateDifrence(ClassSignature sig, bool basicInfoMustMatch = true)
        {
            int diff = 0;
            if (basicInfoMustMatch)
            {
                if (this.nameLength != sig.nameLength) return int.MaxValue;
                if (this.publicClass != sig.publicClass) return int.MaxValue;
                if (this.abstractClass != sig.abstractClass) return int.MaxValue;
            }
            else
            {
                if (this.nameLength != sig.nameLength) diff++;
                if (this.publicClass != sig.publicClass) diff++;
                if (this.abstractClass != sig.abstractClass) diff++;
            }

            diff += Math.Abs(this.privateMethods - sig.privateMethods);
            diff += Math.Abs(this.publicMethods - sig.publicMethods);
            diff += Math.Abs(this.staticMethods - sig.staticMethods);

            diff += Math.Abs(this.publicFields - sig.publicFields);
            diff += Math.Abs(this.privateFields - sig.privateFields);
            diff += Math.Abs(this.staticFields - sig.staticFields);
            diff += Math.Abs(this.readonlyFields - sig.readonlyFields);

            diff += Math.Abs(this.boolFields - sig.boolFields);
            diff += Math.Abs(this.byteFields - sig.byteFields);
            diff += Math.Abs(this.shortFields - sig.shortFields);
            diff += Math.Abs(this.intFields - sig.intFields);
            diff += Math.Abs(this.longFields - sig.longFields);
            diff += Math.Abs(this.floatFields - sig.floatFields);
            diff += Math.Abs(this.doubleFields - sig.doubleFields);
            diff += Math.Abs(this.enumFields - sig.enumFields);
            diff += Math.Abs(this.stringFields - sig.stringFields);
            diff += Math.Abs(this.ArrayFields - sig.ArrayFields);
            diff += Math.Abs(this.OtherFields - sig.OtherFields);

            return diff;
        }

        public static ClassSignature GenerateSignature(Type type)
        {
            BindingFlags bindingFlags = 
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.Static;

            MethodInfo[] methods = type.GetMethods(bindingFlags);
            FieldInfo[] fields = type.GetFields(bindingFlags);

            ClassSignature sig = new ClassSignature
            {
                nameLength = type.Name.Length,

                publicClass = type.IsPublic,
                abstractClass = type.IsAbstract,

                privateMethods = methods.Where(x => x.IsPrivate).Count(),
                publicMethods = methods.Where(x => x.IsPublic).Count(),
                staticMethods = methods.Where(x => x.IsStatic).Count(),

                publicFields = fields.Where(x => x.IsPublic).Count(),
                privateFields = fields.Where(x => x.IsPrivate).Count(),
                staticFields = fields.Where(x => x.IsStatic).Count(),
                readonlyFields = fields.Where(x => x.IsInitOnly).Count(),

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
            };

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum) sig.enumFields++;
                else if (field.FieldType.IsArray) sig.ArrayFields++;
                else if (field.FieldType == typeof(string)) sig.stringFields++;
                else switch (Type.GetTypeCode(field.FieldType))
                {
                    case TypeCode.Boolean: 
                        sig.boolFields++; 
                        break;
                    case TypeCode.Byte: 
                    case TypeCode.SByte:
                        sig.byteFields++; 
                        break;
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                        sig.shortFields++;
                        break;
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                        sig.intFields++; 
                        break;
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        sig.longFields++;
                        break;
                    case TypeCode.Single:
                        sig.floatFields++; 
                        break;
                    case TypeCode.Double: 
                        sig.doubleFields++; 
                        break;
                    default: 
                        sig.OtherFields++;
                        break;
                         
                }
            }

            return sig;
        }

        public override string ToString()
        {
            return string.Format(
                "nameLength = {0},\n" +
                "\n" +
                "publicClass = {1},\n" +
                "abstractClass = {2},\n" +
                "\n" +
                "privateMethods = {3},\n" +
                "publicMethods = {4},\n" +
                "staticMethods = {5},\n" +
                "\n" +
                "publicFields = {6},\n" +
                "privateFields = {7},\n" +
                "staticFields = {8},\n" +
                "readonlyFields = {9},\n" +
                "\n" +
                "boolFields = {10},\n" +
                "byteFields = {11},\n" +
                "shortFields = {12},\n" +
                "intFields = {13},\n" +
                "longFields = {14},\n" +
                "floatFields = {15},\n" +
                "doubleFields = {16},\n" +
                "enumFields = {17},\n" +
                "stringFields = {18},\n" +
                "ArrayFields = {19},\n" +
                "OtherFields = {20}\n",
                nameLength,
                publicClass.ToString().ToLower(),
                abstractClass.ToString().ToLower(),
                privateMethods,
                publicMethods,
                staticMethods,
                publicFields,
                privateFields,
                staticFields,
                readonlyFields,
                boolFields,
                byteFields,
                shortFields,
                intFields,
                longFields,
                floatFields,
                doubleFields,
                enumFields,
                stringFields,
                ArrayFields,
                OtherFields
            );
        }
    }
}
