using System;

namespace Sector_dll.util
{
    class Log
    {
        public static bool enabled = false;

        public static ConsoleColor color = ConsoleColor.Green;

        public static void Info(string msg)
        {
            if (enabled)
            {
                Console.ForegroundColor = color;
                Console.WriteLine("[nezu.cc] " + msg);
                Console.ResetColor();
            }
        }

        public static void Danger(string msg, bool force = false)
        {
            if(enabled || force)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(msg);
                Console.ResetColor();
            }
        }

    }
}
