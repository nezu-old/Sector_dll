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

typedef void(__stdcall* f_DrawCallback)(DrawingFunctions* drawingFunctions);
f_DrawCallback drawCallback = NULL;

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
		ImGui::StyleColorsDark();
		
		HMODULE hModuleDll = GetModuleHandle(TEXT("Sector_dll.dll"));
		if (hModuleDll) {
			drawCallback = (f_DrawCallback)GetProcAddress(hModuleDll, "DrawCallback");
		}
	}

	G::bIsImguiRunning = true;

	ImGui_ImplOpenGL3_NewFrame();
	ImGui_ImplWin32_NewFrame();
	ImGui::NewFrame();

	D::drawList = ImGui::GetBackgroundDrawList();

	if (drawCallback) {
		DrawingFunctions functions = D::GetDrawinfFunctions();
		drawCallback(&functions);
	}

	if (Menu::open) {
		Menu::DrawMenu();
	}

	ImGui::Render();
	ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());

	G::bIsImguiRunning = false;

    return owglSwapBuffers(hDc);
}
