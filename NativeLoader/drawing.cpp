#include "drawing.h"

namespace Menu {
	struct Settings;
	void __stdcall DrawMenu(Settings* settings);
}

ImDrawList* D::drawList = NULL;
ImFont* D::gameFont = NULL;

DrawingFunctions D::GetDrawinfFunctions() {
	DrawingFunctions f = { 0 };
	f.DrawMenu = &Menu::DrawMenu;
	f.DrawRect = &D::DrawRect;
	f.DrawFilledRect = &D::DrawRectFilled;
	f.DrawLine = &D::DrawLine;
	f.DrawText = &D::DrawText;
	f.DrawTextSmall = &D::DrawTextSmall;
	return f;
}

void __stdcall D::DrawRect(int x, int y, int w, int h, int t, ImU32 color) {
	drawList->AddRect(ImVec2((float)x, (float)y), ImVec2((float)(x + w), (float)(y + h)), color, 0.f, ImDrawCornerFlags_All, (float)t);
}

void __stdcall D::DrawRectFilled(int x, int y, int w, int h, ImU32 color) {
	drawList->AddRectFilled(ImVec2((float)x, (float)y), ImVec2((float)(x + w), (float)(y + h)), color, 0.f, ImDrawCornerFlags_All);
}

void __stdcall D::DrawLine(float x1, float y1, float x2, float y2, float t, ImU32 color) {
	drawList->AddLine(ImVec2(x1, y1), ImVec2(x2, y2), color, t);
}

void __stdcall D::DrawText(const char * text, float x, float y, float size, ImU32 color, int align) {
	const char* text_end = text + strlen(text);
	if (align == 0)
		align = (ALIGN_TOP | ALIGN_LEFT);
	if (align != (ALIGN_TOP | ALIGN_LEFT)) {
		ImVec2 text_size = D::gameFont->CalcTextSizeA(size, FLT_MAX, 0.0f, text, text_end);
		if (align & ALIGN_RIGHT)
			x -= text_size.x;
		else if (align & ALIGN_HCENTER)
			x -= text_size.x / 2.f;
		if (align & ALIGN_BOTTOM)
			y -= text_size.y;
		else if (align & ALIGN_VCENTER)
			y -= text_size.y / 2.f;
	}
	D::gameFont->RenderText(D::drawList, size, ImVec2(x, y), color, D::drawList->_CmdHeader.ClipRect, text, text_end, 0, false);
}

void __stdcall D::DrawTextSmall(const char* text, float x, float y, ImU32 color, int align) {
	ImFont * cur_font = D::gameFont;
	D::gameFont = ImGui::GetFont();
	DrawText(text, x+1, y+1, D::gameFont->FontSize, 0xFF000000, align);
	DrawText(text, x, y, D::gameFont->FontSize, color, align);
	D::gameFont = cur_font;
}