using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sector_dll.cheat.Hooks
{
    class Antycheat
    {

        public static bool IsAllowedDirectory(string path)
        {
            string p = path.ToLower();
            return p.Contains(Directory.GetCurrentDirectory().ToLower()) ||
                p.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "vercidium").ToLower()) ||
                p.Contains(Environment.GetFolderPath(Environment.SpecialFolder.Windows).ToLower());
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool FileExists(Func<string, bool> orig, string path)
        {
            if(IsAllowedDirectory(path))
                return orig(path);
            Log.Danger("File.Exists(" + path + ") denied!");
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool DirectoryExists(Func<string, bool> orig, string path)
        {
            if(IsAllowedDirectory(path))
                return orig(path);
            Log.Danger("Directory.Exists(" + path + ") denied!");
            return false;
        }

        public delegate void FileSystemEnumerableIteratorDelegate(object self, string path, string originalUserPath,
            string searchPattern, SearchOption searchOption, object resultHandler, bool checkHost);

        public static FileSystemEnumerableIteratorDelegate origFileSystemEnumerableIterator;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void FileSystemEnumerableIterator(object self, string path, string originalUserPath, 
            string searchPattern, SearchOption searchOption, object resultHandler, bool checkHost)
        {
            //Log.Danger("xd" + path);
            if (IsAllowedDirectory(path))
            {
                origFileSystemEnumerableIterator(self, path, originalUserPath, searchPattern, searchOption, resultHandler, checkHost);
                return;
            }
            Log.Danger("FileSystemEnumerableIterator..ctor(" + path + ") denied!");
            throw new DirectoryNotFoundException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void FileStreamInit(Action<object, string, FileMode, FileAccess, int, bool, FileShare, int, FileOptions, object, string, 
            bool, bool, bool> orig, object self, string path, FileMode mode, FileAccess access, int rights, bool useRights, FileShare share, 
            int bufferSize, FileOptions options, object secAttrs, string msgPath, bool bFromProxy, bool useLongPath, bool checkHost)
        {
            if (IsAllowedDirectory(path))
            {
                orig(self, path, mode, access, rights, useRights, share, bufferSize, options, secAttrs, msgPath, bFromProxy, useLongPath, checkHost);
                return;
            }
            Log.Danger("FileStream.Init(" + path + ") denied!");
            throw new FileNotFoundException();
        }

        public delegate Assembly[] AppDomainnGetAssembliesDeleghate(AppDomain self, bool forIntrospection);

        public static AppDomainnGetAssembliesDeleghate origAppDomainnGetAssemblies;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Assembly[] AppDomainnGetAssemblies(AppDomain self, bool forIntrospection)
        {
            Assembly[] assemblies = origAppDomainnGetAssemblies(self, forIntrospection);
            List<Assembly> assembliesCopy = new List<Assembly>();
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            foreach(Assembly assembly in assemblies)
            {
                if (currentAssembly == assembly)
                {
                    string st = new StackTrace().ToString();
                    if (!st.Contains("MonoMod.RuntimeDetour.Hook"))
                    {
                        Log.Danger("Cheat assembly blocked!");
                        Log.Danger(new StackTrace().ToString());
                    }
                    else
                        assembliesCopy.Add(assembly);
                }
                else
                    assembliesCopy.Add(assembly);
            }
            return assembliesCopy.ToArray();
        }

    }
}
