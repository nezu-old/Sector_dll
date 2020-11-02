using Sector_dll.cheat;
using Sector_dll.cheat;

namespace Sector_dll.sdk
{
    class WorldSpaceBone
    {

        public enum Hitbox
        {
            None,
            Head,
            Chest,
            Arm,
            Leg
        }

        public enum Flags
        {
            None = 0,
            Visible = 1,
            Aimboting = 2,
        }

        public Vec3 head;

        public Vec3 tail;

        public float radius;

        public Hitbox hitbox;

        public Flags flags = Flags.None;

        //public string name;

        public WorldSpaceBone(Vec3 head, Vec3 tail, float radius, Hitbox hitbox)
        {
            this.head = head;
            this.tail = tail;
            this.radius = radius;
            this.hitbox = hitbox;
        }
    }
}
