#include "Menu.h"
#define IMGUI_DEFINE_MATH_OPERATORS
#include "imgui/imgui_internal.h"
#include <unordered_map>
#define GLEW_STATIC
#include "GL\glew.h"
#include "hooks.h"

#define DEB settings->debug1, settings->debug2

unsigned char Menu::open = 1;

unsigned char reloadImg[1024] = {
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFE, 0xFE, 0xFE, 0xF5,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0x4B, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0xFE, 0xFE, 0xFE, 0xFB, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xA6,
	0xFE, 0xFE, 0xFE, 0xDF, 0xFF, 0xFF, 0xFF, 0xBF, 0xFF, 0xFF, 0xFF, 0xAF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x5F,
	0xFF, 0xFF, 0xFF, 0x4F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFE, 0xFE, 0xFE, 0xEA, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0xFF, 0xFF, 0xFF, 0x65, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xAC, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0x3B, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0xFF, 0xFF, 0xFF, 0x02, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0xFE, 0xFE, 0xFE, 0xF8, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00
};


struct animatedbtn_t {
	float w = 0;
	float speed = 0;
	unsigned char mode = 100;
};

std::unordered_map<ImGuiID, animatedbtn_t> animated_buttons;

bool animatedMenuButton(const char* label, const ImVec2 size, bool selected = false, bool vertical = false) {
	ImGuiID id = ImGui::GetID((std::string(label) + "_back").c_str());
	ImGuiStyle style = ImGui::GetStyle();
	animatedbtn_t* b = &animated_buttons[id];
	const ImVec2 label_size = ImGui::CalcTextSize(label, NULL, true);
	const auto pos = ImGui::GetCurrentWindow()->DC.CursorPos;
	const ImRect rect(pos, pos + size);
	ImGui::ItemSize(size, style.FramePadding.y);
	if (!ImGui::ItemAdd(rect, id)) return false;
	bool hovered;
	bool pressed = ImGui::ButtonBehavior(rect, id, &hovered, 0);
	if (pressed) ImGui::MarkItemEdited(id);
	float delta = ImGui::GetIO().DeltaTime;
	float sizevar = vertical ? size.y : size.x;
	/* modes:
	0 - extending
	1 - bounceing
	2 - extended
	100 - hideing/hidden
	*/
	if (hovered || selected) {
		if (b->mode == 100) { //if was hidden/hiding
			b->mode = 0;
			b->speed = 15;
		}
		if (b->mode != 2) { //if not fully extended
			b->speed += delta * 50; //apply acceleration
			b->w += b->speed * delta * 100; //move it by delta * speed
		}
		if (b->w > sizevar) { //we reached the end
			b->w = sizevar;
			b->speed *= b->mode == 0 ? -.25f : 0; //if first hit reverse direction by applying negative speed
			b->mode++; //advance mode
		}
	} else {
		if (b->mode != 100) b->speed = 0; //cancel speed if anny
		b->mode = 100;//set mode to hideing
		b->speed -= delta * 50; //apply acceleration
		b->w += b->speed * delta * 100; //move it by delta * speed
		if (b->w < 0) { //stop it once fully hidden
			b->speed = 0;
			b->w = 0;
		}
	}
	const ImRect rect2(pos, pos + (vertical ? ImVec2(size.x, b->w) : ImVec2(b->w, size.y)));
	ImGui::RenderNavHighlight(rect, id);
	ImGui::RenderFrame(rect2.Min, rect2.Max, ImGui::GetColorU32(selected ? ImGuiCol_ButtonActive : ImGuiCol_Button), false, style.FrameRounding);
	ImGui::RenderTextClipped(rect.Min + style.FramePadding, rect.Max - style.FramePadding, label, NULL, &label_size, style.ButtonTextAlign, &rect);

	//printf("%s: %d %.2f %.2f\n", label, b->mode, b->w, b->speed);

	return pressed;
}

