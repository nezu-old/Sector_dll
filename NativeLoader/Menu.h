#pragma once
#include "imgui/imgui.h"

namespace Menu {

	extern bool open;

	struct Settings {
		unsigned int menu_color;
		
		bool esp;
		bool esp_health_bar;
		bool esp_name;
		bool esp_enemy_only;

		bool no_recoil;
	};
	
	void DrawMenu(Settings * settings);

	void UpdateColors(ImColor color);
	
};

