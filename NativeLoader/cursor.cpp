#include "hooks.h"
#include "globals.h"
#include "menu.h"

POINT lastSetCursor;

f_SetCursorPos H::oSetCursorPos;
BOOL WINAPI H::SetCursorPos(int x, int y) {
	lastSetCursor.x = x;
	lastSetCursor.y = y;
	if (Menu::open) 
		return TRUE;
	return oSetCursorPos(x, y);
}

f_GetCursorPos H::oGetCursorPos;
BOOL WINAPI H::GetCursorPos(LPPOINT point) {
	static bool firstGetAfterMenuClose = false;
	if (Menu::open) {
		firstGetAfterMenuClose = true;
		if (!G::bIsImguiRunning) {
			point->x = lastSetCursor.x;
			point->y = lastSetCursor.y;
			return TRUE;
		}
	} else if (firstGetAfterMenuClose) {
		firstGetAfterMenuClose = false;
		oSetCursorPos(lastSetCursor.x, lastSetCursor.y);
	}
	return oGetCursorPos(point);
}

f_GetRawInputData H::oGetRawInputData;
UINT WINAPI H::GetRawInputData(HRAWINPUT hRawInput, UINT uiCommand, LPVOID pData, PUINT pcbSize, UINT cbSizeHeader) {
	UINT ret = oGetRawInputData(hRawInput, uiCommand, pData, pcbSize, cbSizeHeader);
	if (Menu::open && uiCommand == RID_INPUT) {
		RAWINPUT* rawData = (RAWINPUT*)pData;
		rawData->header.dwType = -1; // game will ignore if it's not RIM_TYPEMOUSE
	}
	return ret;
}