bool childWithTitle(const char* title, ImVec2 size = ImVec2(0, 0)) {
	if (ImGui::BeginChild((std::string("wid_") + title).c_str(), size, true, ImGuiWindowFlags_MenuBar)) {
		if (ImGui::BeginMenuBar()) {
			ImGuiStyle style = ImGui::GetStyle();
			ImGui::SetCursorPosX((ImGui::GetContentRegionAvailWidth() + style.WindowPadding.x * 2 - ImGui::CalcTextSize(title, 0, true).x) / 2);
			ImGui::Text(title);
		}
		ImGui::EndMenuBar();
		return true;
	}
	return false;
}


const char* const KeyNames[] = {
	"Unknown",
	"Mouse 1",
	"Mouse 2",
	"VK_CANCEL",
	"Mouse 3",
	"Mouse 4",
	"Mouse 5",
	"Unknown",
	"Backspace",
	"Tab",
	"Unknown",
	"Unknown",
	"Clear",
	"Enter",
	"Unknown",
	"Unknown",
	"Shift",
	"Control",
	"Menu",
	"Pause",
	"Caps Lock",
	"Kana",
	"Unknown",
	"Junja",
	"Final",
	"Kanji",
	"Unknown",
	"Escape",
	"Convert",
	"VK_NONCONVERT",
	"Accept",
	"VK_MODECHANGE",
	"Space",
	"Page UP",
	"Page DOWN",
	"Enjd",
	"Home",
	"Left",
	"Up",
	"Right",
	"Down",
	"Select",
	"Print",
	"Execute",
	"Print Screen",
	"Insert",
	"Delete",
	"Help",
	"0",
	"1",
	"2",
	"3",
	"4",
	"5",
	"6",
	"7",
	"8",
	"9",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"A",
	"B",
	"C",
	"D",
	"E",
	"F",
	"G",
	"H",
	"I",
	"J",
	"K",
	"L",
	"M",
	"N",
	"O",
	"P",
	"Q",
	"R",
	"S",
	"T",
	"U",
	"V",
	"W",
	"X",
	"Y",
	"Z",
	"L Win",
	"R Win",
	"Apps",
	"Unknown",
	"Sleep",
	"NUMPAD 0",
	"NUMPAD 1",
	"NUMPAD 2",
	"NUMPAD 3",
	"NUMPAD 4",
	"NUMPAD 5",
	"NUMPAD 6",
	"NUMPAD 7",
	"NUMPAD 8",
	"NUMPAD 9",
	"Multiply",
	"Add",
	"Separator",
	"Subtract",
	"Decimal",
	"Divide",
	"F1",
	"F2",
	"F3",
	"F4",
	"F5",
	"F6",
	"F7",
	"F8",
	"F9",
	"F10",
	"F11",
	"F12",
	"F13",
	"F14",
	"F15",
	"F16",
	"F17",
	"F18",
	"F19",
	"F20",
	"F21",
	"F22",
	"F23",
	"F24",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Num loock",
	"Scroll loock",
	"VK_OEM_NEC_EQUAL",
	"VK_OEM_FJ_MASSHOU",
	"VK_OEM_FJ_TOUROKU",
	"VK_OEM_FJ_LOYA",
	"VK_OEM_FJ_ROYA",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"Unknown",
	"L Shift",
	"R Shift",
	"L Control",
	"R Control",
	"L Menu",
	"R Menu"
};

