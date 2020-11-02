using Sector_dll.cheat;
using Sector_dll.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sectorsedge.sdk
{
    class CollisionResult
    {

        public static Vec3 GetBounceVector(object result)
        {
            return new Vec3(SignatureManager.CollisionResult_BounceVector.GetValue(result));
        }

        public static bool DidHitWall(object result)
        {
            object ht = SignatureManager.CollisionResult_HitType.GetValue(result);
            return (byte)Convert.ChangeType(ht, typeof(byte)) == SignatureManager.HitType_HitWall;
        }

    }
}
