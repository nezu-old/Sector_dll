#pragma once
#include "imgui/imgui.h"

#undef DrawText

struct DrawingFunctions {
	void* DrawRect;
	void* DrawFilledRect;
	void* DrawLine;
	void* DrawText;
};

namespace D {

	enum TextAlignment : int {
		ALIGN_TOP = 1,
		ALIGN_BOTTOM = 2,
		ALIGN_LEFT = 4,
		ALIGN_RIGHT = 8,
		ALIGN_VCENTER = 16,
		ALIGN_HCENTER = 32
	};

	extern ImDrawList* drawList;
	extern ImFont* gameFont;

	DrawingFunctions GetDrawinfFunctions();

	void __stdcall DrawRect(int x, int y, int w, int h, int t, ImU32 color);
	void __stdcall DrawRectFilled(int x, int y, int w, int h, ImU32 color);
	void __stdcall DrawLine(float x1, float y1, float x2, float y2, float t, ImU32 color);
	void __stdcall DrawText(const char* text, float x, float y, float size, ImU32 color, int align);

}

