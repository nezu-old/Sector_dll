using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
    class Helper
    {

        public static object GetEquippedScopeType(object player, object weaponType)
        {
            if (SignatureManager.Helper1_GetEquippedScope == null)
                return null;
            return SignatureManager.Helper1_GetEquippedScope.Invoke(null, new object[] { player, weaponType });
        }

    }
}
