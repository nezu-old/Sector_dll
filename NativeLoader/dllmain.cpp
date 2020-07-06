#define _CRT_SECURE_NO_WARNINGS
#include <windows.h>
#include <tlhelp32.h>
#include <tchar.h>
#include <stdio.h>
#include <shlwapi.h>
#include <metahost.h>
#include <psapi.h>
#include <string>
#include <gl\GL.h>
#include "detours.h"

//#pragma comment(lib, "mscoree.lib")
#pragma comment(lib, "detours.lib")
#pragma comment(lib, "OpenGL32.lib")
#define ARRAY_LEN(x) ((sizeof(x)/sizeof(0[x])) / ((size_t)(!(sizeof(x) % sizeof(0[x])))))

DWORD64 FindPattern(HMODULE hMod, std::string pattern);

typedef BOOL(__stdcall* f_wglSwapBuffers)(HDC hDc);
f_wglSwapBuffers owglSwapBuffers = NULL;

void SetupOrtho() {

	glPushAttrib( GL_ALL_ATTRIB_BITS );
	glPushMatrix( );
	GLint viewport [ 4 ];
	glGetIntegerv( GL_VIEWPORT, viewport );
	glViewport( 0, 0, viewport [ 2 ], viewport [ 3 ] );
	glMatrixMode( GL_PROJECTION );
	glLoadIdentity( );
	glOrtho( 0, viewport [ 2 ], viewport [ 3 ], 0, -1, 1 );
	glMatrixMode( GL_MODELVIEW );
	glLoadIdentity( );
	glDisable( GL_DEPTH_TEST );
}

void RestoreGL()
{
	glPopMatrix( );
	glPopAttrib( );    
}

BOOL __stdcall wglSwapBuffersHook(HDC hDc)
{
	SetupOrtho();

	glBegin(GL_LINES);
	glVertex2i(0, 0);
	glVertex2i(500, 500);
	glEnd();

	RestoreGL();
    return owglSwapBuffers(hDc);
}

typedef HMODULE(WINAPI* f_LoadLibraryExW)(LPCWSTR, HANDLE, DWORD);
f_LoadLibraryExW oLoadLibraryExW = LoadLibraryExW;

HMODULE WINAPI LoadLibraryExWHook(LPCWSTR lpLibFileName, HANDLE hFile, DWORD dwFlags) {
    if (wcsstr(lpLibFileName, L"gameoverlayrenderer64.dll")) {
        HMODULE hModule = oLoadLibraryExW(lpLibFileName, hFile, dwFlags);
		
		owglSwapBuffers = (f_wglSwapBuffers)FindPattern(hModule, "40 53 48 83 EC 30 48 8B D9 48 8D 54 24");

		_tprintf(TEXT("[nezu.cc] Address of GameOverlayRenderer.wglSwapBuffers: %llX\n"), (DWORD64)owglSwapBuffers);
		DetourTransactionBegin();
		DetourUpdateThread(GetCurrentThread());
		DetourAttach(&(PVOID&)owglSwapBuffers, wglSwapBuffersHook);
		DetourTransactionCommit();

        return hModule;
	}     
	wprintf(L"LoadEx: %s\n", lpLibFileName);
    return oLoadLibraryExW(lpLibFileName, hFile, dwFlags);
}


BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
    if (ul_reason_for_call == DLL_PROCESS_ATTACH) {

        AllocConsole();
        freopen("CONOUT$", "w", stdout);
        freopen("CONIN$", "r", stdin);
		
		

        DetourTransactionBegin();
        DetourUpdateThread(GetCurrentThread());
        DetourAttach(&(PVOID&)oLoadLibraryExW, LoadLibraryExWHook);
        DetourTransactionCommit();

    }
    return TRUE;
}

#define INRANGE(x,a,b)    (x >= a && x <= b)
#define getBits( x )    (INRANGE((x&(~0x20)),'A','F') ? ((x&(~0x20)) - 'A' + 0xa) : (INRANGE(x,'0','9') ? x - '0' : 0))
#define getByte( x )    (getBits(x[0]) << 4 | getBits(x[1]))

DWORD64 FindPattern(HMODULE hMod, std::string pattern) {
	const char* pat = pattern.c_str();
	DWORD64 firstMatch = 0;
	DWORD64 rangeStart = (DWORD64)hMod;
	MODULEINFO miModInfo;
	GetModuleInformation(GetCurrentProcess(), hMod, &miModInfo, sizeof(MODULEINFO));
	DWORD64 rangeEnd = rangeStart + miModInfo.SizeOfImage;
	for (DWORD64 pCur = rangeStart; pCur < rangeEnd; pCur++) {
		if (!*pat) return firstMatch;
		if (*(PBYTE)pat == '\?' || *(BYTE*)pCur == getByte(pat)) {
			if (!firstMatch) firstMatch = pCur;
			if (!pat[2]) return firstMatch;
			if (*(PWORD)pat == '\?\?' || *(PBYTE)pat != '\?') pat += 3;
			else pat += 2;
		}
		else {
			pat = pattern.c_str();
			firstMatch = 0;
		}
	}
	return NULL;
}

