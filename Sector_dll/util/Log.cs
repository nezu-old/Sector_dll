using System;
using System.IO;

namespace Sector_dll.util
{
    class Log
    {
        public static bool enabled = false;

        public static ConsoleColor color = ConsoleColor.Green;

        public static string Prefix = null;

        public static void Info(object msg) => Info(msg.ToString());

        public static void Info(string msg)
        {
            if (enabled)
            {
                Console.ForegroundColor = color;
                Console.WriteLine((Prefix != null ? Prefix + " " : "") + msg);
                Console.ResetColor();
            }
        }

        public static void Danger(object msg, bool force = false) => Danger(msg.ToString(), force);

        public static void Danger(string msg, bool force = false)
        {
            if(enabled || force)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine((Prefix != null ? Prefix + " " : "") + msg);
                Console.ResetColor();
            }
        }

        public static void Debug(object msg) => Debug(msg.ToString());

        public static void Debug(string msg)
        {
            if (enabled)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine((Prefix != null ? Prefix + " " : "") + msg);
                Console.ResetColor();
            }
        }

        public static void Dump(string file, string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(msg);
            Console.ResetColor();
            using (FileStream fileStream = new FileStream(file, FileMode.Append))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine(msg);
                }
            }
        }

    }
}
