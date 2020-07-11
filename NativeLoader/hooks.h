#pragma once

#include <windows.h>

typedef BOOL(WINAPI* f_wglSwapBuffers)(HDC hDc);
typedef HMODULE(WINAPI* f_LoadLibraryExW)(LPCWSTR, HANDLE, DWORD);
typedef BOOL(WINAPI* f_SetCursorPos)(int x, int y);
typedef BOOL(WINAPI* f_GetCursorPos)(LPPOINT pos);
typedef UINT(WINAPI* f_GetRawInputData)(HRAWINPUT hRawInput, UINT uiCommand, LPVOID pData, PUINT pcbSize, UINT cbSizeHeader);

namespace H {

	extern f_wglSwapBuffers owglSwapBuffers;
	extern f_LoadLibraryExW oLoadLibraryExW;
	extern WNDPROC oWndProc;
	extern f_SetCursorPos oSetCursorPos;
	extern f_GetCursorPos oGetCursorPos;
	extern f_GetRawInputData oGetRawInputData;

	LRESULT WndProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam);
	HMODULE WINAPI LoadLibraryExW(LPCWSTR lpLibFileName, HANDLE hFile, DWORD dwFlags);
	BOOL WINAPI wglSwapBuffers(HDC hDc);
	BOOL WINAPI SetCursorPos(int x, int y);
	BOOL WINAPI GetCursorPos(LPPOINT point);
	UINT WINAPI GetRawInputData(HRAWINPUT hRawInput, UINT uiCommand, LPVOID pData, PUINT pcbSize, UINT cbSizeHeader);

	void HookLoadLibraryExW();
	void HookSwapBuffers(HMODULE hModSteamOverlay);
	void HookWindow(HWND hWindow);

}
