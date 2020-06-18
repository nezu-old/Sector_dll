using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll
{
    class Log
    {
        public static bool enabled = false;

        public static void Info(string msg)
        {
            if(enabled)
                Console.WriteLine("[nezu.cc] " + msg);
        }

    }
}
