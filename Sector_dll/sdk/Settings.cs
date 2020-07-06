using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
    class Settings
    {

        public static double GetFov(object self)
        {
            if (SignatureManager.Settings_fov == null)
                return 0;
            return (double)SignatureManager.Settings_fov.GetValue(self);
        }

    }
}
