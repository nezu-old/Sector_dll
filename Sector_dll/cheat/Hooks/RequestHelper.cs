using Sector_dll.util;
using System;
using System.Runtime.CompilerServices;

namespace Sector_dll.cheat.Hooks
{
    class RequestHelper
    {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static object POST(Func<string, string, bool, object> orig, string url, string body, bool addDomain)
        {
            Log.Info("POST: " + url + " body: " + body);
            //if (body.Contains("devices"))
            //{
            //    System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            //    Log.Info(t.ToString());
            //}
            return orig(url, body, addDomain);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static object GET(Func<string, string> orig, string url)
        {
            Log.Info("GET: " + url);
            return orig(url);
        }

    }
}
