using Sector_dll.cheat;
using System;

namespace sectorsedge.sdk
{
    class InputManager
    {

        public static readonly int Shoot = 26;

        public static bool IsKeyPressed(int key)
        {
            return (bool)SignatureManager.InputManager_IsKeyPressed.Invoke(null, new object[] { Enum.ToObject(SignatureManager.InputManager_KeyType, key) });
        }

        public static void SetKeyPressed(int key, bool val)
        {
            SignatureManager.InputManager_SetKeyPressed.Invoke(null, new object[] { Enum.ToObject(SignatureManager.InputManager_KeyType, key), val });
        }

    }
}
