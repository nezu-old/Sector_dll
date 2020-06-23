using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
    public enum ModType : byte
	{
		Hydraulics,
		LegServos,
		Speed,
		Shielding,
		Building,
		Stealth,
		Explosive,
		None
    }

    static class ModTypeMethods
    {

        public static object ToInternal(this ModType mt)
        {
			if (SignatureManager.ModType == null)
				return null;
			return Enum.ToObject(SignatureManager.ModType, (int)mt);
		}
    }
}
