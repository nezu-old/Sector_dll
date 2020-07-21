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

        public static double GetScaleForSkin(byte skin)
        {
            //0x00 - 1.82
            //0x01 - 1.84
            //0x02 - 1.85
            //0x03 - 1.85
            //0x04 - 1.825
            //0x05 - 1.82
            //0x06 - non existent
            //0x07 - 1.855
            //0x08 - 1.805
            switch (skin)
            {
                case 0x00:  return 1.82;
                case 0x01:  return 1.84;
                case 0x02:  return 1.85;
                case 0x03:  return 1.85;
                case 0x04:  return 1.825;
                case 0x05:  return 1.82;
                case 0x07:  return 1.855;
                case 0x08:  return 1.805;
                default:    return 1;
            }
        }

    }
}
