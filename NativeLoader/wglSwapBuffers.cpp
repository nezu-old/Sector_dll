#include "hooks.h"
#include "globals.h"

#define GLEW_STATIC
#include "GL\glew.h"
#pragma comment(lib, "glew32s.lib")
#pragma comment(lib, "OpenGL32.lib")

#include "imgui/imgui.h"
#define IMGUI_DEFINE_MATH_OPERATORS
#include "imgui/imgui_internal.h"
#include "imgui/imgui_impl_opengl3.h"
#include "imgui/imgui_impl_win32.h"

#include "menu.h"
#include "drawing.h"
#include "BebasNeueRegular.h"

f_wglSwapBuffers H::owglSwapBuffers = NULL;

BOOL __stdcall H::wglSwapBuffers(HDC hDc) {
	static bool once = true;
	if (once) {
		once = false;

		G::hGameWindow = WindowFromDC(hDc);
		HookWindow(G::hGameWindow);

		glewInit();
		ImGui::CreateContext();
		ImGui_ImplWin32_Init(G::hGameWindow);

		ImGuiIO& io = ImGui::GetIO();

		io.Fonts->AddFontDefault();
		ImFontConfig gameFontConfig;
		gameFontConfig.GlyphOffset = ImVec2(0, 1);
		D::gameFont = io.Fonts->AddFontFromMemoryTTF(BebasNeueRegular_ttf, sizeof(BebasNeueRegular_ttf), 24, &gameFontConfig);
			
		ImGui_ImplOpenGL3_Init();

		ImGuiStyle& style = ImGui::GetStyle();
		style.WindowPadding = ImVec2(10, 10);
		style.FramePadding = ImVec2(4, 3);
		style.ItemSpacing = ImVec2(4, 2);
		style.ItemInnerSpacing = ImVec2(3, 2);
		style.IndentSpacing = 4;
		style.ScrollbarSize = 15;
		style.GrabMinSize = 15;
		style.WindowBorderSize = 1;
		style.ChildBorderSize = 1;
		style.PopupBorderSize = 1;
		style.FrameBorderSize = 0;
		style.TabBorderSize = 0;
		style.WindowRounding = 3;
		style.ChildRounding = 3;
		style.FrameRounding = 3;
		style.PopupRounding = 3;
		style.ScrollbarRounding = 3;
		style.GrabRounding = 0;
		style.TabRounding = 3;
		style.WindowTitleAlign = ImVec2(0.5, 0.5);
		style.WindowMenuButtonPosition = ImGuiDir_None;
		style.ColorButtonPosition = ImGuiDir_Right;
		style.ButtonTextAlign = ImVec2(0.5, 0.5);
		style.SelectableTextAlign = ImVec2(0, 0);
		style.DisplaySafeAreaPadding = ImVec2(0, 0);
		Menu::UpdateColors(ImColor(1.f, 0.f, 0.f, 1.f));
	}

	G::bIsImguiRunning = true;

	ImGui_ImplOpenGL3_NewFrame();
	ImGui_ImplWin32_NewFrame();
	ImGui::NewFrame();

	D::drawList = ImGui::GetBackgroundDrawList();

	if (G::drawCallback) {
		DrawingFunctions functions = D::GetDrawinfFunctions();
		typedef void(__stdcall* f_DrawCallback)(DrawingFunctions* drawingFunctions);
		reinterpret_cast<f_DrawCallback>(G::drawCallback)(&functions);
	}

	ImGui::Render();
	ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());

	G::bIsImguiRunning = false;

    return owglSwapBuffers(hDc);
}
