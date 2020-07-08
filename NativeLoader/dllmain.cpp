#define _CRT_SECURE_NO_WARNINGS
#define GLEW_STATIC
#include <windows.h>
#include <tlhelp32.h>
#include <tchar.h>
#include <stdio.h>
#include <shlwapi.h>
#include <metahost.h>
#include <psapi.h>
#include <string>
#include "detours.h"

#include "GL\glew.h"
#pragma comment(lib, "glew32s.lib")
//#define IMGUI_IMPL_OPENGL_LOADER_GLEW

//#include <gl\GL.h>
#include "imgui/imgui.h"
#define IMGUI_DEFINE_MATH_OPERATORS
#include "imgui/imgui_internal.h"
#include "imgui/imgui_impl_opengl3.h"
#include "imgui/imgui_impl_win32.h"

//#pragma comment(lib, "glew32.lib")

#pragma comment(lib, "OpenGL32.lib")
#pragma comment(lib, "detours.lib")

//#pragma comment(lib, "mscoree.lib")

#define ARRAY_LEN(x) ((sizeof(x)/sizeof(0[x])) / ((size_t)(!(sizeof(x) % sizeof(0[x])))))

DWORD64 FindPattern(HMODULE hMod, std::string pattern);

typedef BOOL(__stdcall* f_wglSwapBuffers)(HDC hDc);
f_wglSwapBuffers owglSwapBuffers = NULL;

void SetupOrtho() {

	glPushAttrib(GL_ALL_ATTRIB_BITS);
	glPushMatrix();
	GLint viewport[4];
	glGetIntegerv(GL_VIEWPORT, viewport);
	glViewport(0, 0, viewport[2], viewport[3]);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	glOrtho(0, viewport[2], viewport[3], 0, -1, 1);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	glDisable(GL_DEPTH_TEST);
}

void RestoreGL() {
	glPopMatrix();
	glPopAttrib();
}

HWND hGameWindow;
WNDPROC hGameWindowProc;
HDC lastHdc;

extern IMGUI_IMPL_API LRESULT ImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);

LRESULT CALLBACK windowProc_hook(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam) {
	// Toggle the overlay using the delete key
   /* if (uMsg == WM_KEYDOWN && wParam == VK_DELETE) {
		menuShown = !menuShown;
		return false;
	}*/

	// If the overlay is shown, direct input to the overlay only
	if (true) {
		ImGui_ImplWin32_WndProcHandler(hWnd, uMsg, wParam, lParam);
		//return true;
	}

	// Otherwise call the game's wndProc function
	return CallWindowProc(hGameWindowProc, hWnd, uMsg, wParam, lParam);
}

typedef bool(__stdcall* f_GetDrawData)(char** test);
f_GetDrawData GetDrawData = nullptr;

BOOL __stdcall wglSwapBuffersHook(HDC hDc) {
	static bool once = true;
	if (once) {
		once = false;

		hGameWindow = WindowFromDC(hDc);
		hGameWindowProc = (WNDPROC)SetWindowLongPtr(hGameWindow,
			GWLP_WNDPROC, (LONG_PTR)windowProc_hook);

		glewInit();
		ImGui::CreateContext();
		ImGui_ImplWin32_Init(hGameWindow);


		ImGuiIO& io = ImGui::GetIO();

		io.DeltaTime = 1.0f / 60.0f;
		io.DisplaySize.x = 1920;
		io.DisplaySize.y = 1080;

		io.Fonts->AddFontDefault();
		/*unsigned char* pixels;
		int width, height, bytes_per_pixels;
		io.Fonts->GetTexDataAsRGBA32(&pixels, &width, &height, &bytes_per_pixels);*/

		ImGui_ImplOpenGL3_Init();
		ImGui::StyleColorsDark();
		//ImGui::GetStyle().AntiAliasedFill = false;
		//ImGui::GetStyle().AntiAliasedLines = false;
		//ImGui::GetStyle().WindowTitleAlign = ImVec2(0.5f, 0.5f);
		lastHdc = hDc;

		HMODULE hModuleDll = GetModuleHandle(TEXT("Sector_dll.dll"));
		//_tprintf(TEXT("PLS XD: %llX\n"), (DWORD64)hModuleDll);
		if (hModuleDll) {
			GetDrawData = (f_GetDrawData)GetProcAddress(hModuleDll, "GetDrawData");
			//_tprintf(TEXT("PLS2 XD: %llX\n"), (DWORD64)lol);
			//if (lol) {
				//WCHAR test[]{ L"WTFXDDD" };
				//lol(L"PLS");
			//}
		}
	}	

	ImGui_ImplOpenGL3_NewFrame();
	ImGui_ImplWin32_NewFrame();
	ImGui::NewFrame();

	//_tprintf(TEXT("x: %.2f %.2f\n"), io.MousePos.x, io.MousePos.y);

	//CHAR data[1024] =  { 0 };
	char* data = 0;
	GetDrawData(&data);
	if (data != 0) {
		if(ImGui::Begin("test")) {
			ImGui::Text(data);
		}
		ImGui::End();
		GlobalFree(data);
	}

	auto globalDrawlist = ImGui::GetBackgroundDrawList();
	globalDrawlist->AddRectFilled(ImVec2(50, 50), ImVec2(100, 100), 0x7700ff00);
	//ImGui::RenderFrame(, true);
	//ImGui::RenderFrame(ImVec2(100, 100), ImVec2(200, 200), 0xffff0000, false);
	//ImGui::SetNextWindowPos(ImVec2(20, 20));


	ImGui::ShowDemoWindow();

	ImGui::Render();
	////glViewport(0, 0, 500, 500);
	ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());

	//RestoreGL();

    return owglSwapBuffers(hDc);
}

void HookSwapBuffers(HMODULE hModule) {
	owglSwapBuffers = (f_wglSwapBuffers)FindPattern(hModule, "40 53 48 83 EC 30 48 8B D9 48 8D 54 24");

	_tprintf(TEXT("[nezu.cc] Hooking GameOverlayRenderer.wglSwapBuffers: %llX\n"), (DWORD64)owglSwapBuffers);
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach(&(PVOID&)owglSwapBuffers, wglSwapBuffersHook);
	DetourTransactionCommit();
}

typedef HMODULE(WINAPI* f_LoadLibraryExW)(LPCWSTR, HANDLE, DWORD);
f_LoadLibraryExW oLoadLibraryExW = LoadLibraryExW;

HMODULE WINAPI LoadLibraryExWHook(LPCWSTR lpLibFileName, HANDLE hFile, DWORD dwFlags) {
    if (wcsstr(lpLibFileName, L"gameoverlayrenderer64.dll")) {
        HMODULE hModule = oLoadLibraryExW(lpLibFileName, hFile, dwFlags);
		HookSwapBuffers(hModule);
        return hModule;
	}     
	//wprintf(L"LoadEx: %s\n", lpLibFileName);
    return oLoadLibraryExW(lpLibFileName, hFile, dwFlags);
}


BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
    if (ul_reason_for_call == DLL_PROCESS_ATTACH) {

        AllocConsole();
        freopen("CONOUT$", "w", stdout);
        freopen("CONIN$", "r", stdin);
		
		HMODULE hOverlay = GetModuleHandle(TEXT("gameoverlayrenderer64.dll"));
		if (hOverlay) {
			HookSwapBuffers(hOverlay);
			return TRUE;
		}

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

