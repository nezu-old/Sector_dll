#include "utils.h"
#include <psapi.h>

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

//void SetupOrtho() {
//
//	glPushAttrib(GL_ALL_ATTRIB_BITS);
//	glPushMatrix();
//	GLint viewport[4];
//	glGetIntegerv(GL_VIEWPORT, viewport);
//	glViewport(0, 0, viewport[2], viewport[3]);
//	glMatrixMode(GL_PROJECTION);
//	glLoadIdentity();
//	glOrtho(0, viewport[2], viewport[3], 0, -1, 1);
//	glMatrixMode(GL_MODELVIEW);
//	glLoadIdentity();
//	glDisable(GL_DEPTH_TEST);
//}
//
//void RestoreGL() {
//	glPopMatrix();
//	glPopAttrib();
//}
//
