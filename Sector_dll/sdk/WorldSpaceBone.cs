using Sector_dll.cheat;

namespace Sector_dll.sdk
{
    class WorldSpaceBone
    {

        public static object GetType(object bone)
        {
            return SignatureManager.WorldSpaceBone_type.GetValue(bone); //untested, only 5????
        }

        public static Vec3 GetHead(object bone)
        {
            return new Vec3(SignatureManager.WorldSpaceBone_head.GetValue(bone));
        }

        public static Vec3 GetTail(object bone)
        {
            return new Vec3(SignatureManager.WorldSpaceBone_tail.GetValue(bone));
        }

        public static float GetRadius(object bone)
        {
            return (float)SignatureManager.WorldSpaceBone_radius.GetValue(bone);
        }

        public static int GetID(object bone)
        {
            return (int)SignatureManager.WorldSpaceBone_ID.GetValue(bone);
        }

        public static string GetName(object bone)
        {
            return (string)SignatureManager.WorldSpaceBone_name.GetValue(bone);
        }

    }
}
