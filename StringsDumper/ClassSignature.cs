using System;
using System.Linq;
using System.Reflection;

namespace StringsDumper
{
    class ClassSignature
    {
        public int nameLength = -1;

        public bool publicClass = true;
        public bool abstractClass = false;
        public int nestedTypes = -1;

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

        public int CalculateDifrence(ClassSignature sig, bool basicInfoMustMatch = false)
        {
            int diff = 0;
            if (basicInfoMustMatch)
            {
                if (nameLength != -1 && this.nameLength != sig.nameLength) return int.MaxValue;
                if (this.publicClass != sig.publicClass) return int.MaxValue;
                if (this.abstractClass != sig.abstractClass) return int.MaxValue;
            }
            else
            {
                if (nameLength != -1 && this.nameLength != sig.nameLength) diff++;
                if (this.publicClass != sig.publicClass) diff++;
                if (this.abstractClass != sig.abstractClass) diff++;
            }

            if (nestedTypes != -1) diff += Math.Abs(this.nestedTypes - sig.nestedTypes);

            if (privateMethods != -1) diff += Math.Abs(this.privateMethods - sig.privateMethods);
            if (publicMethods != -1) diff += Math.Abs(this.publicMethods - sig.publicMethods);
            if (staticMethods != -1) diff += Math.Abs(this.staticMethods - sig.staticMethods);

            if (publicFields != -1) diff += Math.Abs(this.publicFields - sig.publicFields);
            if (privateFields != -1) diff += Math.Abs(this.privateFields - sig.privateFields);
            if (staticFields != -1) diff += Math.Abs(this.staticFields - sig.staticFields);
            if (readonlyFields != -1) diff += Math.Abs(this.readonlyFields - sig.readonlyFields);

            if (boolFields != -1) diff += Math.Abs(this.boolFields - sig.boolFields);
            if (byteFields != -1) diff += Math.Abs(this.byteFields - sig.byteFields);
            if (shortFields != -1) diff += Math.Abs(this.shortFields - sig.shortFields);
            if (intFields != -1) diff += Math.Abs(this.intFields - sig.intFields);
            if (longFields != -1) diff += Math.Abs(this.longFields - sig.longFields);
            if (floatFields != -1) diff += Math.Abs(this.floatFields - sig.floatFields);
            if (doubleFields != -1) diff += Math.Abs(this.doubleFields - sig.doubleFields);
            if (enumFields != -1) diff += Math.Abs(this.enumFields - sig.enumFields);
            if (stringFields != -1) diff += Math.Abs(this.stringFields - sig.stringFields);
            if (ArrayFields != -1) diff += Math.Abs(this.ArrayFields - sig.ArrayFields);
            if (OtherFields != -1) diff += Math.Abs(this.OtherFields - sig.OtherFields);

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
                nestedTypes = type.GetNestedTypes(bindingFlags).Count(),

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
                "nestedTypes = {3},\n" +
                "\n" +
                "privateMethods = {4},\n" +
                "publicMethods = {5},\n" +
                "staticMethods = {6},\n" +
                "\n" +
                "publicFields = {7},\n" +
                "privateFields = {8},\n" +
                "staticFields = {9},\n" +
                "readonlyFields = {10},\n" +
                "\n" +
                "boolFields = {11},\n" +
                "byteFields = {12},\n" +
                "shortFields = {13},\n" +
                "intFields = {14},\n" +
                "longFields = {15},\n" +
                "floatFields = {16},\n" +
                "doubleFields = {17},\n" +
                "enumFields = {18},\n" +
                "stringFields = {19},\n" +
                "ArrayFields = {20},\n" +
                "OtherFields = {21}\n",
                nameLength,
                publicClass.ToString().ToLower(),
                abstractClass.ToString().ToLower(),
                nestedTypes,
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
