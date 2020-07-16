using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat.Hooks
{
    class Player
    {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static double RecoilMod(Func<object, double> orig, object self)
        {
            double mod = orig(self);
            mod *= 0;
            return mod;
        }

    }
}
