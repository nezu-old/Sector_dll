#define _CRT_SECURE_NO_WARNINGS
#include <windows.h>
#include <tchar.h>
#include "hooks.h"
#include <cstdio>

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
    if (ul_reason_for_call == DLL_PROCESS_ATTACH) {

       /* AllocConsole();
        freopen("CONOUT$", "w", stdout);
		freopen("CONIN$", "r", stdin);*/
		
		
		HMODULE hOverlay = GetModuleHandle(TEXT("gameoverlayrenderer64.dll"));
		if (hOverlay) {
			H::HookSwapBuffers(hOverlay);
		} else {
			H::HookLoadLibraryExW();
		}

		sizeof(ULONG);
		sizeof(int);

    }
    return TRUE;
}


