using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
    class CustomWatch
    {

        public static double GetProgress(object self)
        {
            return (double)SignatureManager.CustomWatch_get_Progress.Invoke(self, new object[] { });
        }

    }
}
