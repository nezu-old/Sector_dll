using Sector_dll.cheat;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace sectorsedge.cheat
{
    class ThirdPerson
    {

        public static Type CreateProxy()
        {

            AssemblyName aName = new AssemblyName("sectorsedge");
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
            ModuleBuilder mb = ab.DefineDynamicModule(aName.Name);

            var tB = mb.DefineType(
                string.Format("Proxy_{0}", Guid.NewGuid().ToString("N")), TypeAttributes.Public | TypeAttributes.Class, SignatureManager.CameraControler.Type);



            return null;
        }

    }
}
