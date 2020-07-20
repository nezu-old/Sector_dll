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

    }
}
