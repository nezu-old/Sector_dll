using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sector_dll.util
{
    class Util
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Lerp(double a, double b, double c)
        {
            return a + c * (b - a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Map(double x, double in_min, double in_max, double out_min, double out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        public static void DumpShit(Assembly assembly)
        {
            AssemblyDefinition definition = AssemblyDefinition.ReadAssembly(assembly.Location);

            foreach (var type in definition.MainModule.Types)
            {
                foreach (var m in type.Methods)
                {
                    if (m.HasBody)
                    {
                        foreach (var il in m.Body.Instructions)
                        {
                            if (il.OpCode == OpCodes.Call)
                            {
                                var mRef = il.Operand as MethodReference;
                                if(mRef.DeclaringType.Namespace.Contains("Steamworks")) {
                                    Log.Info(mRef.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void DumpStrings(Assembly assembly, MethodInfo mi)
        {
            AssemblyDefinition definition = AssemblyDefinition.ReadAssembly(assembly.Location);

            MethodDefinition md = definition.MainModule.GetType("#=qlP7Rck8fKTTAfxJeTbAdpGzgOJ5BuLGTE8xrRZOLGDs=")
                .Methods.First(x => x.Name == "#=zE9ylfdY=");

            FileStream fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "strings.txt"), FileMode.Create);
            StreamWriter sw = new StreamWriter(fileStream);

            foreach (var type in definition.MainModule.Types)
            {
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
                                var mRef = il.Operand as MethodReference;
                                if (mRef != null && string.Equals(mRef.FullName, md.FullName, StringComparison.InvariantCultureIgnoreCase))
                                {

                                    string s = (string)mi.Invoke(null, new object[] { id, true });
                                    Log.Info(s);
                                    sw.WriteLine(id.ToString() + ": " + s);
                                }
                            }
                        }
                    }
                }
            }
            sw.Close();
            fileStream.Close();

            Log.Danger(md.FullName);

            for (int i = -2001000000; i > -2010000000; i--)
            {
                if (i % 10000 == 0)
                    Console.Title = i.ToString();
                try
                {
                    string x = (string)mi.Invoke(null, new object[] { i, true });
                    if (x.Length < 200)
                        Log.Info(x);
                }
                catch (Exception)
                {
                    //Log.Danger(ex.ToString());
                }
            }
        }

    }
}
