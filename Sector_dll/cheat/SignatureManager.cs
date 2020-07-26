﻿using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace Sector_dll.cheat
{
    class SignatureManager
    {

        public static ResolvedType Player = new ResolvedType("Player", new ClassSignature() 
        {
            nameLength = 31,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 0,

            privateMethods = 2,
            publicMethods = 114,
            staticMethods = 0,

            publicFields = 120,
            privateFields = 5,
            staticFields = 0,
            readonlyFields = 5,

            boolFields = 26,
            byteFields = 3,
            shortFields = 0,
            intFields = 15,
            longFields = 0,
            floatFields = 1,
            doubleFields = 8,
            enumFields = 10,
            stringFields = 3,
            ArrayFields = 4,
            OtherFields = 58
        });

        public static ResolvedType GClass49 = new ResolvedType("GClass49", new ClassSignature() 
        {
            nameLength = 31,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 3,

            privateMethods = 49,
            publicMethods = 331,
            staticMethods = 0,

            publicFields = 156,
            privateFields = 45,
            staticFields = 5,
            readonlyFields = 6,

            boolFields = 40,
            byteFields = 1,
            shortFields = 0,
            intFields = 16,
            longFields = 1,
            floatFields = 0,
            doubleFields = 24,
            enumFields = 9,
            stringFields = 0,
            ArrayFields = 11,
            OtherFields = 131
        });

        public static ResolvedType DrawingHelper = new ResolvedType("DrawingHelper", new ClassSignature()
        {
            nameLength = 27,

            publicClass = true,
            abstractClass = true,
            nestedTypes = 0,

            privateMethods = 0,
            publicMethods = 17,
            staticMethods = 13,

            publicFields = 77,
            privateFields = 2,
            staticFields = 79,
            readonlyFields = 0,

            boolFields = 0,
            byteFields = 0,
            shortFields = 0,
            intFields = 0,
            longFields = 0,
            floatFields = 0,
            doubleFields = 2,
            enumFields = 0,
            stringFields = 0,
            ArrayFields = 29,
            OtherFields = 48
        });

        public static ResolvedType Vec4 = new ResolvedType("Vec4", new ClassSignature()
        {
            nameLength = 27,

            publicClass = true,
            abstractClass = false,

            privateMethods = 0,
            publicMethods = 7,
            staticMethods = 1,

            publicFields = 5,
            privateFields = 0,
            staticFields = 1,
            readonlyFields = 0,

            boolFields = 0,
            byteFields = 0,
            shortFields = 0,
            intFields = 0,
            longFields = 0,
            floatFields = 0,
            doubleFields = 4,
            enumFields = 0,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 1
        });

        public static ResolvedType Font = new ResolvedType("Font", new ClassSignature()
        {
            nameLength = 27,

            publicClass = true,
            abstractClass = false,

            privateMethods = 0,
            publicMethods = 13,
            staticMethods = 0,

            publicFields = 0,
            privateFields = 3,
            staticFields = 0,
            readonlyFields = 0,

            boolFields = 0,
            byteFields = 0,
            shortFields = 0,
            intFields = 1,
            longFields = 0,
            floatFields = 0,
            doubleFields = 1,
            enumFields = 0,
            stringFields = 1,
            ArrayFields = 0,
            OtherFields = 0
        });

        public static ResolvedType RequestHelper = new ResolvedType("RequestHelper", new ClassSignature()
        {
            nameLength = 39,

            publicClass = true,
            abstractClass = true,
            nestedTypes = 3,

            privateMethods = 1,
            publicMethods = 6,
            staticMethods = 3,

            publicFields = 0,
            privateFields = 0,
            staticFields = 0,
            readonlyFields = 0,

            boolFields = 0,
            byteFields = 0,
            shortFields = 0,
            intFields = 0,
            longFields = 0,
            floatFields = 0,
            doubleFields = 0,
            enumFields = 0,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 0
        });

        public static ResolvedType ConnectionHelper = new ResolvedType("ConnectionHelper", new ClassSignature() 
        {
            nameLength = 27,

            publicClass = true,
            abstractClass = true,

            privateMethods = 2,
            publicMethods = 40,
            staticMethods = 38,

            publicFields = 4,
            privateFields = 2,
            staticFields = 6,
            readonlyFields = 1,

            boolFields = 1,
            byteFields = 1,
            shortFields = 0,
            intFields = 0,
            longFields = 0,
            floatFields = 0,
            doubleFields = 0,
            enumFields = 0,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 4
        });

        public static ResolvedType Map = new ResolvedType("Map", new ClassSignature()
        {
            nameLength = 27,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 0,

            privateMethods = 2,
            publicMethods = 59,
            staticMethods = 3,

            publicFields = 37,
            privateFields = 0,
            staticFields = 0,
            readonlyFields = 0,

            boolFields = 1,
            byteFields = 0,
            shortFields = 0,
            intFields = 9,
            longFields = 0,
            floatFields = 3,
            doubleFields = 1,
            enumFields = 4,
            stringFields = 1,
            ArrayFields = 13,
            OtherFields = 10
        });

        public static ResolvedType CollisionHelper = new ResolvedType("CollisionHelper", new ClassSignature()
        {
            nameLength = 31,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 0,

            privateMethods = 46,
            publicMethods = 185,
            staticMethods = 2,

            publicFields = 128,
            privateFields = 28,
            staticFields = 5,
            readonlyFields = 3,

            boolFields = 34,
            byteFields = 0,
            shortFields = 0,
            intFields = 12,
            longFields = 1,
            floatFields = 0,
            doubleFields = 17,
            enumFields = 8,
            stringFields = 0,
            ArrayFields = 10,
            OtherFields = 106
        });

        public static ResolvedType Helper1 = new ResolvedType("Helper1", new ClassSignature() 
        {
            nameLength = 27,

            publicClass = true,
            abstractClass = true,
            nestedTypes = 0,

            privateMethods = 0,
            publicMethods = 49,
            staticMethods = 45,

            publicFields = 103,
            privateFields = 0,
            staticFields = 103,
            readonlyFields = 0,

            boolFields = 16,
            byteFields = 0,
            shortFields = 0,
            intFields = 29,
            longFields = 0,
            floatFields = 0,
            doubleFields = 4,
            enumFields = 4,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 50
        });

        public static ResolvedType Helper = new ResolvedType("Helper", new ClassSignature()
        {
            nameLength = 39,

            publicClass = true,
            abstractClass = true,
            nestedTypes = 0,

            privateMethods = 2,
            publicMethods = 59,
            staticMethods = 57,

            publicFields = 0,
            privateFields = 0,
            staticFields = 0,
            readonlyFields = 0,

            boolFields = 0,
            byteFields = 0,
            shortFields = 0,
            intFields = 0,
            longFields = 0,
            floatFields = 0,
            doubleFields = 0,
            enumFields = 0,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 0
        });

        public static ResolvedType Bones = new ResolvedType("Bones", new ClassSignature()
        {
            nameLength = 27,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 0,

            privateMethods = 0,
            publicMethods = 28,
            staticMethods = 0,

            publicFields = 20,
            privateFields = 0,
            staticFields = 0,
            readonlyFields = 1,

            boolFields = 7,
            byteFields = 0,
            shortFields = 0,
            intFields = 4,
            longFields = 2,
            floatFields = 0,
            doubleFields = 0,
            enumFields = 1,
            stringFields = 0,
            ArrayFields = 1,
            OtherFields = 8
        });

        public static ResolvedType CustomWatch = new ResolvedType("CustomWatch", new ClassSignature()
        {
            nameLength = 39,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 0,

            privateMethods = 2,
            publicMethods = 23,
            staticMethods = 0,

            publicFields = 2,
            privateFields = 4,
            staticFields = 0,
            readonlyFields = 1,

            boolFields = 1,
            byteFields = 0,
            shortFields = 0,
            intFields = 2,
            longFields = 1,
            floatFields = 0,
            doubleFields = 1,
            enumFields = 0,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 1
        });

        //public static ResolvedType XXX = new ResolvedType("XXX", new ClassSignature());

        public static ResolvedType[] ResolvedTypes = new ResolvedType[]
        {
            Player,
            GClass49,
            DrawingHelper,
            Vec4,
            Font,
            RequestHelper,
            ConnectionHelper,
            //HistoryPlayer,
            Map,
            CollisionHelper,
            Helper1,
            Helper,
            Bones,
            CustomWatch
        };

        public static Type PlayerBase;

        public static FieldInfo PLayer_PlayerLoadout;

        public static FieldInfo Player_BoneTransforms;

        public static ConstructorInfo Player_BotConstructor;

        public static MethodInfo Player_GetBones;

        public static Type LocalPlayer;

        public static MethodInfo LocalPlayer_Update;

        public static FieldInfo PlayerBase_origin;

        public static FieldInfo PlayerBase_health;

        public static FieldInfo PlayerBase_crouching;

        public static FieldInfo PlayerBase_name;

        public static MethodInfo PLayerBase_EitherMod;

        public static MethodInfo PLayerBase_CurrentWeaponType;

        public static MethodInfo PLayerBase_CurrentWeaponIndex;

        public static MethodInfo PlayerBase_RecoilMod;

        public static MethodInfo PlayerBase_Base_SetTeam;

        public static MethodInfo PlayerBase_Base_GetTeam;

        public static FieldInfo PlayerBase_Base_CharacterTexture;

        public static FieldInfo PlayerBase_Base_Pitch;

        public static FieldInfo PlayerBase_Base_Yaw;

        public static FieldInfo PlayerBase_Base_CrouchWatch;

        public static Type CharacterTexture;

        public static FieldInfo CharacterTexture_PlayerType;

        public static MethodInfo GClass49_vmethod_4;

        public static MethodInfo GClass49_getPlayersToXray;

        public static FieldInfo GClass49_player_list;

        public static FieldInfo GClass49_Base_matrix1;

        public static FieldInfo GClass49_Base_matrix2;

        public static FieldInfo GCLass49_Base_Map;

        public static MethodInfo GClass49_Base_GetPlayerColor;

        public static FieldInfo GClass49_Base_LocalPlayer;

        public static MethodInfo GClass49_Base_IsScoped;

        public static MethodInfo GClass49_Base_GetCurrentPLayer;

        public static FieldInfo GClass49_Base_ScopeSizes1; //first, 2x

        public static FieldInfo GClass49_Base_ScopeSizes2; //second, 4x

        public static MethodInfo GClass49_Base_GenerateGlBuffersForPlayer;

        public static FieldInfo GClass49_Base_Base_ScreenWidth;

        public static FieldInfo GClass49_Base_Base_ScreenHeight;

        public static FieldInfo GClass49_Base_Base_Settings;

        public static MethodInfo GClass49_Base_Base_Draw;

        public static Type Settings;

        public static FieldInfo Settings_fov;

        public static ConstructorInfo Font_Constructor;

        public static MethodInfo RequestHelper_POST;

        public static MethodInfo RequestHelper_GET;

        public static Type Drawing;

        public static MethodInfo Drawing_DrawFilledRect;

        public static MethodInfo Drawing_DrawString;

        public static MethodInfo DrawingHelper_DrawRect;

        public static Type Rect;

        public static ConstructorInfo Rect_Constructor_vec2;

        public static ConstructorInfo Rect_Constructor_standalone;

        public static Type Vec2;

        public static ConstructorInfo Vec2_Constructor;

        public static Type Size;

        public static ConstructorInfo Size_Constructor;

        public static Type Color;

        public static ConstructorInfo Color_Constructor_uint;

        public static ConstructorInfo Color_Constructor_byte;

        public static ConstructorInfo Color_Constructor_float;

        public static Type Vec3;

        public static ConstructorInfo Vec3_Constructor_double;

        public static MethodInfo Vec4_Multiply_Matrix4;

        public static ConstructorInfo Vec4_Constructor_standalone;

        public static ConstructorInfo Vec4_Constructor_Vec3;

        public static Type Matrix4;

        public static ConstructorInfo Matrix4_Constructor;

        public static MethodInfo Matrix4_Generate;

        public static MethodInfo Matrix4_Multiply_Matrix4;

        public static Type ModType;

        //public static MethodInfo GenerateHistoryPlayer;

        public static Type MapBase;

        public static Type WorldSpaceBone;

        public static FieldInfo WorldSpaceBone_type;

        public static FieldInfo WorldSpaceBone_head;
        
        public static FieldInfo WorldSpaceBone_tail;

        public static FieldInfo WorldSpaceBone_radius;
        
        public static FieldInfo WorldSpaceBone_ID;

        public static FieldInfo WorldSpaceBone_name;

        public static ConstructorInfo CollisionHelper_Constructor;

        public static MethodInfo CollisionHelper_GetBonesWorldSpace;
        
        public static Type PlayerLoadout;

        public static Type WeaponType;

        public static MethodInfo Helper1_GetEquippedScope;

        public static MethodInfo Helper1_GetProcAddress;

        public static Type ScopeType;

        public static List<MethodInfo> RegQueryValueEx = new List<MethodInfo>();

        public static MethodInfo DiscordCreate;

        public static MethodInfo Helper_CurrentBloom;

        public static Type TeamType;

        public static MethodInfo Bones_Base_GetBoneByName;

        public static FieldInfo Bones_Base_BoneList;

        public static FieldInfo Bones_Base_Base_BBMin;

        public static FieldInfo Bones_Base_Base_BBMax;

        public static Type Bone;

        public static FieldInfo Bone_Head;

        public static FieldInfo Bone_Tail;

        public static FieldInfo Bone_IsHead;

        public static FieldInfo Bone_Radius;

        public static FieldInfo Bone_Name;

        public static Type GameManager;

        public static MethodInfo GameManager_OtherPlayerYOffset;

        public static MethodInfo GameManager_SetupBones;

        public static MethodInfo CustomWatch_get_Progress;

        public static bool FindSignatures(Assembly aassembly)
        {
            //Log.Info("Waiting for debugger to attach");
            //while (!Debugger.IsAttached)
            //{
            //    Thread.Sleep(100);
            //}
            //Log.Info("Debugger attached");
            //Debugger.Break();
            RegQueryValueEx.Clear();
            foreach (Type t in aassembly.GetTypes())
            {
                ClassSignature sig = ClassSignature.GenerateSignature(t);
                foreach (ResolvedType rt in ResolvedTypes)
                    rt.Update(sig, t);
                if(t.IsEnum)
                    foreach(FieldInfo fi in t.GetFields(BindingFlags.Static | BindingFlags.Public))
                    {
                        if(fi.Name == "Aegis")
                        {
                            TeamType = t;
                            Log.Info("Found enum TeamType as: " + TeamType.ToString());
                        }
                    }
                foreach (MethodInfo method in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                {
                    foreach (Attribute attrib in method.GetCustomAttributes())
                    {
                        if (attrib is DllImportAttribute)
                        {
                            DllImportAttribute dllImport = attrib as DllImportAttribute;
                            if (dllImport.EntryPoint == "RegQueryValueEx")
                                RegQueryValueEx.Add(method);
                            if (dllImport.EntryPoint == "DiscordCreate")
                                DiscordCreate = method;
                        }
                    }
                }
            }
            if(TeamType == null) { Log.Info("TeamType is null"); return false; }

            foreach (ResolvedType rt in ResolvedTypes)
            {
                Log.Info(rt.ToString());
                if (rt.Diff > 0)
                {
                    Log.Info(ClassSignature.GenerateSignature(rt.Type).ToString());
                }
            }

            //Thread.Sleep(int.MaxValue - 1);

            PlayerBase = Player.Type.BaseType;
            MapBase = Map.Type.BaseType;
            Log.Info("Found class PlayerBase as: " + PlayerBase.ToString());
            Log.Info("Found class MapBase as: " + MapBase.ToString());
            if (PlayerBase == null) { Log.Info("PlayerBase is null"); return false; }
            if (MapBase == null) { Log.Info("MapBase is null"); return false; }

            //foreach (Type t in aassembly.GetTypes())
            //{
            //    foreach(MethodInfo mi in t.GetMethods(BindingFlags.Public | BindingFlags.Static))
            //    {
            //        if(mi.GetParameters().Length == 1 && mi.GetParameters()[0].ParameterType == PlayerBase && mi.ReturnType == HistoryPlayer.Type)
            //        {
            //            GenerateHistoryPlayer = mi;
            //            Log.Info("Found GenerateHistoryPlayer as: " + GenerateHistoryPlayer.ToString());
            //            break;
            //        }
            //    }
            //}

            //if (GenerateHistoryPlayer == null) { Log.Info("GenerateHistoryPlayer is null"); return false; }

            BindingFlags bindingFlags =
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly;

            foreach (FieldInfo fi in Player.Type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.FieldType.Name.Length == 43 && fi.Name.Length == 15)
                {
                    PLayer_PlayerLoadout = fi;
                    PlayerLoadout = fi.FieldType;
                    Log.Info("Found PLayer_PlayerLoadout as: " + PLayer_PlayerLoadout.ToString());
                    Log.Info("Found class PlayerLoadout as: " + PlayerLoadout.ToString());

                }
                if(fi.FieldType.Name == "Matrix4F[]")
                {
                    Player_BoneTransforms = fi;
                    Log.Info("Found class Player_BoneTransforms as: " + Player_BoneTransforms.ToString());
                }
            }
            if (PLayer_PlayerLoadout == null) { Log.Info("PLayer_PlayerLoadout is null"); return false; }
            if (PlayerLoadout == null) { Log.Info("PlayerLoadout is null"); return false; }
            if (Player_BoneTransforms == null) { Log.Info("Player_BoneTransforms is null"); return false; }

            foreach (ConstructorInfo ci in Player.Type.GetConstructors())
            {
                if(ci.IsPublic && ci.GetParameters().Length == 3 && ci.GetParameters()[1].ParameterType == typeof(byte))
                {
                    Player_BotConstructor = ci;
                    GameManager = ci.GetParameters()[0].ParameterType;
                    Log.Info("Found Player_BotConstructor as: " + Player_BotConstructor.ToString());
                    Log.Info("Found class OfflineGameManager as: " + GameManager.ToString());
                }
            }
            if (Player_BotConstructor == null) { Log.Info("Player_BotConstructor is null"); return false; }
            if (GameManager == null) { Log.Info("OfflineGameManager is null"); return false; }

            foreach(MethodInfo mi in GameManager.GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
            {
                if (mi.Name.Length == 15 && mi.ReturnType == typeof(double) && mi.GetParameters().Length == 1 && 
                    mi.GetParameters()[0].ParameterType == Player.Type)
                {
                    GameManager_OtherPlayerYOffset = mi;
                    Log.Info("Found GameManager_OtherPlayerYOffset as: " + GameManager_OtherPlayerYOffset.ToString());
                }
            }
            if (GameManager_OtherPlayerYOffset == null) { Log.Info("GameManager_OtherPlayerYOffset is null"); return false; }

            foreach (MethodInfo mi in GameManager.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (mi.Name.Length == 15 && mi.ReturnType == typeof(void) && mi.GetParameters().Length == 1 &&
                    mi.GetParameters()[0].ParameterType == Player.Type && mi.GetMethodBody().GetILAsByteArray().Length > 300)
                {
                    GameManager_SetupBones = mi;
                    Log.Info("Found GameManager_SetupBones as: " + GameManager_SetupBones.ToString());
                }
            }
            if (GameManager_SetupBones == null) { Log.Info("GameManager_SetupBones is null"); return false; }

            foreach (MethodInfo mi in Player.Type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (mi.ReturnType == Bones.Type && mi.GetGenericArguments().Length == 0)
                {
                    Player_GetBones = mi;
                    Log.Info("Found Player_GetBones as: " + Player_GetBones.ToString());
                }
            }
            if(Player_GetBones == null) { Log.Info("Player_GetBones is null"); return false; }

            foreach (FieldInfo fi in PlayerLoadout.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.FieldType.IsEnum && WeaponType == null)
                {
                    WeaponType = fi.FieldType;
                    Log.Info("Found enum WeaponType as: " + WeaponType.ToString());
                }
            }
            if (WeaponType == null) { Log.Info("WeaponType is null"); return false; }

            foreach (FieldInfo f in PlayerBase.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (f.IsPublic && f.Name.Length == 11 && IsStruct(f.FieldType))
                {
                    PlayerBase_origin = f;
                    Vec3 = f.FieldType;
                    Log.Info("Found PlayerBase.origin as: " + PlayerBase_origin.ToString());
                    Log.Info("Found class Vec3 as: " + Vec3.ToString());
                }
                if (f.IsPrivate && !f.IsFamily && f.FieldType.IsArray && f.Name.Length == 23)
                {
                    PlayerBase_crouching = f;
                    Log.Info("Found PlayerBase_crouching as: " + PlayerBase_crouching.ToString());
                }
            }
            if (PlayerBase_origin == null) { Log.Info("PlayerBase_origin is null"); return false; }
            if (Vec3 == null) { Log.Info("Vec3 is null"); return false; }
            //if (PlayerBase_crouching == null) { Log.Info("PlayerBase_crouching is null"); return false; }//missing

            foreach (MethodInfo mi in PlayerBase.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (mi.IsPublic && mi.Name.Length == 23 && mi.GetParameters().Length == 1 &&
                    mi.ReturnType == typeof(bool) && mi.GetParameters()[0].ParameterType.Name.Length == 39)
                {
                    PLayerBase_EitherMod = mi;
                    ModType = mi.GetParameters()[0].ParameterType;
                    Log.Info("Found PLayerBase_EitherMod as: " + PLayerBase_EitherMod.ToString());
                    Log.Info("Found ModType as: " + ModType.ToString());
                }
                if (mi.ReturnType == WeaponType && mi.GetGenericArguments().Length == 0)
                {
                    PLayerBase_CurrentWeaponType = mi;
                    Log.Info("Found PLayerBase_CurrentWeaponType as: " + PLayerBase_CurrentWeaponType.ToString());
                }
                if (mi.ReturnType == typeof(int) && mi.GetParameters().Length == 0 && mi.Name.Length == 15
                    && mi.GetMethodBody().LocalVariables.Count == 0 && mi.GetMethodBody().MaxStackSize == 8
                    && mi.GetMethodBody().GetILAsByteArray().Length > 20)
                {
                    PLayerBase_CurrentWeaponIndex = mi;
                    Log.Info("Found PLayerBase_CurrentWeaponIndex as: " + PLayerBase_CurrentWeaponIndex.ToString());
                }
                if (mi.ReturnType == typeof(double) && mi.GetParameters().Length == 0 && mi.Name.Length == 23 && mi.DeclaringType == PlayerBase)
                {
                    PlayerBase_RecoilMod = mi;
                    Log.Info("Found PLayerBase_RecoilMod as: " + PlayerBase_RecoilMod.ToString());
                }
                if(mi.ReturnType == typeof(void) && mi.GetParameters().Length == 1 && mi.GetParameters()[0].ParameterType == TeamType)
                {
                    PlayerBase_Base_SetTeam = mi;
                    Log.Info("Found PLayerBase_Base_SetTeam as: " + PlayerBase_Base_SetTeam.ToString());
                }
                if(mi.ReturnType == TeamType && mi.Name.Length == 15 && mi.GetParameters().Length == 0)
                {
                    PlayerBase_Base_GetTeam = mi;
                    Log.Info("Found PLayerBase_Base_GetTeam as: " + PlayerBase_Base_GetTeam.ToString());
                }
            }
            if (PLayerBase_EitherMod == null) { Log.Info("PLayerBase_EitherMod is null"); return false; }
            if (ModType == null) { Log.Info("ModType is null"); return false; }
            if (PLayerBase_CurrentWeaponType == null) { Log.Info("PLayerBase_CurrentWeaponType is null"); return false; }
            if (PLayerBase_CurrentWeaponIndex == null) { Log.Info("PLayerBase_CurrentWeaponIndex is null"); return false; }
            if (PlayerBase_RecoilMod == null) { Log.Info("PlayerBase_RecoilMod is null"); return false; }
            if (PlayerBase_Base_SetTeam == null) { Log.Info("PLayerBase_Base_SetTeam is null"); return false; }
            if (PlayerBase_Base_GetTeam == null) { Log.Info("PLayerBase_Base_GetTeam is null"); return false; }

            foreach (ConstructorInfo ci in PlayerBase.GetConstructors())
            {
                if (ci.GetParameters().Length == 0)
                {
                    object player = ci.Invoke(new object[] { });
                    foreach (FieldInfo fi in player.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (fi.IsPublic && fi.FieldType == typeof(string) && fi.Name.Length == 11)
                        {
                            string s = (string)fi.GetValue(player);
                            if (s == "Player")
                            {
                                PlayerBase_name = fi;
                                Log.Info("Found PlayerBase_name as: " + PlayerBase_name.ToString());
                            }
                        }
                        if (fi.IsPublic && fi.FieldType == typeof(double) && fi.Name.Length == 15)
                        {
                            double d = (double)fi.GetValue(player);
                            if (d == 100.0)
                            {
                                PlayerBase_health = fi;
                                Log.Info("Found PlayerBase_health as: " + PlayerBase_health.ToString());
                            }
                        }
                    }
                    player = null; //dispose
                    break;
                }
            }
            if (PlayerBase_name == null) { Log.Info("PlayerBase_name is null"); return false; }
            if (PlayerBase_health == null) { Log.Info("PlayerBase_health is null"); return false; }

            foreach (FieldInfo fi in PlayerBase.BaseType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if(fi.Name.Length == 15 && fi.FieldType.Name.Length == 39 && IsStruct(fi.FieldType))
                {
                    CharacterTexture = fi.FieldType;
                    PlayerBase_Base_CharacterTexture = fi;
                    Log.Info("Found struct CharacterTexture as: " + CharacterTexture.ToString());
                    Log.Info("Found PlayerBase_Base_CharacterTexture as: " + PlayerBase_Base_CharacterTexture.ToString());
                }
            }
            if (CharacterTexture == null) { Log.Info("CharacterTexture is null"); return false; }
            if (PlayerBase_Base_CharacterTexture == null) { Log.Info("PlayerBase_Base_CharacterTexture is null"); return false; }

            foreach(FieldInfo fi in PlayerBase.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (fi.FieldType.IsArray && fi.FieldType == typeof(double[]))
                {
                    if(PlayerBase_Base_Pitch == null)
                    {
                        PlayerBase_Base_Pitch = fi;
                        Log.Info("Found PlayerBase_Base_Pitch as: " + PlayerBase_Base_Pitch.ToString());
                    }
                    else if(PlayerBase_Base_Yaw == null)
                    {
                        PlayerBase_Base_Yaw = fi;
                        Log.Info("Found PlayerBase_Base_Yaw as: " + PlayerBase_Base_Yaw.ToString());
                    }
                    else
                    {
                        Log.Info("PlayerBase_Base has more double[]!");
                        return false;
                    }
                }
                
            }
            if (PlayerBase_Base_Pitch == null) { Log.Info("PlayerBase_Base_Pitch is null"); return false; }
            if (PlayerBase_Base_Yaw == null) { Log.Info("PlayerBase_Base_Yaw is null"); return false; }

            foreach (FieldInfo fi in PlayerBase.BaseType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.FieldType == CustomWatch.Type && fi.Name.Length == 23 && fi.IsInitOnly)
                {
                    PlayerBase_Base_CrouchWatch = fi;
                    Log.Info("Found PlayerBase_Base_CrouchWatch as: " + PlayerBase_Base_CrouchWatch.ToString());
                }
            }
            if (PlayerBase_Base_CrouchWatch == null) { Log.Info("PlayerBase_Base_CrouchWatch is null"); return false; }

            ConstructorInfo CharacterTexture_Constructor = CharacterTexture.GetConstructors().First();
            foreach(FieldInfo fi in CharacterTexture.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if(fi.FieldType == CharacterTexture_Constructor.GetParameters()[0].ParameterType)
                {
                    CharacterTexture_PlayerType = fi;
                    Log.Info("Found CharacterTexture_PlayerType as: " + CharacterTexture_PlayerType.ToString());
                }
            }
            if (CharacterTexture_PlayerType == null) { Log.Info("CharacterTexture_PlayerType is null"); return false; }

            foreach (ConstructorInfo ci in Vec3.GetConstructors())
            {
                if (ci.GetParameters().Length == 3)
                {
                    Vec3_Constructor_double = ci;
                    Log.Info("Found Vec3_Constructor_double as: " + Vec3_Constructor_double.ToString());
                }
            }
            if (Vec3_Constructor_double == null) { Log.Info("Vec3_Constructor_double is null"); return false; }

            foreach (MethodInfo m in GClass49.Type.GetMethods(bindingFlags))
            {
                if (m.IsPublic && m.IsVirtual && !m.IsConstructor && m.ReturnType == typeof(void) && m.GetParameters().Length == 1
                    && m.Name.Length == 15 && m.GetParameters()[0].ParameterType.Name.Length == 43)
                {
                    GClass49_vmethod_4 = m;
                    Drawing = m.GetParameters()[0].ParameterType;
                    Log.Info("Found GClass49.vmethod_4 as: " + m.ToString());
                    Log.Info("Found class Drawing as: " + Drawing.ToString());
                }
                if (m.IsPrivate && m.GetParameters().Length == 0 && m.ReturnType.IsGenericType && m.Name.Length == 27
                    && m.ReturnType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    GClass49_getPlayersToXray = m;
                    Log.Info("Found GClass49.getPlayersToXray as: " + m.ToString());
                }

            }
            if (GClass49_vmethod_4 == null) { Log.Info("GClass49_vmethod_4 is null"); return false; }
            if (Drawing == null) { Log.Info("Drawing is null"); return false; }
            if (GClass49_getPlayersToXray == null) { Log.Info("GClass49_getPlayersToXray is null"); return false; }

            bool second = false;
            foreach (FieldInfo f in GClass49.Type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (f.IsPublic && f.Name.Length == 23 && f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(List<>)
                    && f.FieldType.GetGenericArguments().Length == 1 && f.FieldType.GetGenericArguments()[0] == Player.Type)
                {
                    if (second)
                    {
                        GClass49_player_list = f;
                        Log.Info("Found GClass49.player_list field as: " + f.ToString());
                        break;
                    }
                    else
                        second = true;
                }
            }
            if (GClass49_player_list == null) { Log.Info("GClass49_player_list is null"); return false; }

            foreach (MethodInfo m in DrawingHelper.Type.GetMethods(bindingFlags))
            {
                if (m.IsPublic && m.IsStatic && m.ReturnType == typeof(void) && m.GetParameters().Length == 4
                    && m.GetParameters()[0].ParameterType == Drawing.BaseType && IsStruct(m.GetParameters()[1].ParameterType)
                    && m.GetParameters()[2].ParameterType == typeof(double) && IsStruct(m.GetParameters()[3].ParameterType))
                {
                    DrawingHelper_DrawRect = m;
                    Rect = m.GetParameters()[1].ParameterType;
                    Color = m.GetParameters()[3].ParameterType;
                    Log.Info("Found DrawingHelper.DrawRect as: " + m.ToString());
                    Log.Info("Found class Rect as: " + Rect.ToString());
                    Log.Info("Found class Color as: " + Color.ToString());
                    break;
                }
            }
            if (DrawingHelper_DrawRect == null) { Log.Info("DrawingHelper_DrawRect is null"); return false; }
            if (Rect == null) { Log.Info("Rect is null"); return false; }
            if (Color == null) { Log.Info("Color is null"); return false; }

            foreach (MethodInfo mi in Drawing.GetMethods(bindingFlags))
            {
                if (mi.IsPublic && mi.Name.Length == 15 && mi.GetParameters().Length == 2
                    && mi.GetParameters()[0].ParameterType == Rect && mi.GetParameters()[1].ParameterType == Color)
                {
                    Drawing_DrawFilledRect = mi;
                    Log.Info("Found Drawing_DrawFilledRect as: " + Drawing_DrawFilledRect.ToString());
                }
                if (mi.IsPublic && mi.Name.Length == 11 && mi.GetParameters().Length == 7 && mi.GetParameters()[0].ParameterType == typeof(string))
                {
                    Drawing_DrawString = mi;
                    Log.Info("Found Drawing_DrawString as: " + Drawing_DrawString.ToString());
                }
            }
            if (Drawing_DrawFilledRect == null) { Log.Info("Drawing_DrawFilledRect is null"); return false; }
            if (Drawing_DrawString == null) { Log.Info("Drawing_DrawString is null"); return false; }

            foreach (ConstructorInfo ci in Font.Type.GetConstructors())
            {
                if (ci.IsPublic && ci.GetParameters().Length == 3 && ci.GetParameters()[2].ParameterType == typeof(int))
                {
                    Font_Constructor = ci;
                    Log.Info("Found Font_Constructor constructor as: " + Font_Constructor.ToString());
                }
            }
            if (Font_Constructor == null) { Log.Info("Font_Constructor is null"); return false; }

            foreach (MethodInfo mi in RequestHelper.Type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (mi.GetParameters().Length == 3)
                {
                    RequestHelper_POST = mi;
                    Log.Info("Found RequestHelper_POST as: " + RequestHelper_POST.ToString());
                }
                if (mi.GetParameters().Length == 1 && mi.GetParameters()[0].ParameterType == typeof(string))
                {
                    RequestHelper_GET = mi;
                    Log.Info("Found RequestHelper_GET as: " + RequestHelper_GET.ToString());
                }
            }
            if (RequestHelper_POST == null) { Log.Info("RequestHelper_POST is null"); return false; }
            if (RequestHelper_GET == null) { Log.Info("RequestHelper_GET is null"); return false; }

            foreach (ConstructorInfo ci in Rect.GetConstructors())
            {
                if (ci.GetParameters().Length == 2)
                {
                    Rect_Constructor_vec2 = ci;
                    Vec2 = ci.GetParameters()[0].ParameterType;
                    Size = ci.GetParameters()[1].ParameterType;
                    Log.Info("Found Rect_Constructor_vec2 as: " + Rect_Constructor_vec2.ToString());
                    Log.Info("Found class Vec2 as: " + Vec2.ToString());
                    Log.Info("Found class Size as: " + Size.ToString());
                }
                else if (ci.GetParameters().Length == 4)
                {
                    Rect_Constructor_standalone = ci;
                    Log.Info("Found Rect_Constructor_standalone as: " + Rect_Constructor_standalone.ToString());
                }
            }
            if (Rect_Constructor_vec2 == null) { Log.Info("Rect_Constructor_vec2 is null"); return false; }
            if (Vec2 == null) { Log.Info("Vec2 is null"); return false; }
            if (Size == null) { Log.Info("Size is null"); return false; }
            if (Rect_Constructor_standalone == null) { Log.Info("Rect_Constructor_standalone is null"); return false; }

            Vec2_Constructor = Vec2.GetConstructors()[0];
            Size_Constructor = Size.GetConstructors()[0];
            if (Vec2_Constructor == null) { Log.Info("Vec2_Constructor is null"); return false; }
            if (Size_Constructor == null) { Log.Info("Size_Constructor is null"); return false; }
            Log.Info("Found Vec2_Constructor as: " + Vec2_Constructor.ToString());
            Log.Info("Found Size_Constructor as: " + Size_Constructor.ToString());

            foreach (ConstructorInfo ci in Color.GetConstructors())
            {
                if (ci.GetParameters().Length == 1)
                {
                    Color_Constructor_uint = ci;
                    Log.Info("Found Color_Constructor_uint as: " + Color_Constructor_uint.ToString());
                }
                else if (ci.GetParameters().Length == 4)
                {
                    if (ci.GetParameters()[0].ParameterType == typeof(byte))
                    {
                        Color_Constructor_byte = ci;
                        Log.Info("Found Color_Constructor_byte as: " + Color_Constructor_byte.ToString());
                    }
                    else if (ci.GetParameters()[0].ParameterType == typeof(float))
                    {
                        Color_Constructor_float = ci;
                        Log.Info("Found Color_Constructor_float as: " + Color_Constructor_float.ToString());
                    }
                }
            }
            if (Color_Constructor_uint == null) { Log.Info("Color_Constructor_uint is null"); return false; }
            if (Color_Constructor_byte == null) { Log.Info("Color_Constructor_byte is null"); return false; }
            if (Color_Constructor_float == null) { Log.Info("Color_Constructor_float is null"); return false; }

            foreach (MethodInfo mi in Vec4.Type.GetMethods())
            {
                if (mi.Name == "op_Multiply")
                {
                    Vec4_Multiply_Matrix4 = mi;
                    Matrix4 = mi.GetParameters()[1].ParameterType;
                    Log.Info("Found Vec4_Multiply_Matrix4 as: " + Vec4_Multiply_Matrix4.ToString());
                    Log.Info("Found class Matrix4 as: " + Matrix4.ToString());
                }
            }
            if (Vec4_Multiply_Matrix4 == null) { Log.Info("Vec4_Multiply_Matrix4 is null"); return false; }
            if (Matrix4 == null) { Log.Info("Matrix4 is null"); return false; }

            foreach (MethodInfo mi in Matrix4.GetMethods())
            {
                if (mi.Name == "op_Multiply")
                {
                    Matrix4_Multiply_Matrix4 = mi;
                    Log.Info("Found Matrix4_Multiply_Matrix4 as: " + Matrix4_Multiply_Matrix4.ToString());
                }
                if (mi.GetParameters().Length == 4 && mi.GetParameters()[0].ParameterType == typeof(double)
                     && mi.GetParameters()[1].ParameterType == typeof(double) && mi.GetParameters()[2].ParameterType == typeof(double)
                      && mi.GetParameters()[3].ParameterType == typeof(double) && mi.Name.Length == 15)
                {
                    Matrix4_Generate = mi;
                    Log.Info("Found Matrix4_Generate as: " + Matrix4_Generate.ToString());
                }
            }
            if (Matrix4_Multiply_Matrix4 == null) { Log.Info("Matrix4_Multiply_Matrix4 is null"); return false; }
            if (Matrix4_Generate == null) { Log.Info("Matrix4_Generate is null"); return false; }

            foreach (ConstructorInfo ci in Matrix4.GetConstructors())
            {
                if (ci.GetParameters().Length == 16)
                {
                    Matrix4_Constructor = ci;
                    Log.Info("Found Matrix4_Constructor as: " + Matrix4_Constructor.ToString());
                }
            }
            if (Matrix4_Constructor == null) { Log.Info("Matrix4_Constructor is null"); return false; }

            foreach (ConstructorInfo ci in Vec4.Type.GetConstructors())
            {
                if (ci.GetParameters().Length == 4)
                {
                    Vec4_Constructor_standalone = ci;
                    Log.Info("Found Vec4_Constructor_standalone as: " + Vec4_Constructor_standalone.ToString());
                }
                if (ci.GetParameters()[0].ParameterType == Vec3)
                {
                    Vec4_Constructor_Vec3 = ci;
                    Log.Info("Found Vec4_Constructor_Vec3 as: " + Vec4_Constructor_Vec3.ToString());
                }
            }
            if (Vec4_Constructor_standalone == null) { Log.Info("Vec4_Constructor_standalone is null"); return false; }
            if (Vec4_Constructor_Vec3 == null) { Log.Info("Vec4_Constructor_Vec3 is null"); return false; }

            foreach (FieldInfo f in GClass49.Type.BaseType.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (f.IsPublic && f.Name.Length == 11 && f.FieldType == Matrix4)
                {
                    if (GClass49_Base_matrix1 == null)
                    {
                        GClass49_Base_matrix1 = f;
                        Log.Info("Found GClass49_Base_matrix1 field as: " + GClass49_Base_matrix1.ToString());
                    }
                    else if (GClass49_Base_matrix2 == null)
                    {
                        GClass49_Base_matrix2 = f;
                        Log.Info("Found GClass49_Base_matrix2 field as: " + GClass49_Base_matrix2.ToString());
                    }
                    else
                    {
                        Log.Info("Found unexpected third matrix4 field as: " + f.ToString());
                    }
                }
                if (f.IsPublic && f.FieldType.BaseType != null && f.FieldType.BaseType == PlayerBase && f.Name.Length == 15)
                {
                    LocalPlayer = f.FieldType;
                    GClass49_Base_LocalPlayer = f;
                    Log.Info("Found class LocalPlayer as: " + LocalPlayer.ToString());
                    Log.Info("Found GClass49_Base_LocalPlayer as: " + GClass49_Base_LocalPlayer.ToString());
                }
                if (f.IsPublic && f.FieldType == Map.Type)
                {
                    GCLass49_Base_Map = f;
                    Log.Info("Found class GCLass49_Base_Map as: " + GCLass49_Base_Map.ToString());
                }
            }
            if (GClass49_Base_matrix1 == null) { Log.Info("GClass49_Base_matrix1 is null"); return false; }
            if (GClass49_Base_matrix2 == null) { Log.Info("GClass49_Base_matrix2 is null"); return false; }
            if (LocalPlayer == null) { Log.Info("LocalPlayer is null"); return false; }
            if (GCLass49_Base_Map == null) { Log.Info("GCLass49_Base_Map is null"); return false; }

            foreach (MethodInfo mi in LocalPlayer.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)){
                if(mi.Name.Length == 15 && mi.ReturnType == typeof(void) && mi.GetParameters().Length == 1 &&
                    mi.GetParameters()[0].ParameterType.IsClass)
                {
                    LocalPlayer_Update = mi;
                    Log.Info("Found LocalPlayer_Update as: " + LocalPlayer_Update.ToString());
                }
            }
            if (LocalPlayer_Update == null) { Log.Info("LocalPlayer_Update is null"); return false; }

            foreach (FieldInfo fi in GClass49.Type.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Static))
            {
                if (fi.FieldType.IsGenericType && fi.FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>)
                    && fi.FieldType.GetGenericArguments()[0] == WeaponType && fi.FieldType.GetGenericArguments()[1] == Vec2)
                {
                    if (GClass49_Base_ScopeSizes1 == null)
                    {
                        GClass49_Base_ScopeSizes1 = fi;
                        Log.Info("Found GClass49_Base_ScopeSizes1 as: " + GClass49_Base_ScopeSizes1.ToString());
                    }
                    else if (GClass49_Base_ScopeSizes2 == null)
                    {
                        GClass49_Base_ScopeSizes2 = fi;
                        Log.Info("Found GClass49_Base_ScopeSizes2 as: " + GClass49_Base_ScopeSizes2.ToString());
                    }
                    else
                    {
                        Log.Info("Found unexpected third GClass49_Base_ScopeSizes field as: " + fi.ToString());
                    }
                }
            }
            if (GClass49_Base_ScopeSizes1 == null) { Log.Info("GClass49_Base_ScopeSizes1 is null"); return false; }
            if (GClass49_Base_ScopeSizes2 == null) { Log.Info("GClass49_Base_ScopeSizes2 is null"); return false; }

            foreach (MethodInfo mi in GClass49.Type.BaseType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (mi.IsPublic && mi.ReturnType == Color && mi.GetParameters().Length == 1
                    && mi.GetParameters()[0].ParameterType == PlayerBase.BaseType)
                {
                    GClass49_Base_GetPlayerColor = mi;
                    Log.Info("Found GClass49_Base_GetPlayerColor as: " + GClass49_Base_GetPlayerColor.ToString());
                }
                if (mi.ReturnType == typeof(bool) && mi.GetParameters().Length == 0 && mi.Name.Length == 15
                    && mi.GetMethodBody().LocalVariables.Count == 1 && mi.GetMethodBody().MaxStackSize == 9)
                {
                    GClass49_Base_IsScoped = mi;
                    Log.Info("Found GClass49_Base_IsScoped as: " + GClass49_Base_IsScoped.ToString());
                }
                if (mi.ReturnType == PlayerBase && mi.GetParameters().Length == 0 && mi.Name.Length == 15)
                {
                    GClass49_Base_GetCurrentPLayer = mi;
                    Log.Info("Found GClass49_Base_GetCurrentPLayer as: " + GClass49_Base_GetCurrentPLayer.ToString());
                }
                if (mi.ReturnType == typeof(uint) && mi.GetParameters().Length == 0 && mi.Name.Length == 27)
                {
                    GClass49_Base_GenerateGlBuffersForPlayer = mi;
                    Log.Info("Found GClass49_Base_GenerateGlBuffersForPlayer as: " + GClass49_Base_GenerateGlBuffersForPlayer.ToString());
                }
            }
            if (GClass49_Base_GetPlayerColor == null) { Log.Info("GClass49_Base_GetPlayerColor is null"); return false; }
            if (GClass49_Base_IsScoped == null) { Log.Info("GClass49_Base_IsScoped is null"); return false; }
            if (GClass49_Base_GetCurrentPLayer == null) { Log.Info("GClass49_Base_GetCurrentPLayer is null"); return false; }
            if (GClass49_Base_GenerateGlBuffersForPlayer == null) { Log.Info("GClass49_Base_GenerateGlBuffersForPlayer is null"); return false; }

            foreach (MethodInfo mi in GClass49.Type.BaseType.BaseType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if(mi.ReturnType == typeof(void) && mi.Name.Length == 23 && mi.GetParameters().Length == 1 &&
                    mi.GetParameters()[0].ParameterType == Drawing)
                {
                    GClass49_Base_Base_Draw = mi;
                    Log.Info("Found GClass49_Base_Base_Draw as: " + GClass49_Base_Base_Draw.ToString());
                }
            }
            if(GClass49_Base_Base_Draw == null) { Log.Info("GClass49_Base_Base_Draw is null"); return false; }

            foreach (FieldInfo fi in GClass49.Type.BaseType.BaseType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.IsPublic && fi.Name.Length == 15 && Type.GetTypeCode(fi.FieldType) == TypeCode.Double)
                {
                    if (GClass49_Base_Base_ScreenWidth == null)
                    {
                        GClass49_Base_Base_ScreenWidth = fi;
                        Log.Info("Found GClass49_Base_Base_ScreenWidth field as: " + GClass49_Base_Base_ScreenWidth.ToString());
                    }
                    else if (GClass49_Base_Base_ScreenHeight == null)
                    {
                        GClass49_Base_Base_ScreenHeight = fi;
                        Log.Info("Found GClass49_Base_Base_ScreenHeight field as: " + GClass49_Base_Base_ScreenHeight.ToString());
                    }
                    else
                    {
                        Log.Info("Found unexpected third double(screensize) field as: " + fi.ToString());
                    }
                }
                if (fi.FieldType.Name.Length == 31 && fi.Name.Length == 11)
                {
                    GClass49_Base_Base_Settings = fi;
                    Settings = fi.FieldType;
                    Log.Info("Found GClass49_Base_Base_Settings field as: " + GClass49_Base_Base_Settings.ToString());
                    Log.Info("Found class Settings as: " + Settings.ToString());
                }
            }
            if (GClass49_Base_Base_ScreenWidth == null) { Log.Info("GClass49_Base_Base_ScreenWidth is null"); return false; }
            if (GClass49_Base_Base_ScreenHeight == null) { Log.Info("GClass49_Base_Base_ScreenHeight is null"); return false; }
            if (GClass49_Base_Base_Settings == null) { Log.Info("GClass49_Base_Base_Settings is null"); return false; }
            if (Settings == null) { Log.Info("Settings is null"); return false; }

            foreach (FieldInfo fi in Settings.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.FieldType == typeof(double) && fi.Name.Length == 11)
                {
                    Settings_fov = fi;
                    Log.Info("Found Settings_fov field as: " + Settings_fov.ToString());
                }
            }
            if (Settings_fov == null) { Log.Info("Settings_fov is null"); return false; }

            foreach(MethodInfo mi in CollisionHelper.Type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if(mi.GetParameters().Length == 1 && mi.GetParameters()[0].ParameterType == Player.Type && mi.ReturnType.IsGenericType && 
                    mi.ReturnType.GetGenericTypeDefinition() == typeof(List<>) && mi.ReturnType.GetGenericArguments().Length == 1 && mi.Name.Length == 27)
                {
                    CollisionHelper_GetBonesWorldSpace = mi;
                    WorldSpaceBone = mi.ReturnType.GetGenericArguments()[0];
                    Log.Info("Found CollisionHelper_GetBonesWorldSpace as: " + CollisionHelper_GetBonesWorldSpace.ToString());
                    Log.Info("Found class WorldSpaceBone as: " + WorldSpaceBone.ToString());
                }
            }
            if (CollisionHelper_GetBonesWorldSpace == null) { Log.Info("CollisionHelper_GetBonesWorldSpace is null"); return false; }
            if (WorldSpaceBone == null) { Log.Info("WorldSpaceBone is null"); return false; }
            CollisionHelper_Constructor = CollisionHelper.Type.GetConstructors()[0];
            Log.Info("Found CollisionHelper_Constructor as: " + CollisionHelper_Constructor.ToString());

            foreach (FieldInfo fi in WorldSpaceBone.GetFields(BindingFlags.Public | BindingFlags.Instance)) //relying on order beeing always thesame
            {
                if (WorldSpaceBone_type == null)
                {
                    WorldSpaceBone_type = fi;
                    Log.Info("Found WorldSpaceBone_type as: " + WorldSpaceBone_type.ToString());
                }
                else if (WorldSpaceBone_head == null)
                {
                    WorldSpaceBone_head = fi;
                    Log.Info("Found WorldSpaceBone_head as: " + WorldSpaceBone_head.ToString());
                }
                else if (WorldSpaceBone_tail == null)
                {
                    WorldSpaceBone_tail = fi;
                    Log.Info("Found WorldSpaceBone_tail as: " + WorldSpaceBone_tail.ToString());
                }
                else if (WorldSpaceBone_radius == null)
                {
                    WorldSpaceBone_radius = fi;
                    Log.Info("Found WorldSpaceBone_radius as: " + WorldSpaceBone_radius.ToString());
                }
                else if (WorldSpaceBone_ID == null)
                {
                    WorldSpaceBone_ID = fi;
                    Log.Info("Found WorldSpaceBone_ID as: " + WorldSpaceBone_ID.ToString());
                }
                else if (WorldSpaceBone_name == null)
                {
                    WorldSpaceBone_name = fi;
                    Log.Info("Found WorldSpaceBone_name as: " + WorldSpaceBone_name.ToString());
                }
                else
                {
                    Log.Info("WorldSpaceBone has changed !!!!!!!!!!!");
                    return false;
                }
            }
            if (WorldSpaceBone_type == null) { Log.Info("WorldSpaceBone_type is null"); return false; }
            if (WorldSpaceBone_head == null) { Log.Info("WorldSpaceBone_head is null"); return false; }
            if (WorldSpaceBone_tail == null) { Log.Info("WorldSpaceBone_tail is null"); return false; }
            if (WorldSpaceBone_radius == null) { Log.Info("WorldSpaceBone_radius is null"); return false; }
            if (WorldSpaceBone_ID == null) { Log.Info("WorldSpaceBone_ID is null"); return false; }
            if (WorldSpaceBone_name == null) { Log.Info("WorldSpaceBone_name is null"); return false; }


            foreach (MethodInfo mi in Helper1.Type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (mi.ReturnType.IsEnum && mi.GetParameters().Length == 2 && mi.GetParameters()[0].ParameterType == PlayerBase
                    && mi.GetParameters()[1].ParameterType == WeaponType && mi.Name.Length == 27)
                {
                    Helper1_GetEquippedScope = mi;
                    ScopeType = mi.ReturnType;
                    Log.Info("Found Helper1_GetEquippedScope as: " + Helper1_GetEquippedScope.ToString());
                    Log.Info("Found enum ScopeType as: " + ScopeType.ToString());
                }
                if (mi.GetCustomAttribute<DllImportAttribute>() != null && mi.GetCustomAttribute<DllImportAttribute>().EntryPoint == "GetProcAddress")
                {
                    Helper1_GetProcAddress = mi;
                    Log.Info("Found Helper1_GetProcAddress as: " + Helper1_GetProcAddress.ToString());
                }
            }
            if (Helper1_GetEquippedScope == null) { Log.Info("Helper1_GetEquippedScope is null"); return false; }
            if (ScopeType == null) { Log.Info("ScopeType is null"); return false; }
            if (Helper1_GetProcAddress == null) { Log.Info("Helper1_GetProcAddress is null"); return false; }

            foreach (MethodInfo mi in Helper.Type.GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                if(mi.ReturnType == typeof(double) && mi.Name.Length == 15 && mi.GetParameters().Length == 1 &&
                    mi.GetParameters()[0].ParameterType == PlayerBase.BaseType)
                {
                    Helper_CurrentBloom = mi;
                    Log.Info("Found Helper_CurrentBloom as: " + Helper_CurrentBloom.ToString());
                }
            }
            if(Helper_CurrentBloom == null) { Log.Info("Helper_CurrentBloom is null"); return false; }

            foreach(MethodInfo mi in Bones.Type.BaseType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if(mi.Name.Length == 23 && mi.GetParameters().Length == 1 && mi.GetParameters()[0].ParameterType == typeof(string))
                {
                    Bones_Base_GetBoneByName = mi;
                    Bone = mi.ReturnType;
                    Log.Info("Found Bones_Base_GetBoneByName as: " + Bones_Base_GetBoneByName.ToString());
                    Log.Info("Found class Bone as: " + Bone.ToString());
                }
            }
            if (Bones_Base_GetBoneByName == null) { Log.Info("Bones_Base_GetBoneByName is null"); return false; }
            if (Bone == null) { Log.Info("Bone is null"); return false; }

            foreach (FieldInfo fi in Bones.Type.BaseType.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if(fi.FieldType.IsGenericType && fi.FieldType.GetGenericTypeDefinition() == typeof(List<>) && 
                    fi.FieldType.GetGenericArguments().Length == 1 && fi.FieldType.GetGenericArguments()[0] == Bone)
                {
                    Bones_Base_BoneList = fi;
                    Log.Info("Found Bones_Base_BoneList as: " + Bones_Base_BoneList.ToString());
                }
            }
            if (Bones_Base_BoneList == null) { Log.Info("Bones_Base_BoneList is null"); return false; }

            foreach(FieldInfo fi in Bones.Type.BaseType.BaseType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if(fi.Name.Length == 11 && fi.FieldType == Vec3)
                {
                    if(Bones_Base_Base_BBMin == null)
                    {
                        Bones_Base_Base_BBMin = fi;
                        Log.Info("Found Bones_Base_Base_BBMin as: " + Bones_Base_Base_BBMin.ToString());
                    }
                    else if(Bones_Base_Base_BBMax == null)
                    {
                        Bones_Base_Base_BBMax = fi;
                        Log.Info("Found Bones_Base_Base_BBMax as: " + Bones_Base_Base_BBMax.ToString());
                    }
                    else
                    {
                        Log.Info("Unexpected third vec3 in Bones_Base_Base");
                        return false;
                    }
                }
            }
            if (Bones_Base_Base_BBMin == null) { Log.Info("Bones_Base_Base_BBMin is null"); return false; }
            if (Bones_Base_Base_BBMax == null) { Log.Info("Bones_Base_Base_BBMax is null"); return false; }

            foreach (FieldInfo fi in Bone.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if(fi.FieldType == typeof(bool) && fi.Name.Length == 11 && Bone_IsHead == null) //first one
                {
                    Bone_IsHead = fi;
                    Log.Info("Found Bone_IsHead as: " + Bone_IsHead.ToString());
                }
                if (fi.FieldType == Vec3)
                {
                    if(Bone_Head == null)
                    {
                        Bone_Head = fi;
                        Log.Info("Found Bone_Head as: " + Bone_Head.ToString());
                    }
                    else if (Bone_Tail == null)
                    {
                        Bone_Tail = fi;
                        Log.Info("Found Bone_Tail as: " + Bone_Tail.ToString());
                    }
                }
                if(fi.FieldType == typeof(string))
                {
                    Bone_Name = fi;
                    Log.Info("Found Bone_Name as: " + Bone_Name.ToString());
                }
                if(fi.FieldType == typeof(float))
                {
                    Bone_Radius = fi;
                    Log.Info("Found Bone_Radius as: " + Bone_Radius.ToString());
                }
            }
            if (Bone_IsHead == null) { Log.Info("Bone_IsHead is null"); return false; }
            if (Bone_Head == null) { Log.Info("Bone_Head is null"); return false; }
            if (Bone_Tail == null) { Log.Info("Bone_Tail is null"); return false; }
            if (Bone_Name == null) { Log.Info("Bone_Name is null"); return false; }
            if (Bone_Radius == null) { Log.Info("Bone_Radius is null"); return false; }

            foreach(MethodInfo mi in CustomWatch.Type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if(mi.ReturnType == typeof(double) && mi.GetParameters().Length == 0 && mi.Name.Length == 15 &&
                    mi.GetMethodBody().GetILAsByteArray().Length > 20)
                {
                    CustomWatch_get_Progress = mi;
                    Log.Info("Found CustomWatch_get_Progress as: " + CustomWatch_get_Progress.ToString());
                }
            }
            if (CustomWatch_get_Progress == null) { Log.Info("CustomWatch_get_Progress is null"); return false; }

            return true;
        }

        private static bool IsNumeric(Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsStruct(Type t)
        {
            return t.IsValueType && !t.IsEnum && !IsNumeric(t);
        } 

        public static void PrintMethodSig(MethodInfo mi)
        {
            Log.Info(string.Format("{0}: l: {1}, s: {2}, b: {3}", mi.Name, mi.GetMethodBody().LocalVariables.Count, 
                mi.GetMethodBody().MaxStackSize, mi.GetMethodBody().GetILAsByteArray().Length));
        }

        public static string GenerateSig(string className)
        {
            Assembly aassembly = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "sectorsedge");
            Type t = aassembly.GetType(className, true);
            if(t != null) 
                return ClassSignature.GenerateSignature(t).ToString();
            return "Type not found!";
        }

    }
}
