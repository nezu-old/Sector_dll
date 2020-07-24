#pragma once
#include "imgui/imgui.h"

namespace Menu {

	extern bool open;

	struct Settings {
		unsigned int menu_color;
		
		int esp;
		int esp_health_bar;
		int esp_name;
		int esp_enemy_only;

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

