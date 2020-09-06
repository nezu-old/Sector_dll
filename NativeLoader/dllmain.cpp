#define _CRT_SECURE_NO_WARNINGS
#include <windows.h>
#include <tchar.h>
#include "hooks.h"
#include "globals.h"
#include <cstdio>

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
	if (ul_reason_for_call == DLL_PROCESS_ATTACH) {
		G::drawCallback = lpReserved;

		HMODULE hOverlay = GetModuleHandle(TEXT("gameoverlayrenderer64.dll"));
		if (hOverlay) {
			H::HookSwapBuffers(hOverlay);
		}
		else {
			H::HookLoadLibraryExW();
		}

	}
	else if (ul_reason_for_call == DLL_PROCESS_DETACH) {
		//H::UnhookAll();
	}
    return TRUE;
}


