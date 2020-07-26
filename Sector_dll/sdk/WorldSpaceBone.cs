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

        public Vec3 head;

        public Vec3 tail;

        public float radius;

        public Hitbox hitbox;

        public WorldSpaceBone(Vec3 head, Vec3 tail, float radius, Hitbox hitbox)
        {
            this.head = head;
            this.tail = tail;
            this.radius = radius;
            this.hitbox = hitbox;
        }
    }
}