bool Hotkey(const char* label, int* k, const ImVec2& size_arg = ImVec2(0, 0)) {
	ImGuiWindow* window = ImGui::GetCurrentWindow();
	if (window->SkipItems)
		return false;

	ImGuiContext& g = *GImGui;
	ImGuiIO& io = g.IO;
	const ImGuiStyle& style = g.Style;

	const ImGuiID id = window->GetID(label);

	char buf_display[64] = "None";
	if (*k != 0 && g.ActiveId != id) {
		strcpy_s(buf_display, KeyNames[*k]);
	} else if (g.ActiveId == id) {
		strcpy_s(buf_display, "<Press a key>");
	}

	const ImVec2 label_size = ImGui::CalcTextSize(label, NULL, true);
	const ImVec2 label_size2 = ImGui::CalcTextSize(buf_display, NULL, true);

	ImVec2 size = ImGui::CalcItemSize(size_arg, label_size.x + label_size2.x + style.FramePadding.x * 2 + style.ItemInnerSpacing.x * 2, 
		label_size.y + style.FramePadding.y * 2.0f);
	const ImRect frame_bb(window->DC.CursorPos + ImVec2(label_size.x + style.ItemInnerSpacing.x, 0.0f), window->DC.CursorPos + size);
	const ImRect total_bb(window->DC.CursorPos, frame_bb.Max);

	ImGui::ItemSize(total_bb, style.FramePadding.y);
	if (!ImGui::ItemAdd(total_bb, id))
		return false;

	const bool focus_requested = ImGui::FocusableItemRegister(window, id);
	const bool focus_requested_by_code = focus_requested && (g.FocusRequestCurrWindow == window && g.FocusRequestCurrCounterRegular == window->DC.FocusCounterRegular);
	const bool focus_requested_by_tab = focus_requested && !focus_requested_by_code;

	const bool hovered = ImGui::ItemHoverable(frame_bb, id);

	if (hovered) {
		ImGui::SetHoveredID(id);
		g.MouseCursor = ImGuiMouseCursor_Hand;
	}

	const bool user_clicked = hovered && io.MouseClicked[0];

	if ((focus_requested && !focus_requested_by_tab) || user_clicked) { //allow the tab key to be also ussed
		if (g.ActiveId != id) {
			// Start edition
			memset(io.MouseDown, 0, sizeof(io.MouseDown));
			memset(io.KeysDown, 0, sizeof(io.KeysDown));
			*k = 0;
		}
		ImGui::SetActiveID(id, window);
		ImGui::FocusWindow(window);
	} else if (io.MouseClicked[0]) {
		// Release focus when we click outside
		if (g.ActiveId == id)
			ImGui::ClearActiveID();
	}

	bool value_changed = false;
	int key = *k;

	if (g.ActiveId == id) {
		for (auto i = 0; i < 5; i++) {
			if (io.MouseDown[i]) {
				switch (i) {
				case 0: key = VK_LBUTTON; break;
				case 1: key = VK_RBUTTON; break;
				case 2: key = VK_MBUTTON; break;
				case 3: key = VK_XBUTTON1; break;
				case 4: key = VK_XBUTTON2; break;
				}
				value_changed = true;
				ImGui::ClearActiveID();
			}
		}
		if (!value_changed) {
			for (auto i = VK_BACK; i <= VK_RMENU; i++) {
				if (io.KeysDown[i]) {
					key = i;
					value_changed = true;
					ImGui::ClearActiveID();
				}
			}
		}

		if (ImGui::IsKeyPressedMap(ImGuiKey_Escape)) {
			*k = 0;
			ImGui::ClearActiveID();
		} else {
			*k = key;
		}
	}

	// Render
	// Select which buffer we are going to display. When ImGuiInputTextFlags_NoLiveEdit is Set 'buf' might still be the old value. We Set buf to NULL to prevent accidental usage from now on.


	ImGui::RenderFrame(frame_bb.Min, frame_bb.Max, 
		ImGui::GetColorU32(style.Colors[g.ActiveId == id ? ImGuiCol_ButtonActive : ImGuiCol_Button]), true, style.FrameRounding);


	const ImRect clip_rect(frame_bb.Min.x, frame_bb.Min.y, frame_bb.Min.x + size.x, frame_bb.Min.y + size.y); // Not using frame_bb.Max because we have adjusted size
	ImVec2 render_pos = frame_bb.Min + style.FramePadding;
	ImGui::RenderTextClipped(frame_bb.Min + style.FramePadding, frame_bb.Max - style.FramePadding, buf_display, NULL, NULL, style.ButtonTextAlign, &clip_rect);
	//RenderTextClipped(frame_bb.Min + style.FramePadding, frame_bb.Max - style.FramePadding, buf_display, NULL, NULL, GetColorU32(ImGuiCol_Text), style.ButtonTextAlign, &clip_rect);
	//draw_window->DrawList->AddText(g.Font, g.FontSize, render_pos, GetColorU32(ImGuiCol_Text), buf_display, NULL, 0.0f, &clip_rect);

	if (label_size.x > 0)
		ImGui::RenderText(ImVec2(total_bb.Min.x, frame_bb.Min.y + style.FramePadding.y), label);

	//ImGui::GetForegroundDrawList()->AddRect(frame_bb.Min, frame_bb.Max, ImColor(255, 0, 255));
	//ImGui::GetForegroundDrawList()->AddRect(total_bb.Min, total_bb.Max, ImColor(0, 0, 255));

	return value_changed;
}

