using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StringsDumper
{
    class Program
    {
        private static string loadingPath;

        static Assembly LoadFromGame(object sender, ResolveEventArgs args)
        {
            string assemblyPath = Path.Combine(loadingPath, new AssemblyName(args.Name).Name + ".dll");
            if (!File.Exists(assemblyPath)) return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        static void Main(string[] args)
        {

            try
            {
                if (args.Length != 1)
                {
                    Console.WriteLine("Usage " + Path.GetFileName(Assembly.GetExecutingAssembly().Location) + " <sectorsedge man executable>");
                    Environment.Exit(10);
                    return;
                }
                //Environment.CurrentDirectory = Path.GetDirectoryName(args[0]);

                loadingPath = Path.GetDirectoryName(args[0]);
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromGame);
                
                AssemblyDefinition definition = AssemblyDefinition.ReadAssembly(args[0]);
                Assembly assembly = Assembly.LoadFile(args[0]);

                //Console.WriteLine(GenerateSignature(assembly.GetType("#=z5Yqy3S3egHnqI0lLeEnH1ZMw$GJCKrFMNkluyr8="))); Console.Read();

                MethodInfo getString = null;
                MethodInfo getString2 = null;
                ResolvedType easyantycheat = new ResolvedType("eac", new ClassSignature()
                {
                    nameLength = 43,

                    publicClass = false,
                    abstractClass = true,
                    nestedTypes = 6,

                    privateMethods = 0,
                    publicMethods = 17,
                    staticMethods = 13,

                    publicFields = 0,
                    privateFields = 8,
                    staticFields = 8,
                    readonlyFields = 2,

                    boolFields = 0,
                    byteFields = 0,
                    shortFields = 0,
                    intFields = 0,
                    longFields = 0,
                    floatFields = 0,
                    doubleFields = 0,
                    enumFields = 0,
                    stringFields = 1,
                    ArrayFields = 1,
                    OtherFields = 6
                });
                foreach (Type t in assembly.GetTypes())
                {
                    ClassSignature sig = ClassSignature.GenerateSignature(t);
                    easyantycheat.Update(sig, t);
                    foreach (MethodInfo mi in t.GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
                    {
                        if (mi.Name.Length == 11 && mi.ReturnType == typeof(string) && mi.GetParameters().Length == 1 && mi.GetParameters()[0].ParameterType == typeof(int) &&
                            mi.GetParameters()[0].Name.Length == 31)
                        {
                            getString = mi;
                            Console.WriteLine("Found getString as " + getString);
                        }
                        if (mi.Name.Length == 27 && mi.ReturnType == typeof(string) && mi.GetParameters().Length == 2 && mi.GetParameters()[0].ParameterType == typeof(int) && 
                            mi.GetParameters()[1].ParameterType == typeof(bool) && mi.GetParameters()[0].Name.Length == 31 && mi.GetParameters()[1].Name.Length == 31)
                        {
                            getString2 = mi;
                            Console.WriteLine("Found getString2 as " + getString2);
                        }
                    }
                }
                if(getString == null)
                {
                    throw new Exception("Failed to find getString in " + assembly.FullName);
                }
                if (getString2 == null)
                {
                    throw new Exception("Failed to find getString2 in " + assembly.FullName);
                }
                Console.WriteLine(easyantycheat.ToString());
                if (easyantycheat.Diff > 0)
                {
                    Console.WriteLine(ClassSignature.GenerateSignature(easyantycheat.Type).ToString());
                }
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(easyantycheat.Type.TypeHandle);

                MethodDefinition md = definition.MainModule.GetType(getString.DeclaringType.Name).Methods.First(x => x.Name == getString.Name);
                if (md == null)
                {
                    throw new Exception("Failed to find get MethodDefinition for getString!");
                }

                FileStream fileStream = new FileStream(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "strings.txt"), FileMode.Create);
                StreamWriter sw = new StreamWriter(fileStream);

                int i = 0;
                foreach (var type in definition.MainModule.Types)
                {
                    bool writtenType = false;
                    bool writtenFunc = false;
                    foreach (var m in type.Methods)
                    {
                        if (m.HasBody)
                        {
                            int id = 0;
                            foreach (var il in m.Body.Instructions)
                            {
                                if (il.OpCode == OpCodes.Ldc_I4)
                                {
                                    id = (int)il.Operand;
                                }
                                if (il.OpCode == OpCodes.Call)
                                {
                                    if (il.Operand is MethodReference mRef && string.Equals(mRef.FullName, md.FullName, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        string s = (string)getString.Invoke(null, new object[] { id });
                                        if(!writtenType)
                                        {
                                            sw.WriteLine(type.Name + ":");
                                            writtenType = true;
                                        }
                                        if (!writtenFunc)
                                        {
                                            sw.WriteLine("\t" + m.FullName + ":");
                                            writtenFunc = true;
                                        }
                                        sw.WriteLine("\t\t" + id.ToString() + ": " + s);
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }
                sw.Close();
                fileStream.Close();
                Console.WriteLine("Written " + i + " strings!");
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                Console.WriteLine(sb.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Press anny key to exit");
            Console.Read();
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
    }
}
