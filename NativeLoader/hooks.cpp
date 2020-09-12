#include "hooks.h"
#include "utils.h"
#include "detours.h"
#pragma comment(lib, "detours.lib")
#include "globals.h"

f_LoadLibraryExW fLoadLibraryExW = NULL;
f_wglSwapBuffers fwglSwapBuffers = NULL;
f_SetCursorPos fSetCursorPos = NULL;
f_GetCursorPos fGetCursorPos = NULL;
f_GetRawInputData fGetRawInputData = NULL;

void H::HookLoadLibraryExW() {

	oLoadLibraryExW = (f_LoadLibraryExW)GetProcAddress(LoadLibrary(TEXT("Kernel32.dll")), "LoadLibraryExW");
	if (!oLoadLibraryExW) 
		return;

	fLoadLibraryExW = oLoadLibraryExW;

	DetourTransactionBegin();
    DetourUpdateThread(GetCurrentThread());
    DetourAttach(&(PVOID&)oLoadLibraryExW, H::LoadLibraryExW);
    DetourTransactionCommit();

}

void H::HookSwapBuffers(HMODULE hModSteamOverlay) {

	owglSwapBuffers = (f_wglSwapBuffers)FindPattern(hModSteamOverlay, "40 53 48 83 EC 30 48 8B D9 48 8D 54 24");
	if (!owglSwapBuffers) 
		return;

	fwglSwapBuffers = owglSwapBuffers;

	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach(&(PVOID&)owglSwapBuffers, H::wglSwapBuffers);
	DetourTransactionCommit();

}

void H::HookWindow(HWND hWindow) {

	/*HMODULE hUser32 = LoadLibrary(TEXT("User32.dll"));
	oSetCursorPos = (f_SetCursorPos)GetProcAddress(hUser32, "SetCursorPos");
	oGetCursorPos = (f_GetCursorPos)GetProcAddress(hUser32, "GetCursorPos");
	oGetRawInputData = (f_GetRawInputData)GetProcAddress(hUser32, "GetRawInputData");
	if (!oGetCursorPos || !oSetCursorPos || !oGetRawInputData)
		return;

	fSetCursorPos = oSetCursorPos;
	fGetCursorPos = oGetCursorPos;
	fGetRawInputData = oGetRawInputData;

	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach(&(PVOID&)oSetCursorPos, H::SetCursorPos);
	DetourAttach(&(PVOID&)oGetCursorPos, H::GetCursorPos);
	DetourAttach(&(PVOID&)oGetRawInputData, H::GetRawInputData);
	DetourTransactionCommit();

	oWndProc = (WNDPROC)SetWindowLongPtr(hWindow, GWLP_WNDPROC, (LONG_PTR)H::WndProc);*/

}

void H::UnhookAll() {

	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	if (oLoadLibraryExW) DetourDetach(&(PVOID&)oLoadLibraryExW, H::LoadLibraryExW);
	if (owglSwapBuffers) DetourDetach(&(PVOID&)owglSwapBuffers, H::wglSwapBuffers);
	if (oSetCursorPos) DetourDetach(&(PVOID&)oSetCursorPos, H::SetCursorPos);
	if (oGetCursorPos) DetourDetach(&(PVOID&)oGetCursorPos, H::GetCursorPos);
	if (oGetRawInputData) DetourDetach(&(PVOID&)oGetRawInputData, H::GetRawInputData);
	DetourTransactionCommit();

	if(oWndProc) SetWindowLongPtr(G::hGameWindow, GWLP_WNDPROC, (LONG_PTR)oWndProc);

}