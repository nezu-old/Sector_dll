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
			OnKey
        }

		public static string AimbotModeStrings(AimbotMode espMode)
		{
			switch (espMode)
			{
				case AimbotMode.Off: return "OFF";
				case AimbotMode.Always: return "Always";
				case AimbotMode.OnKey: return "On Key";
				default: return "Error";
			}
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 0)]
		public struct Settings
		{
			public EspModes esp_mode;
			public EspTarget esp_target;
			public bool esp_box;
			public bool esp_skeleton;
			public bool esp_snaplines;
			public bool esp_name;
			public bool esp_health_num;
			public bool esp_health_bar;
			public bool esp_weapon;
			public bool esp_oov_arrow;
			public bool esp_flags;

			public bool esp_grenade;
			public bool esp_grenade_launcher;
			public bool esp_scanner;
			public bool esp_c4;
			public bool esp_disruptor;

			public AimbotMode aimbot_mode;
			public Keys aimbot_key;

			public int no_recoil;

			public int menuX;
			public int menuY;

			public float debug1;
			public float debug2;
			public int debug3;
			public int debug4;
			public int debug5;
			public int debug6;
		};

		public static Settings settings = new Settings()
		{
		};

	}
}
