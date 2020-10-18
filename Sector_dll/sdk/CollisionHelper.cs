using Sector_dll.cheat;
using Sector_dll.sdk;
using System;

namespace sectorsedge.sdk
{
    class CollisionHelper
    {

        public static object TraceProjectile(object map, object toolType, Vec3 origin, Vec3 velocity)
        {
            return SignatureManager.CollisionHelper_TraceProjectile.Invoke(null, 
                new object[] { map, toolType, origin.ToInternal(), velocity.ToInternal(), 0 });
        }

        public static Vec3 GetShootVector(object player)
        {
            return new Vec3(SignatureManager.CollisionHelper_GetShootVector.Invoke(null, new object[] { player }));
        }

        public static Vec3 GetShootVectorBase(object player)
        {
            double yaw = Player.GetYaw(player);
            double pitch = Player.GetPitch(player);
            if (Player.IsScopped(player) && Player.GetCurrentWeaponType(player) == ToolType.GLauncher)
                pitch += 0.3;
            return Vec3.FromAngle(pitch, yaw) * Weapon.GetVelocity(Player.GetCurrentWeapon(player));
        }

    }
}
