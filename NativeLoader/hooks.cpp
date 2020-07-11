#include "hooks.h"
#include "utils.h"
#include "detours.h"
#pragma comment(lib, "detours.lib")

void H::HookLoadLibraryExW() {

	oLoadLibraryExW = (f_LoadLibraryExW)GetProcAddress(LoadLibrary(TEXT("Kernel32.dll")), "LoadLibraryExW");
	if (!oLoadLibraryExW) 
		return;

	DetourTransactionBegin();
    DetourUpdateThread(GetCurrentThread());
    DetourAttach(&(PVOID&)oLoadLibraryExW, H::LoadLibraryExW);
    DetourTransactionCommit();

}

void H::HookSwapBuffers(HMODULE hModSteamOverlay) {

	owglSwapBuffers = (f_wglSwapBuffers)FindPattern(hModSteamOverlay, "40 53 48 83 EC 30 48 8B D9 48 8D 54 24");
	if (!owglSwapBuffers) 
		return;

	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach(&(PVOID&)owglSwapBuffers, H::wglSwapBuffers);
	DetourTransactionCommit();

}

void H::HookWindow(HWND hWindow) {

	HMODULE hUser32 = LoadLibrary(TEXT("User32.dll"));
	oSetCursorPos = (f_SetCursorPos)GetProcAddress(hUser32, "SetCursorPos");
	oGetCursorPos = (f_GetCursorPos)GetProcAddress(hUser32, "GetCursorPos");
	oGetRawInputData = (f_GetRawInputData)GetProcAddress(hUser32, "GetRawInputData");
	if (!oGetCursorPos || !oSetCursorPos || !oGetRawInputData)
		return;

	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach(&(PVOID&)oSetCursorPos, H::SetCursorPos);
	DetourAttach(&(PVOID&)oGetCursorPos, H::GetCursorPos);
	DetourAttach(&(PVOID&)oGetRawInputData, H::GetRawInputData);
	DetourTransactionCommit();

	oWndProc = (WNDPROC)SetWindowLongPtr(hWindow, GWLP_WNDPROC, (LONG_PTR)H::WndProc);

}
