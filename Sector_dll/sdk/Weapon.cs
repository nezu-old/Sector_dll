using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sectorsedge.sdk
{
    class Weapon
    {

        public static double GetVelocity(object weapon)
        {
            return (double)SignatureManager.Weapon_Velocity.GetValue(weapon);
        }

    }
}
