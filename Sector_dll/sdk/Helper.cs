using Sector_dll.cheat;
using System;
using System.Runtime.CompilerServices;

namespace Sector_dll.sdk
{
    class Helper
    {

        public static object GetEquippedScopeType(object player, object weaponType)
        {
            return SignatureManager.Helper1_GetEquippedScope.Invoke(null, new object[] { player, weaponType });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Lerp(double a, double b, double c)
        {
            return a + c * (b - a);
        }

        public static Vec3 Lerp(in Vec3 firstVector, in Vec3 secondVector, double by)
        {
            double num = Lerp(firstVector.x, secondVector.x, by);
            double num2 = Lerp(firstVector.y, secondVector.y, by);
            double num3 = Lerp(firstVector.z, secondVector.z, by);
            return new Vec3(num, num2, num3);
        }

        public static Vec2 VectorAngles(Vec3 vec)
        {
            double yaw = -Math.Atan2(vec.z, vec.x) - Math.PI / 2;
            double pitch = -Math.Asin(vec.y / vec.Len());
            return new Vec2(yaw, pitch);
        }

    }
}
