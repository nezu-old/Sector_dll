﻿using Sector_dll.cheat;
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

        public static string GetName(object bone)
        {
            return (string)SignatureManager.Bone_Name.GetValue(bone);
        }

        public static bool IsHead(object bone)
        {
            return (bool)SignatureManager.Bone_IsHead.GetValue(bone);
        }

        public static float GetRadius(object bone)
        {
            return (float)SignatureManager.Bone_Radius.GetValue(bone);
        }

    }
}
