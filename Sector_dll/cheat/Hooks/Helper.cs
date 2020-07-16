using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat.Hooks
{
    class Helper
    {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static double CurrentBloom(Func<object, double> orig, object player)
        {
            double bloom = orig(player);
            bloom *= 0;
            return bloom;
        }

    }
}
