#include "Menu.h"

bool Menu::open = false;

void __stdcall Menu::DrawMenu(Settings* settings) {

	if (!Menu::open)
		return;

	//sizeof(*settings)
	
	ImGui::ShowDemoWindow();

	if (ImGui::Begin("Debug")) {
		ImColor col = ImColor(settings->menu_color);
		if (ImGui::ColorEdit3("GUI color", &col.Value.x, ImGuiColorEditFlags_NoInputs)) {
			UpdateColors(col);
			settings->menu_color = (ImU32)col;
		}
		ImGui::SliderFloat("debug1", &settings->debug1, 0, 5);
		ImGui::SliderFloat("debug2", &settings->debug2, 0, 5);
		ImGui::SliderInt("debug3", &settings->debug3, 0, 5);
		ImGui::SliderInt("debug4", &settings->debug4, 0, 5);
		ImGui::Checkbox("debug5", (bool*)&settings->debug5);
		ImGui::Checkbox("debug6", (bool*)&settings->debug6);
	}
	ImGui::End();

}

void Menu::UpdateColors(ImColor color) {

	ImVec4* colors = ImGui::GetStyle().Colors;

	//shades of gray
	colors[ImGuiCol_Text] =					ImVec4(1.00f, 1.00f, 1.00f, 1.00f);
	colors[ImGuiCol_TextDisabled] =			ImVec4(0.50f, 0.50f, 0.50f, 1.00f);
	colors[ImGuiCol_WindowBg] =				ImVec4(0.20f, 0.20f, 0.20f, 1.00f);
	colors[ImGuiCol_ChildBg] =				ImVec4(0.00f, 0.00f, 0.00f, 0.27f);
	colors[ImGuiCol_PopupBg] =				ImVec4(0.27f, 0.27f, 0.27f, 1.00f);
	colors[ImGuiCol_Border] =				ImVec4(0.43f, 0.43f, 0.50f, 0.50f);
	colors[ImGuiCol_BorderShadow] =			ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
	colors[ImGuiCol_FrameBg] =				ImVec4(0.00f, 0.00f, 0.00f, 0.56f);
	colors[ImGuiCol_FrameBgHovered] =		ImVec4(0.00f, 0.00f, 0.00f, 1.00f);
	colors[ImGuiCol_FrameBgActive] =		ImVec4(0.31f, 0.31f, 0.31f, 0.24f);
	colors[ImGuiCol_MenuBarBg] =			ImVec4(0.00f, 0.00f, 0.00f, 0.27f);
	colors[ImGuiCol_ScrollbarBg] =			ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
	colors[ImGuiCol_ScrollbarGrab] =		ImVec4(0.31f, 0.31f, 0.31f, 1.00f);
	colors[ImGuiCol_ScrollbarGrabActive] =	ImVec4(0.51f, 0.51f, 0.51f, 1.00f);
	colors[ImGuiCol_ScrollbarGrabHovered] = ImVec4(0.41f, 0.41f, 0.41f, 1.00f);
	colors[ImGuiCol_Header] =               ImVec4(1.00f, 1.00f, 1.00f, 0.27f);
	colors[ImGuiCol_HeaderHovered] =        ImVec4(1.00f, 1.00f, 1.00f, 1.35f);
	colors[ImGuiCol_HeaderActive] =         ImVec4(1.00f, 1.00f, 1.00f, 0.43f);
	colors[ImGuiCol_Separator] =            ImVec4(0.43f, 0.43f, 0.43f, 0.50f);
	colors[ImGuiCol_ResizeGrip] =           ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
	colors[ImGuiCol_ResizeGripHovered] =    ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
	colors[ImGuiCol_ResizeGripActive] =     ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
	colors[ImGuiCol_PlotLines] =            ImVec4(0.61f, 0.61f, 0.61f, 1.00f);
	colors[ImGuiCol_NavWindowingHighlight] =ImVec4(1.00f, 1.00f, 1.00f, 0.70f);
	colors[ImGuiCol_NavWindowingDimBg] =    ImVec4(0.80f, 0.80f, 0.80f, 0.20f);
	colors[ImGuiCol_ModalWindowDimBg] =     ImVec4(0.80f, 0.80f, 0.80f, 0.35f);
	
	//title
	float title_x = color.Value.x;
	float title_y = color.Value.y;
	float title_z = color.Value.z;
	float title_w = color.Value.w;
	colors[ImGuiCol_TitleBg] =              ImVec4(title_x, title_y, title_z, title_w * 0.84f);
	colors[ImGuiCol_TitleBgActive] =        ImVec4(title_x, title_y, title_z, title_w * 1.00f);
	colors[ImGuiCol_TitleBgCollapsed] =     ImVec4(title_x, title_y, title_z, title_w * 0.32f);
	
	//base
	ImVec4 base_color = ImVec4(color.Value.x, color.Value.y, color.Value.z, color.Value.w * 0.60f);
	colors[ImGuiCol_SliderGrab] =           base_color;
	colors[ImGuiCol_CheckMark] =            base_color;
	colors[ImGuiCol_Button] =               base_color;
	colors[ImGuiCol_Tab] =                  base_color;
	colors[ImGuiCol_DragDropTarget] =       base_color;

	//ative
	ImVec4 active_color = ImVec4(color.Value.x, color.Value.y, color.Value.z, color.Value.w * 0.76f);
	colors[ImGuiCol_ButtonActive] =         active_color;
	colors[ImGuiCol_SeparatorActive] =      active_color;
	colors[ImGuiCol_TabActive] =            active_color;
	colors[ImGuiCol_SliderGrabActive] =     active_color;

	//hovered
	ImVec4 hovered_color = base_color;
	colors[ImGuiCol_ButtonHovered] =		hovered_color;
	colors[ImGuiCol_SeparatorHovered] =		hovered_color;
	colors[ImGuiCol_TabHovered] =			hovered_color;

	//other
	colors[ImGuiCol_PlotLinesHovered] =     ImVec4(1.00f, 0.43f, 0.35f, 1.00f);
	colors[ImGuiCol_PlotHistogram] =        ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
	colors[ImGuiCol_PlotHistogramHovered] = ImVec4(1.00f, 0.60f, 0.00f, 1.00f);
	colors[ImGuiCol_TextSelectedBg] =       ImVec4(0.26f, 0.45f, 0.98f, 0.35f);
	colors[ImGuiCol_NavHighlight] =         ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
	colors[ImGuiCol_TabUnfocused] =         ImVec4(0.07f, 0.10f, 0.15f, 0.97f);
	colors[ImGuiCol_TabUnfocusedActive] =   ImVec4(0.14f, 0.26f, 0.42f, 1.00f);
}