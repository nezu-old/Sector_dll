#include "drawing.h"

ImDrawList* D::drawList = NULL;
ImFont* D::gameFont = NULL;

DrawingFunctions D::GetDrawinfFunctions() {
	DrawingFunctions f = { 0 };
	f.DrawRect = &D::DrawRect;
	f.DrawFilledRect = &D::DrawRectFilled;
	f.DrawText = &D::DrawText;
	return f;
}

void __stdcall D::DrawRect(int x, int y, int w, int h, int t, ImU32 color) {
	drawList->AddRect(ImVec2((float)x, (float)y), ImVec2((float)(x + w), (float)(y + h)), color, 0.f, ImDrawCornerFlags_All, (float)t);
}

void __stdcall D::DrawRectFilled(int x, int y, int w, int h, ImU32 color) {
	drawList->AddRectFilled(ImVec2((float)x, (float)y), ImVec2((float)(x + w), (float)(y + h)), color, 0.f, ImDrawCornerFlags_All);
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
