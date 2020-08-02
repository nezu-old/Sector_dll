using Sector_dll.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

		public enum EspTarget : int
        {
			All,
			Enemy,
			Team
        }

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 0)]
		public struct Settings
		{
			public uint menu_color;
			public int aimbot_mode;
			public int aimbot_key;
			
			public EspModes esp_mode;
			public EspTarget esp_team;
			public int esp_box;
			public int esp_skeleton;
			public int esp_snaplines;
			public int esp_name;
			public int esp_health_num;
			public int esp_health_bar;
			public int esp_weapon;
			public int esp_oov_arrow;
			public int esp_flags;
			
			public int no_recoil;

			public float debug1;
			public float debug2;
			public int debug3;
			public int debug4;
			public int debug5;
			public int debug6;
		};

		public static Settings settings = new Settings()
		{
			menu_color = Color.red,
		};

	}
}
