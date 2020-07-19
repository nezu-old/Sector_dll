using Sector_dll.cheat;
using Sector_dll.util;
using System;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace Sector_dll.sdk
{
    class CollisionHelper
    {

        private static object instance;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void CollisionHelperConstructor(Action<object, object> orig, object self, object a1)
        {
            orig(self, a1);
            instance = self;
            Log.Danger("Got CollisionHelper instance as: " + instance.ToString());
        }

        public static object GetBonesWorldSpace(object player)
        {
            return SignatureManager.CollisionHelper_GetBonesWorldSpace.Invoke(instance, new object[] { player });
        }

    }
}
