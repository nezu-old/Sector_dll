#include "hooks.h"

f_LoadLibraryExW H::oLoadLibraryExW = NULL;

HMODULE WINAPI H::LoadLibraryExW(LPCWSTR lpLibFileName, HANDLE hFile, DWORD dwFlags) {
    if (wcsstr(lpLibFileName, L"gameoverlayrenderer64.dll")) {
        HMODULE hModule = oLoadLibraryExW(lpLibFileName, hFile, dwFlags);
		HookSwapBuffers(hModule);
        return hModule;
	}
    return oLoadLibraryExW(lpLibFileName, hFile, dwFlags);
}