enum Tabs_t {
	TAB_AIM = 0,
	TAB_VISUALS,
	TAB_MISC,
	TAB_SKINS,
};

void draw_TAB_AIM(Menu::Settings* settings) {
	if (childWithTitle("Aimbot", ImVec2(0, 0))) {
		static const char* aimbotmodes[] = { "Off", "Always", "On key" };
		if (settings->aimbot_mode == 2) { //on key
			Hotkey("##aim_key", &settings->aimbot_key);
			ImGui::SameLine();
		}
		ImGui::SetNextItemWidth(100);
		ImGui::Combo("Mode", &settings->aimbot_mode, aimbotmodes, IM_ARRAYSIZE(aimbotmodes));

		ImGui::Checkbox("Silent", (bool*)&settings->esp_box);
		ImGui::Checkbox("Autoscope", (bool*)&settings->esp_box);
		ImGui::Checkbox("Friendly fire", (bool*)&settings->esp_box);
		ImGui::Checkbox("Box", (bool*)&settings->esp_box);
		ImGui::Checkbox("Box", (bool*)&settings->esp_box);


	}

	ImGui::EndChild();

}

void draw_TAB_VISUALS(Menu::Settings* settings) {

	if (childWithTitle("Player ESP", ImVec2(180, 264))) {
		static const char* espmodes[] = { "Off", "Always", "On death" };
		ImGui::Combo("Mode", &settings->esp_mode, espmodes, IM_ARRAYSIZE(espmodes));
		static const char* targetslist[] = { "All", "Enemies", "Teammates" };
		ImGui::Combo("Target", &settings->esp_team, targetslist, IM_ARRAYSIZE(targetslist));
		ImGui::Checkbox("Box", (bool*)&settings->esp_box);
		ImGui::Checkbox("Skeleton", (bool*)&settings->esp_skeleton);
		ImGui::Checkbox("Snaplines", (bool*)&settings->esp_snaplines);
		ImGui::Checkbox("Name", (bool*)&settings->esp_name);
		ImGui::Checkbox("Health", (bool*)&settings->esp_health_num);
		ImGui::Checkbox("Healthbar", (bool*)&settings->esp_health_bar);
		ImGui::Checkbox("Weapon", (bool*)&settings->esp_weapon);
		ImGui::Checkbox("Out of fov arrow", (bool*)&settings->esp_oov_arrow);
		ImGui::Checkbox("Info flags", (bool*)&settings->esp_flags);
	}
	ImGui::EndChild();
}

//GL_ALPHA_TEST

void draw_TAB_MISC(Menu::Settings* settings) {

}

void draw_TAB_SKINS(Menu::Settings* settings) {

}

GLuint configReleadTexture = NULL;

std::vector<std::string> configs;

