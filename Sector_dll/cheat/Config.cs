using Sector_dll.sdk;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sector_dll.cheat
{
	class Config
	{

		public enum EspModes : int
		{
			Off,
			Always,
			OnDeath
		};

		public static string EspModesStrings(EspModes espMode)
        {
            switch (espMode)
            {
                case EspModes.Off: return "OFF";
                case EspModes.Always: return "ON";
                case EspModes.OnDeath: return "On Death";
				default: return "Error";
            }
        } 

		public enum EspTarget : int
        {
			All,
			Enemy,
			Team
        }

		public static string EspTargetStrings(EspTarget espMode)
		{
			switch (espMode)
			{
				case EspTarget.All: return "All";
				case EspTarget.Enemy: return "Enemy";
				case EspTarget.Team: return "Friendly";
				default: return "Error";
			}
		}

		public enum AimbotMode : int
        {
			Off,
			Always,
			OnShoot
        }

		public static string AimbotModeStrings(AimbotMode espMode)
		{
			switch (espMode)
			{
				case AimbotMode.Off: return "OFF";
				case AimbotMode.Always: return "Always";
				case AimbotMode.OnShoot: return "On Shoot";
				default: return "Error";
			}
		}

		public enum AimbotPenetration : int
		{
			OnlyVisible,
			IfAnnyVisible,
			Autowall
		}

		public static string AimbotPenetrationStrings(AimbotPenetration aimbotPenetration)
		{
			switch (aimbotPenetration)
			{
				case AimbotPenetration.OnlyVisible: return "Only Visible";
				case AimbotPenetration.IfAnnyVisible: return "If Any Visible";
				case AimbotPenetration.Autowall: return "Autowall";
				default: return "Error";
			}
		}

		public enum EspVisCheck : int
		{
			Off,
			IndicateVisible,
			OnlyVisible
		}

		public static string EspVisCheckStrings(EspVisCheck aimbotPenetration)
		{
			switch (aimbotPenetration)
			{
				case EspVisCheck.Off: return "OFF";
				case EspVisCheck.IndicateVisible: return "Indicate Visible";
				case EspVisCheck.OnlyVisible: return "Only Visible";
				default: return "Error";
			}
		}

		public enum EspBoxMode : int
		{
			Off,
			Standard,
			Precise
		}

		public static string EspBoxModeStrings(EspBoxMode aimbotPenetration)
		{
			switch (aimbotPenetration)
			{
				case EspBoxMode.Off: return "OFF";
				case EspBoxMode.Standard: return "Standard";
				case EspBoxMode.Precise: return "Precise";
				default: return "Error";
			}
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 0)]
		public struct Settings
		{
			public EspModes esp_mode;
			public EspTarget esp_target;
			public EspBoxMode esp_box;
			public bool esp_skeleton;
			public bool esp_snaplines;
			public bool esp_name;
			public bool esp_health_num;
			public bool esp_health_bar;
			public bool esp_weapon;
			public bool esp_oov_arrow;
			public bool esp_flags;
			public EspVisCheck esp_vis_mode;

			public bool esp_grenade;
			public bool esp_grenade_launcher;
			public bool esp_scanner;
			public bool esp_c4;
			public bool esp_disruptor;

			public AimbotMode aimbot_mode;
			public float aimbot_fov;
			public float aimbot_smooth;
			public bool aimbot_auto_switch_target;
			public bool aimbot_auto_scope;
			public bool aimbot_auto_shoot;
			public AimbotPenetration aimbot_penetration;

			public bool spread_croshair;
			public float speed_multiplyer;
			//public int no_recoil;

			public int menuX;
			public int menuY;

			public bool debug_esp;
		};

		public static Settings settings = new Settings()
		{
			esp_mode = EspModes.Always,
			esp_target = EspTarget.All,
			esp_box = EspBoxMode.Precise,
			esp_skeleton = true,
			esp_snaplines = false,
			esp_name = true,
			esp_health_num = true,
			esp_health_bar = true,
			esp_weapon = true,
			esp_oov_arrow = true,
			esp_flags = true,
			esp_vis_mode = EspVisCheck.IndicateVisible,

			esp_grenade = true,
			esp_grenade_launcher = true,
			esp_scanner = true,
			esp_c4 = true,
			esp_disruptor = true,

			aimbot_mode = AimbotMode.Always,
			aimbot_fov = 10,
			aimbot_auto_scope = false,
			aimbot_auto_shoot = true,
			aimbot_auto_switch_target = true,
			aimbot_smooth = 1,
			aimbot_penetration = AimbotPenetration.OnlyVisible,

			spread_croshair = false,
			speed_multiplyer = 1,

			menuX = 0,
			menuY = 150,
			
		};

	}
}
