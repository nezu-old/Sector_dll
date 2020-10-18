using Microsoft.Build.Framework;
using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
    class CollisionEntity
    {

        public static Vec3 GetPosition(object e, bool useTransform = false)
        {
            return useTransform ? GetTransform(e).GetTranslation() : new Vec3(SignatureManager.CollisionEntity_Position.GetValue(e));
        }

        public static Vec3 GetVelocity(object e)
        {
            return new Vec3(SignatureManager.CollisionEntity_Velocity.GetValue(e));
        }

        public static byte GetOwnerID(object e)
        {
            return (byte)SignatureManager.CollisionEntity_OwnerID.GetValue(e);
        }

        //only g lanuncher!
        public static double GetBounceWatchProgress(object e)
        {
            object watch = SignatureManager.CollisionEntity_BounceWatch.GetValue(e);
            if (watch == null) 
                return 0.0;
            return CustomWatch.GetProgress(watch);
        }

        public static int GetLifetime(object e)
        {
            return (int)SignatureManager.CollisionEntity_Get_Lifetime.Invoke(e, new object[] { });
        }

        public static ToolType GetTool(object e)
        {
            return (ToolType)(byte)Convert.ChangeType(SignatureManager.CollisionEntity_type.GetValue(e), typeof(byte));
        }

        public static object GetToolRaw(object e)
        {
            return SignatureManager.CollisionEntity_type.GetValue(e);
        }

        public static Matrix4 GetTransform(object e)
        {
            return new Matrix4(SignatureManager.CollisionEntity_Matrix.GetValue(e), null);
        }

        public static double GetHealth(object e)
        {
            return (double)SignatureManager.CollisionEntity_Health.GetValue(e);
        }

    }
}
