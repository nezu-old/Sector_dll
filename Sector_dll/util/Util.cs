using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.util
{
    class Util
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Lerp(double a, double b, double c)
        {
            return a + c * (b - a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Map(double x, double in_min, double in_max, double out_min, double out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

    }
}
