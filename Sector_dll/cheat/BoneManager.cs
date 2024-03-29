﻿using Sector_dll.sdk;
using sectorsedge.sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sector_dll.cheat
{
    class BoneManager
    {
        public static List<WorldSpaceBone>[] BoneCache { get; } = new List<WorldSpaceBone>[GameManager.MaxPlayers];

        public static WorldSpaceBone[] HeadBones { get; } = new WorldSpaceBone[GameManager.MaxPlayers];

        public static WorldSpaceBone[] TorsoBones { get; } = new WorldSpaceBone[GameManager.MaxPlayers];

        private static readonly List<object>[,] NormalBones = new List<object>[30, 10]; //ik this is deformers not bones but i like calling it bones

        public static void InvalidateBones()
        {
            for (int i = 0; i < BoneCache.Length; i++)
                BoneCache[i] = null;
        }

        public static void VisibilityCheck(Vec3 from, int i)
        {
            GameManager.PlayerFlags[i] &= ~PlayerFlags.Visible;
            if (BoneCache[i] == null)
                throw new Exception($"BoneCache for index {i} is null");

            object map = GameManager.GetMap();
            object weapon = Enum.ToObject(SignatureManager.WeaponType, (byte)ToolType.None);
            foreach (WorldSpaceBone bone in BoneCache[i])
            {
                object trace = CollisionHelper.TraceProjectile(map, weapon, from, bone.tail - from);
                if (!CollisionResult.DidHitWall(trace))
                {
                    bone.flags |= WorldSpaceBone.Flags.Visible;
                    GameManager.PlayerFlags[i] |= PlayerFlags.Visible;
                }
            }
        }

        public static void SetupBones(object player, int index)
        {
            if (BoneCache[index] != null)
                return;
            BoneCache[index] = new List<WorldSpaceBone>();

            GameManager.SetupBones(GameManager.instance.Target, player);
            byte skinType = Player.GetSkinId(player);
            TeamType team = Player.GetTeam(player);
            Vec3 origin = Player.GetOrigin(player);

            List<object> bones = NormalBones[skinType, (int)team];
            if (bones == null)
            {
                bones = new List<object>();
                List<object> all_bones = Bones.GetBoneList(Player.GetBones(player));
                foreach (object bone in all_bones)
                {
                    string name = Bone.GetName(bone).ToLower();
                    if(name.Contains("finger") || name.Contains("hand") || name.Contains("thumb"))
                        bones.Add(null);//to keep indexes alligned
                    else if (!name.Contains("control") && !name.Contains("contol") && //i have to waste cpu cycles cuz verc can't spell shit XD /s
                        !name.Contains("blade") && !name.Contains("trigger") && !name.Contains("weapon"))
                        bones.Add(bone);
                }
                NormalBones[skinType, (int)team] = bones;
            }

            object[] transforms = Player.GetBoneTransforms(player);

            double scale = (2.7 - 0.15) / Bones.GetScaleForSkin(skinType); // player h(2.7) is diffrent(3.7) for infected but that's dead
            double offset = (1.22 * scale);// + 0.2;

            Matrix4 matrix = Matrix4.CreateRotationX(-1.5707963267948966) * Matrix4.CreateScale(-scale, scale, scale)
                * Matrix4.CreateTranslation(origin + new Vec3(0, offset, 0));

            for (int j = 0; j < bones.Count(); j++)
            {
                object bone = bones[j];
                if (bone == null)
                    continue;
                Matrix4 final_transform = new Matrix4(transforms[j]) * matrix;
                Vec3 bone_head = final_transform * Bone.GetHead(bone);
                Vec3 bone_tail = final_transform * Bone.GetTail(bone);
                bool isHead = Bone.IsHead(bone);
                if (isHead)
                    bone_tail = Helper.Lerp(bone_head, bone_tail, 0.55);

                WorldSpaceBone worldSpaceBone = new WorldSpaceBone(bone_head, bone_tail, Bone.GetRadius(bone),
                    isHead ? WorldSpaceBone.Hitbox.Head : Bone.IsTorso(bone) ? WorldSpaceBone.Hitbox.Chest : WorldSpaceBone.Hitbox.None);

                //worldSpaceBone.name = Bone.GetName(bone);

                BoneCache[index].Add(worldSpaceBone);
                if (isHead)
                    HeadBones[index] = worldSpaceBone;
            }
            if (HeadBones[index] == null) 
                throw new Exception("Failed to find headbone in skin: " + skinType + ", t: " + (byte)team);
        }

    }
}
