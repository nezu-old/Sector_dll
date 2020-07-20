using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
    class Bone
    {

        public static Vec3 GetHead(object bone)
        {
            return new Vec3(SignatureManager.Bone_Head.GetValue(bone));
        }

        public static Vec3 GetTail(object bone)
        {
            return new Vec3(SignatureManager.Bone_Tail.GetValue(bone));
        }

    }
}
