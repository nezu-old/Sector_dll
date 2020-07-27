#pragma once
#include "imgui/imgui.h"

namespace Menu {

	extern unsigned char open;

	struct Settings {
		unsigned int menu_color;
		
		int esp_mode;
		int esp_team;
		int esp_box;
		int esp_skeleton;
		int esp_name;
		int esp_health_num;
		int esp_health_bar;
		int esp_weapon;
		int esp_oov_arrow;
		int esp_flags;

		int no_recoil;

		float debug1;
		float debug2;
		int debug3;
		int debug4;
		int debug5;
		int debug6;
	};
	
	void __stdcall DrawMenu(Settings * settings);

	void UpdateColors(ImColor color);
	
};

