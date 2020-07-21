using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
    class Bones
    {

        public static List<object> GetBoneList(object bones)
        {
            return (SignatureManager.Bones_Base_BoneList.GetValue(bones) as IEnumerable<object>).Cast<object>().ToList();
        }

        public static Vec3 GetBBMax(object bones)
        {
            return new Vec3(SignatureManager.Bones_Base_Base_BBMax.GetValue(bones));
        }

        public static Vec3 GetBBMin(object bones)
        {
            return new Vec3(SignatureManager.Bones_Base_Base_BBMin.GetValue(bones));
        }

    }
}
