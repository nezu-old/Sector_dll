using Mono.Cecil;
using Mono.Cecil.Cil;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.IO;
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
            publicMethods = 118,
            staticMethods = 0,

            publicFields = 119,
            privateFields = 3,
            staticFields = 0,
            readonlyFields = 5,

            boolFields = 27,
            byteFields = 3,
            shortFields = 0,
            intFields = 19,
            longFields = 0,
            floatFields = 1,
            doubleFields = 8,
            enumFields = 5,
            stringFields = 3,
            ArrayFields = 1,
            OtherFields = 55
        });

        public static ResolvedType GClass49 = new ResolvedType("GClass49", new ClassSignature() 
        {
            nameLength = 31,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 3,

            privateMethods = 51,
            publicMethods = 346,
            staticMethods = 0,

            publicFields = 145,
            privateFields = 43,
            staticFields = 5,
            readonlyFields = 6,

            boolFields = 38,
            byteFields = 1,
            shortFields = 0,
            intFields = 15,
            longFields = 1,
            floatFields = 0,
            doubleFields = 21,
            enumFields = 8,
            stringFields = 0,
            ArrayFields = 10,
            OtherFields = 125
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

        public static ResolvedType Map = new ResolvedType("Map", new ClassSignature()
        {
            nameLength = 27,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 0,

            privateMethods = 2,
            publicMethods = 59,
            staticMethods = 3,

            publicFields = 35,
            privateFields = 0,
            staticFields = 0,
            readonlyFields = 0,

            boolFields = 3,
            byteFields = 0,
            shortFields = 0,
            intFields = 7,
            longFields = 0,
            floatFields = 3,
            doubleFields = 1,
            enumFields = 4,
            stringFields = 2,
            ArrayFields = 13,
            OtherFields = 8
        });

        public static ResolvedType Helper1 = new ResolvedType("Helper1", new ClassSignature() 
        {
            nameLength = 27,

            publicClass = true,
            abstractClass = true,
            nestedTypes = 0,

            privateMethods = 0,
            publicMethods = 53,
            staticMethods = 49,

            publicFields = 104,
            privateFields = 0,
            staticFields = 104,
            readonlyFields = 0,

            boolFields = 24,
            byteFields = 0,
            shortFields = 0,
            intFields = 51,
            longFields = 0,
            floatFields = 0,
            doubleFields = 4,
            enumFields = 3,
            stringFields = 1,
            ArrayFields = 0,
            OtherFields = 21
        });

        public static ResolvedType Helper = new ResolvedType("Helper", new ClassSignature()
        {
            nameLength = 27,

            publicClass = true,
            abstractClass = true,
            nestedTypes = 8,

            privateMethods = 8,
            publicMethods = 45,
            staticMethods = 49,

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

            privateMethods = 2,
            publicMethods = 28,
            staticMethods = 1,

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

        public static ResolvedType CollisionEntity = new ResolvedType("CollisionEntity", new ClassSignature()
        {
            nameLength = 39,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 0,

            privateMethods = 0,
            publicMethods = 21,
            staticMethods = 2,

            publicFields = 31,
            privateFields = 5,
            staticFields = 1,
            readonlyFields = 0,

            boolFields = 12,
            byteFields = 2,
            shortFields = 2,
            intFields = 1,
            longFields = 0,
            floatFields = 1,
            doubleFields = 3,
            enumFields = 2,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 13
        });

        public static ResolvedType Scanner = new ResolvedType("Scanner", new ClassSignature()
        {
            nameLength = 39,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 0,

            privateMethods = 0,
            publicMethods = 20,
            staticMethods = 1,

            publicFields = 35,
            privateFields = 0,
            staticFields = 1,
            readonlyFields = 0,

            boolFields = 11,
            byteFields = 2,
            shortFields = 1,
            intFields = 0,
            longFields = 0,
            floatFields = 1,
            doubleFields = 3,
            enumFields = 2,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 15
        });

        public static ResolvedType C4 = new ResolvedType("C4", new ClassSignature()
        {
            nameLength = 43,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 0,

            privateMethods = 0,
            publicMethods = 19,
            staticMethods = 0,

            publicFields = 39,
            privateFields = 0,
            staticFields = 0,
            readonlyFields = 0,

            boolFields = 15,
            byteFields = 2,
            shortFields = 1,
            intFields = 0,
            longFields = 0,
            floatFields = 1,
            doubleFields = 3,
            enumFields = 3,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 14
        });

        public static ResolvedType Grenade = new ResolvedType("Grenade", new ClassSignature()
        {
            nameLength = 43,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 0,

            privateMethods = 0,
            publicMethods = 19,
            staticMethods = 0,

            publicFields = 34,
            privateFields = 0,
            staticFields = 1,
            readonlyFields = 0,

            boolFields = 11,
            byteFields = 2,
            shortFields = 1,
            intFields = 0,
            longFields = 0,
            floatFields = 1,
            doubleFields = 3,
            enumFields = 2,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 14
        });

        public static ResolvedType GLauncher = new ResolvedType("GLauncher", new ClassSignature()
        {
            nameLength = 39,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 0,

            privateMethods = 0,
            publicMethods = 19,
            staticMethods = 0,

            publicFields = 31,
            privateFields = 0,
            staticFields = 0,
            readonlyFields = 0,

            boolFields = 11,
            byteFields = 2,
            shortFields = 1,
            intFields = 0,
            longFields = 0,
            floatFields = 1,
            doubleFields = 3,
            enumFields = 2,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 11
        });

        public static ResolvedType Projectile = new ResolvedType("Projectile", new ClassSignature()
        {
            nameLength = 39,

            publicClass = true,
            abstractClass = false,
            nestedTypes = 0,

            privateMethods = 0,
            publicMethods = 21,
            staticMethods = 2,

            publicFields = 31,
            privateFields = 5,
            staticFields = 1,
            readonlyFields = 0,

            boolFields = 12,
            byteFields = 2,
            shortFields = 2,
            intFields = 1,
            longFields = 0,
            floatFields = 1,
            doubleFields = 3,
            enumFields = 2,
            stringFields = 0,
            ArrayFields = 0,
            OtherFields = 13
        });

        //public static ResolvedType XXX = new ResolvedType("XXX", new ClassSignature());
        //public static ResolvedType XXX = new ResolvedType("XXX", new ClassSignature());

        public static ResolvedType[] ResolvedTypes = new ResolvedType[]
        {
            Player,
            GClass49,
            Vec4,
            RequestHelper,
            Map,
            Helper1,
            Helper,
            Bones,
            CustomWatch,
            CollisionEntity,
            Scanner,
            C4,
            Grenade,
            GLauncher,
            Projectile
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

        public static MethodInfo RequestHelper_POST;

        public static MethodInfo RequestHelper_GET;

        public static Type Drawing;

        public static Type Color;

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

        public static Type MapBase;

        public static Type PlayerLoadout;

        public static Type WeaponType;

        public static MethodInfo Helper1_GetEquippedScope;

        public static MethodInfo Helper1_GetProcAddress;

        public static Type ScopeType;

        public static List<MethodInfo> RegQueryValueEx = new List<MethodInfo>();

        public static MethodInfo DiscordCreate;

        //public static MethodInfo Helper_CurrentBloom;

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

        public static MethodInfo GameManager_GetPlayerByID;

        public static FieldInfo GameManager_CollisionEntityList;

        public static MethodInfo CustomWatch_get_Progress;

        public static FieldInfo CollisionEntity_Position;

        public static FieldInfo CollisionEntity_OwnerID;

        public static FieldInfo CollisionEntity_type;

        public static FieldInfo CollisionEntity_Matrix;

        public static MethodInfo CollisionEntity_Get_Lifetime;

        public static FieldInfo CollisionEntity_BounceWatch;

        public static FieldInfo CollisionEntity_Health;

        public static bool FindSignatures(Assembly assembly)
        {
            //Log.Info("Waiting for debugger to attach");
            //while (!Debugger.IsAttached)
            //{
            //    Thread.Sleep(100);
            //}
            //Log.Info("Debugger attached");
            //Debugger.Break();
            RegQueryValueEx.Clear();
            foreach (Type t in assembly.GetTypes())
            {
                ClassSignature sig = ClassSignature.GenerateSignature(t);
                foreach (ResolvedType rt in ResolvedTypes)
                    rt.Update(sig, t);
                foreach(ConstructorInfo ci in t.GetConstructors(BindingFlags.Instance | BindingFlags.Public))
                {
                    ParameterInfo[] pi = ci.GetParameters();
                    if (pi.Length == 4 && pi[0].ParameterType == typeof(byte) && pi[1].ParameterType == typeof(byte)
                            && pi[2].ParameterType == typeof(byte) && pi[3].ParameterType == typeof(byte) &&
                            pi[3].HasDefaultValue)
                    {
                        Color = t;
                        Log.Info("Found struct Color as: " + Color.ToString());
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
            if(Color == null) { Log.Info("Color is null"); return false; }

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
                    Log.Info("Found class GameManager as: " + GameManager.ToString());
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
                    mi.GetParameters()[0].ParameterType == Player.Type && mi.GetMethodBody().GetILAsByteArray().Length > 200)
                {
                    //Log.Danger(mi.GetMethodBody().MaxStackSize + " " + mi.GetMethodBody().LocalVariables.Count + " " + mi.GetMethodBody().GetILAsByteArray().Length);
                    GameManager_SetupBones = mi;
                    Log.Info("Found GameManager_SetupBones as: " + GameManager_SetupBones.ToString());
                }
            }
            if (GameManager_SetupBones == null) { Log.Info("GameManager_SetupBones is null"); return false; }

            foreach(MethodInfo mi in GameManager.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if(mi.ReturnType == PlayerBase && mi.Name.Length == 15 && mi.GetParameters().Length == 1 
                    && mi.GetParameters()[0].ParameterType == typeof(byte))
                {
                    GameManager_GetPlayerByID = mi;
                    Log.Info("Found GameManager_GetPlayerByID as: " + GameManager_GetPlayerByID.ToString());
                }
            }
            if (GameManager_GetPlayerByID == null) { Log.Info("GameManager_GetPlayerByID is null"); return false; }

            foreach (FieldInfo fi in GameManager.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if(fi.Name.Length == 11 && fi.FieldType.IsGenericType && fi.FieldType.GetGenericTypeDefinition() == typeof(List<>)
                    && fi.FieldType.GetGenericArguments().Length == 1 && fi.FieldType.GetGenericArguments()[0] == CollisionEntity.Type)
                {
                    GameManager_CollisionEntityList = fi;
                    Log.Info("Found GameManager_CollisionEntityList as: " + GameManager_CollisionEntityList.ToString());
                }
            }
            if (GameManager_CollisionEntityList == null) { Log.Info("GameManager_CollisionEntityList is null"); return false; }

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

            foreach (FieldInfo fi in PlayerBase.BaseType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.Name.Length == 15 && fi.FieldType.Name.Length == 39 && IsStruct(fi.FieldType))
                {
                    CharacterTexture = fi.FieldType;
                    PlayerBase_Base_CharacterTexture = fi;
                    Log.Info("Found struct CharacterTexture as: " + CharacterTexture.ToString());
                    Log.Info("Found PlayerBase_Base_CharacterTexture as: " + PlayerBase_Base_CharacterTexture.ToString());
                }
            }
            if (CharacterTexture == null) { Log.Info("CharacterTexture is null"); return false; }
            if (PlayerBase_Base_CharacterTexture == null) { Log.Info("PlayerBase_Base_CharacterTexture is null"); return false; }

            foreach (FieldInfo fi in PlayerBase.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (fi.Name.Length == 11 && fi.FieldType.Name.Length == 39 && fi.FieldType.IsEnum)
                {
                    TeamType = fi.FieldType;
                    Log.Info("Found enum TeamType as: " + TeamType.ToString());
                }
            }

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
                    && mi.GetMethodBody().LocalVariables.Count == 1 && mi.GetMethodBody().MaxStackSize == 12
                    && mi.GetMethodBody().GetILAsByteArray().Length > 100
                    )
                {
                    //Log.Danger(mi.GetMethodBody().MaxStackSize + " " + mi.GetMethodBody().LocalVariables.Count + " " + mi.GetMethodBody().GetILAsByteArray().Length);
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

            foreach(FieldInfo fi in PlayerBase.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (fi.FieldType == typeof(double) && fi.Name.Length == 15)
                {
                    if(PlayerBase_Base_Yaw == null)
                    {
                        PlayerBase_Base_Yaw = fi;
                        Log.Info("Found PlayerBase_Base_Yaw as: " + PlayerBase_Base_Yaw.ToString());
                    }
                    else if(PlayerBase_Base_Pitch == null)
                    {
                        PlayerBase_Base_Pitch = fi;
                        Log.Info("Found PlayerBase_Base_Pitch as: " + PlayerBase_Base_Pitch.ToString());
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

            bool second = true;
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

            foreach (MethodInfo mi in RequestHelper.Type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (mi.GetParameters().Length == 2)
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
                    && fi.FieldType.GetGenericArguments()[0] == WeaponType )//&& fi.FieldType.GetGenericArguments()[1] == Vec2)
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
            //if (GClass49_Base_ScopeSizes1 == null) { Log.Info("GClass49_Base_ScopeSizes1 is null"); return false; }
            //if (GClass49_Base_ScopeSizes2 == null) { Log.Info("GClass49_Base_ScopeSizes2 is null"); return false; }

            foreach (MethodInfo mi in GClass49.Type.BaseType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (mi.IsPublic && mi.ReturnType == Color && mi.GetParameters().Length == 1
                    && mi.GetParameters()[0].ParameterType == PlayerBase.BaseType)
                {
                    GClass49_Base_GetPlayerColor = mi;
                    Log.Info("Found GClass49_Base_GetPlayerColor as: " + GClass49_Base_GetPlayerColor.ToString());
                }
                if (mi.ReturnType == typeof(bool) && mi.GetParameters().Length == 0 && mi.Name.Length == 15 &&
                    mi.GetMethodBody().GetILAsByteArray().Length > 50)
                {
                    //Log.Danger(mi.GetMethodBody().MaxStackSize + " " + mi.GetMethodBody().LocalVariables.Count + " " + mi.GetMethodBody().GetILAsByteArray().Length);
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
                    if (GClass49_Base_Base_ScreenHeight == null)
                    {
                        GClass49_Base_Base_ScreenHeight = fi;
                        Log.Info("Found GClass49_Base_Base_ScreenHeight field as: " + GClass49_Base_Base_ScreenHeight.ToString());
                    }
                    else if(GClass49_Base_Base_ScreenWidth == null)
                    {
                        GClass49_Base_Base_ScreenWidth = fi;
                        Log.Info("Found GClass49_Base_Base_ScreenWidth field as: " + GClass49_Base_Base_ScreenWidth.ToString());
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

            //foreach (MethodInfo mi in Helper.Type.GetMethods(BindingFlags.Static | BindingFlags.Public))
            //{
            //    if(mi.ReturnType == typeof(double) && mi.Name.Length == 15 && mi.GetParameters().Length == 1 &&
            //        mi.GetParameters()[0].ParameterType == PlayerBase.BaseType)
            //    {
            //        Helper_CurrentBloom = mi;
            //        Log.Info("Found Helper_CurrentBloom as: " + Helper_CurrentBloom.ToString());
            //    }
            //}
            //if(Helper_CurrentBloom == null) { Log.Info("Helper_CurrentBloom is null"); return false; }

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

            foreach(MethodInfo mi in CollisionEntity.Type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if(mi.ReturnType == typeof(int) && mi.GetParameters().Length == 0 && mi.Name.Length == 15)
                {
                    CollisionEntity_Get_Lifetime = mi;
                    Log.Info("Found CollisionEntity_Get_Lifetime as: " + CollisionEntity_Get_Lifetime.ToString());
                }
            }
            if (CollisionEntity_Get_Lifetime == null) { Log.Info("CollisionEntity_Get_Lifetime is null"); return false; }

            foreach(FieldInfo fi in CollisionEntity.Type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.FieldType == CustomWatch.Type && fi.Name.Length == 23)
                {
                    CollisionEntity_BounceWatch = fi;
                    Log.Info("Found CollisionEntity_BounceWatch as: " + CollisionEntity_BounceWatch.ToString());
                }
                if (fi.FieldType == WeaponType)
                {
                    CollisionEntity_type = fi;
                    Log.Info("Found CollisionEntity_type as: " + CollisionEntity_type.ToString());
                }
                if (fi.FieldType == Matrix4)
                {
                    CollisionEntity_Matrix = fi;
                    Log.Info("Found CollisionEntity_Matrix as: " + CollisionEntity_Matrix.ToString());
                }
            }
            if (CollisionEntity_BounceWatch == null) { Log.Info("CollisionEntity_BounceWatch is null"); return false; }
            if (CollisionEntity_type == null) { Log.Info("CollisionEntity_type is null"); return false; }
            if (CollisionEntity_Matrix == null) { Log.Info("CollisionEntity_Matrix is null"); return false; }

            ModuleDefinition moduleDefinition = AssemblyDefinition.ReadAssembly(assembly.Location).MainModule;
            if (moduleDefinition == null) { Log.Info("moduleDefinition is null"); return false; }

            TypeDefinition CollisionEntityType = moduleDefinition.GetType(CollisionEntity.Type.Name);
            if (CollisionEntityType == null) { Log.Info("CollisionEntityType is null"); return false; }

            MethodDefinition CollisionEntity_CloneConst = CollisionEntityType.Methods.Where(x => x.Name == ".ctor" && x.Parameters.Count == 1).First();
            if (CollisionEntity_CloneConst == null) { Log.Info("CollisionEntity_CloneConst is null"); return false; }

            OpCode LastopCode = OpCodes.Nop;
            int fieldNum = 0;
            bool healthNext = false;
            foreach (var il in CollisionEntity_CloneConst.Body.Instructions)
            {
                if (il.OpCode == OpCodes.Ldc_R8)
                {
                    double val = (double)il.Operand;
                    if (val == 100.0)
                        healthNext = true;
                }
                if (healthNext && il.OpCode == OpCodes.Stfld)
                {
                    healthNext = false;
                    FieldDefinition field = il.Operand as FieldDefinition;
                    CollisionEntity_Health = CollisionEntity.Type.GetField(field.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (CollisionEntity_Health != null)
                        Log.Info("Found CollisionEntity_Health as: " + CollisionEntity_Health.ToString());
                }
                else if (il.OpCode == OpCodes.Stfld && LastopCode == OpCodes.Ldfld)
                {
                    FieldDefinition field = il.Operand as FieldDefinition;
                    if (fieldNum == 2) //OwnerID
                    {
                        CollisionEntity_OwnerID = CollisionEntity.Type.GetField(field.Name, BindingFlags.Public | BindingFlags.Instance);
                        if (CollisionEntity_OwnerID != null)
                            Log.Info("Found CollisionEntity_OwnerID as: " + CollisionEntity_OwnerID.ToString());
                    }
                    if (fieldNum == 4) //position
                    {
                        CollisionEntity_Position = CollisionEntity.Type.GetField(field.Name, BindingFlags.Public | BindingFlags.Instance);
                        if(CollisionEntity_Position != null)
                            Log.Info("Found CollisionEntity_Position as: " + CollisionEntity_Position.ToString());
                    }
                    fieldNum++;
                }
                LastopCode = il.OpCode;
            }
            if (CollisionEntity_OwnerID == null) { Log.Info("CollisionEntity_OwnerID is null"); return false; }
            if (CollisionEntity_Position == null) { Log.Info("CollisionEntity_Position is null"); return false; }
            if (CollisionEntity_Health == null) { Log.Info("CollisionEntity_Health is null"); return false; }

            //Log.Info(CollisionEntity_CloneConst);

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
