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

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 0)]
		public struct Settings
		{
			public uint menu_color;

			public int esp;
			public int esp_health_bar;
			public int esp_name;
			public int esp_enemy_only;

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

			esp = 1,
			esp_health_bar = 1,
			esp_name = 1,
			esp_enemy_only = 1,

			no_recoil = 1,

			debug1 = 1.0f,
			debug2 = 1.0f,
			debug3 = 1,
			debug4 = 1,
		};

	}
}