void __stdcall Menu::DrawMenu(Settings* settings) {

	if (!Menu::open)
		return;

	ImGui::ShowDemoWindow();

	if (ImGui::Begin("Debug")) {
		ImColor col = ImColor(settings->menu_color);
		if (ImGui::ColorEdit3("GUI color", &col.Value.x, ImGuiColorEditFlags_NoInputs)) {
			UpdateColors(col);
			settings->menu_color = (ImU32)col;
		}
		ImGui::SliderFloat("debug1", &settings->debug1, 0, 500);
		ImGui::SliderFloat("debug2", &settings->debug2, 0, 500);
		ImGui::SliderInt("debug3", &settings->debug3, 0, 5);
		ImGui::SliderInt("debug4", &settings->debug4, 0, 5);
		ImGui::Checkbox("debug5", (bool*)&settings->debug5);
		ImGui::Checkbox("debug6", (bool*)&settings->debug6);
	}
	ImGui::End();

	if (Menu::open == 1) {
		Menu::open++;
		ImGui::GetIO().DeltaTime = 0.f;
		for (auto item : animated_buttons) {
			animatedbtn_t* b = &animated_buttons[item.first];
			b->mode = 100;
			b->speed = 0.f;
			b->w = 0.f;
		}
	}
	ImGui::PushStyleVar(ImGuiStyleVar_WindowPadding, ImVec2(0, 0));
	ImGui::SetNextWindowSize(ImVec2(600, 400));
	if (ImGui::Begin("nezu.cc", 0, ImGuiWindowFlags_NoCollapse | ImGuiWindowFlags_NoResize)) {
		static int selectedTab = 0;
		{
			ImGui::PushStyleVar(ImGuiStyleVar_ItemSpacing, ImVec2(0, 0));
			ImGui::Columns(2, 0, true);
			ImGui::SetColumnWidth(-1, 150);
			ImGui::GetCurrentWindow()->DC.CurrentColumns->Flags |= ImGuiColumnsFlags_NoResize;
			static const char* menus[] = { "Aim", "Visuals", "Misc", "Skins" };
			const ImVec2 size(ImGui::GetContentRegionAvailWidth(), 50);
			for (int i = 0; i < IM_ARRAYSIZE(menus); i++)
				if (animatedMenuButton(menus[i], size, selectedTab == i)) selectedTab = i;
			
			ImGui::PushStyleVar(ImGuiStyleVar_WindowPadding, ImVec2(4, 4));
			ImGui::PushStyleVar(ImGuiStyleVar_ItemSpacing, ImVec2(4, 4));
			if (childWithTitle("Configs")) {
				ImGui::PushItemWidth(-1);
				static int selectedconfigindex = 0;
				static std::string configname = "";
				static bool initX = false; if (!initX) { initX = true; configname = configs.size() > 0 ? configs[0] : ""; }
				static float listsize = 30;
				ImVec2 avail = ImGui::GetContentRegionAvail();
				if (ImGui::ListBoxHeader("##configfiles", ImVec2(avail.x, avail.y - listsize))) {
					ImGuiListClipper clipper((int)configs.size(), ImGui::GetTextLineHeightWithSpacing());
					while (clipper.Step())
						for (int i = clipper.DisplayStart; i < clipper.DisplayEnd; i++) {
							const bool item_selected = (i == selectedconfigindex);
							ImGui::PushID(i);
							if (ImGui::Selectable(configs[i].c_str(), item_selected)) {
								selectedconfigindex = i;
								if (selectedconfigindex >= 0 && selectedconfigindex < (int)configs.size())
									configname = configs[selectedconfigindex];
							}
							if (item_selected) ImGui::SetItemDefaultFocus();
							ImGui::PopID();
						}
					ImGui::ListBoxFooter();
				}
				listsize = 0;
				ImGuiStyle style = ImGui::GetStyle();
				static char xd[100] = "no";

				ImGui::GetCurrentWindow()->DC.ItemWidth = ImMax(1.0f, ImGui::GetContentRegionMaxAbs().x - ImGui::GetCurrentWindow()->DC.CursorPos.x);
				ImGui::InputText("##selectedconfigname", xd, sizeof(xd));
				listsize += ImGui::GetItemRectSize().y + style.ItemSpacing.y;
				static float buttonH = 20;
				if (ImGui::Button("Load", ImVec2(ImGui::GetContentRegionAvailWidth() - (configReleadTexture != NULL ? buttonH + style.ItemSpacing.x : 0), 0)))
					void();//loadConfig(configs[selectedconfigindex].c_str());
				if (configReleadTexture == NULL) {
					GLint last_texture;
					glGetIntegerv(GL_TEXTURE_BINDING_2D, &last_texture);
					glGenTextures(1, &configReleadTexture);
					glBindTexture(GL_TEXTURE_2D, configReleadTexture);
					glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
					glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
#ifdef GL_UNPACK_ROW_LENGTH
					glPixelStorei(GL_UNPACK_ROW_LENGTH, 0);
#endif
					glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, 16, 16, 0, GL_RGBA, GL_UNSIGNED_BYTE, reloadImg);
					glBindTexture(GL_TEXTURE_2D, last_texture);
				} else {
					ImGui::SameLine();
					buttonH = ImGui::GetCurrentWindow()->DC.CurrLineSize.y;
					if (ImGui::ImageButton((void*)(intptr_t)configReleadTexture, 
						ImVec2(buttonH - style.FramePadding.x * 2, buttonH - style.FramePadding.y * 2)))
						void();//refreshConfigs();
				}
				listsize += ImGui::GetItemRectSize().y + style.ItemSpacing.y;
				if (ImGui::Button("Save", ImVec2(ImGui::GetContentRegionAvailWidth() / 2, 0))) {
					//saveConfig(configname.c_str());
					//refreshConfigs();
				}
				ImGui::SameLine();
				if (ImGui::Button("Delete", ImVec2(ImGui::GetContentRegionAvailWidth(), 0))) {
					//deleteConfig(configname.c_str());
					configname = "";
					//refreshConfigs();
				}
				listsize += ImGui::GetItemRectSize().y + style.ItemSpacing.y;
				ImGui::PushStyleColor(ImGuiCol_Button, 0x910000FF);
				ImGui::PushStyleColor(ImGuiCol_ButtonHovered, 0xba0000FF);
				ImGui::PushStyleColor(ImGuiCol_ButtonActive, 0xba0000FF);
				if (ImGui::Button("Unload", ImVec2(ImGui::GetContentRegionAvailWidth(), ImGui::GetCurrentWindow()->DC.LastItemRect.GetHeight() * 1.5f)))
					H::UnhookAll();
				ImGui::PopStyleColor(3);
				listsize += ImGui::GetItemRectSize().y + style.ItemSpacing.y;
				ImGui::PopItemWidth();
			}
			ImGui::EndChild();
			ImGui::PopStyleVar(2);
			ImGui::PushStyleVar(ImGuiStyleVar_ItemSpacing, ImVec2(0, 0));
			ImGui::NextColumn();
			ImGui::PopStyleVar();
			ImGui::PopStyleVar();
		}
		ImGui::PushStyleVar(ImGuiStyleVar_WindowPadding, ImVec2(8, 8));
		if (ImGui::BeginChild("##settings", ImVec2(), true)) {
			switch (selectedTab) {
			case TAB_AIM:		draw_TAB_AIM(settings);          break;
			case TAB_VISUALS:   draw_TAB_VISUALS(settings);      break;
			case TAB_MISC:      draw_TAB_MISC(settings);         break;
			case TAB_SKINS:     draw_TAB_SKINS(settings);        break;
			default:            selectedTab = TAB_AIM;   break;
			}
		}
		ImGui::EndChild();
		ImGui::PopStyleVar();
		ImGui::Columns();
	}
	ImGui::End();
	ImGui::PopStyleVar();

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
	colors[ImGuiCol_Header] =               ImVec4(1.00f, 1.00f, 1.00f, 0.24f);
	colors[ImGuiCol_HeaderHovered] =        ImVec4(1.00f, 1.00f, 1.00f, 0.31f);
	colors[ImGuiCol_HeaderActive] =         ImVec4(1.00f, 1.00f, 1.00f, 0.39f);
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