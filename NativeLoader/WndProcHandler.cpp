#include "hooks.h"
#include "imgui/imgui_impl_win32.h"
#include "menu.h"
#include "globals.h"
#include <stdio.h>

extern IMGUI_IMPL_API LRESULT ImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);

WNDPROC H::oWndProc = NULL;

LRESULT CALLBACK H::WndProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam) {
	if ((uMsg == WM_KEYUP || uMsg == WM_KEYDOWN) && wParam == VK_INSERT) {
		if(uMsg == WM_KEYUP)
			Menu::open = (Menu::open == 0) ? 1 : 0;
		return FALSE;
	}

	if (Menu::open) {
		G::bIsImguiRunning = true;
		bool ret = ImGui_ImplWin32_WndProcHandler(hWnd, uMsg, wParam, lParam) || uMsg == WM_MOUSEMOVE || uMsg == WM_NCHITTEST || uMsg == WM_GETDLGCODE;
		G::bIsImguiRunning = false;

		if (ret && !((uMsg >= WM_KEYFIRST && uMsg <= WM_KEYLAST) && !ImGui::GetIO().WantCaptureKeyboard))
			return true;
	}
	return CallWindowProc(oWndProc, hWnd, uMsg, wParam, lParam);
}
