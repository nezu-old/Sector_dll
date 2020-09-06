#include <windows.h>
#include "nt.h"
#include <tlhelp32.h>
#include <tchar.h>
#include <stdio.h>
#include <shlwapi.h>
#include <string>
#include <vector>
#include <fstream>
#include "detours.h"
#pragma comment(lib, "Shlwapi.lib")
#pragma comment(lib, "detours.lib")

#ifdef UNICODE
#define LoadLibraryName  "LoadLibraryW"
typedef std::wstring String;
#else
#define LoadLibraryName  "LoadLibraryA"
typedef std::string String;
#endif // !UNICODE

#define BUFSIZE 4096

#ifdef _DEBUG
#define WAIT_TIMEOUT_2 INFINITE
#else
#define WAIT_TIMEOUT_2 (1000 * 10)
#endif // !_DEBUG

void printError(const TCHAR* msg);
DWORD findProcess(const TCHAR* name);
void* FindModule(DWORD dwPID, const TCHAR* cName);
BOOL injectDll(HANDLE hProc, const TCHAR* dll, bool hide = false);

struct injectionData {
    void* pGetProcAddress;
    void* pModule;
    char entryName[16];
};

f_NtCreateUserProcess oNtCreateUserProcess = NULL;

NTSTATUS NTAPI NtCreateUserProcessHook(
    _Out_ PHANDLE ProcessHandle,
    _Out_ PHANDLE ThreadHandle,
    _In_ ACCESS_MASK ProcessDesiredAccess,
    _In_ ACCESS_MASK ThreadDesiredAccess,
    _In_opt_ POBJECT_ATTRIBUTES ProcessObjectAttributes,
    _In_opt_ POBJECT_ATTRIBUTES ThreadObjectAttributes,
    _In_ ULONG ProcessFlags, // PROCESS_CREATE_FLAGS_*
    _In_ ULONG ThreadFlags, // THREAD_CREATE_FLAGS_*
    _In_opt_ PVOID ProcessParameters, // PRTL_USER_PROCESS_PARAMETERS
    _Inout_ PPS_CREATE_INFO CreateInfo,
    _In_opt_ PPS_ATTRIBUTE_LIST AttributeList) {
    _tprintf(TEXT("[nezu.cc] Skipping debugger\n"));
    CreateInfo->InitState.IFEOSkipDebugger = 1;
    return oNtCreateUserProcess(ProcessHandle, ThreadHandle, ProcessDesiredAccess, ThreadDesiredAccess,
        ProcessObjectAttributes, ThreadObjectAttributes, ProcessFlags, ThreadFlags, ProcessParameters,
        CreateInfo, AttributeList);
}

typedef HRESULT(__stdcall* f_CLRCreateInstance)(REFCLSID clsid, REFIID riid, LPVOID* ppInterface);
struct ddd {
    GUID CLSID_CLRMetaHost_l;// = { 0x9280188d,0xe8e,0x4867,{0xb3, 0xc, 0x7f, 0xa8, 0x38, 0x84, 0xe8, 0xde} };
    GUID IID_ICLRMetaHost_l;// = { 0xD332DB9E,0xB9B3,0x4125,{0x82, 0x07, 0xA1, 0x48, 0x84, 0xF5, 0x32, 0x16} };
    GUID IID_ICLRRuntimeInfo_l;// = { 0xBD39D1D2, 0xBA2F, 0x486a, {0x89, 0xB0, 0xB4, 0xB0, 0xCB, 0x46, 0x68, 0x91} };
    GUID CLSID_CorRuntimeHost_l;// = { 0xcb2f6723, 0xab3a, 0x11d2, {0x9c, 0x40, 0x00, 0xc0, 0x4f, 0xa3, 0x0a, 0x3e} };
    GUID IID_ICorRuntimeHost_l;// = { 0xcb2f6722, 0xab3a, 0x11d2, {0x9c, 0x40, 0x00, 0xc0, 0x4f, 0xa3, 0x0a, 0x3e} };
    GUID IID_AppDomain_l;// = { 0x05f696dc, 0x2b29, 0x3663, {0xad, 0x8b, 0xc4, 0x38, 0x9c, 0xf2, 0xa7, 0x13} };
    wchar_t ver[20];// = L"v4.0.30319"
    wchar_t cla[64];// = L"Hack.Evil"
    wchar_t fun[64];// = L"Main"
    void* data;
    ULONG data_len;
    f_CLRCreateInstance CLRCreateInstance;
};

int main(int argc, char* argv[])
{
    //ShowWindow(GetConsoleWindow(), SW_HIDE);
    _tprintf(TEXT("[nezu.cc] Loader starting\n"));

    oNtCreateUserProcess = (f_NtCreateUserProcess)GetProcAddress(LoadLibrary(TEXT("ntdll.dll")), "NtCreateUserProcess");
    if (!oNtCreateUserProcess) {
        _tprintf(TEXT("[nezu.cc] ERROR: Failed to get address of NtCreateUserProcess\n"));
        return 10;
    }
    DetourTransactionBegin();
    DetourUpdateThread(GetCurrentThread());
    DetourAttach(&(PVOID&)oNtCreateUserProcess, NtCreateUserProcessHook);
    DetourTransactionCommit();

    HANDLE hProc;
    HANDLE hThread;

    if (argc == 2) {

        char* p = argv[1];
        PathRemoveFileSpecA(p); // just insers null terminator so buffer is same size;
        CHAR gameExe[BUFSIZE];
        strcpy_s(gameExe, p);
        strcat_s(gameExe, "\\sectorsedge.exe");

        printf("CMD: %s\n", gameExe);

        PROCESS_INFORMATION pi = { 0 };
        STARTUPINFOA si = { 0 };
        si.cb = sizeof(si);
        if (!CreateProcessA(NULL, gameExe, NULL, NULL, FALSE, CREATE_SUSPENDED, NULL, NULL, &si, &pi)) {
            printError(TEXT("CreateProcessA"));
            return 11;
        }
        hProc = pi.hProcess;
        hThread = pi.hThread;

    } else {
        exit(0);
        return 0;
    }

//#ifdef DEBUG

    //while (!::IsDebuggerPresent())
    //    ::Sleep(100); // to avoid 100% CPU load
    //DebugBreak();

//#endif // DEBUG

    if (injectDll(hProc, TEXT("MSCOREE.DLL"))) {
        _tprintf(TEXT("[nezu.cc] Injected\n"));
    } else {
        TerminateProcess(hProc, 1);
        CloseHandle(hProc);
        MessageBox(NULL, TEXT("Failed to fully inject, closing process to prevent detection"), TEXT("Fuck"), MB_ICONERROR | MB_OK);
        return 0;
    }

    unsigned char shellcode[] = {
        0x48, 0xB9, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, //mov rcx, data_array           ///ofs 2 dec
        0x48, 0x83, 0xEC, 0x30, //sub rsp, 0x30

        //shellcode: void inject(bbb*)
        0x40, 0x55, 0x53, 0x57, 0x48, 0x8D, 0x6C, 0x24, 0xB9, 0x48, 0x81, 0xEC,
        0xD0, 0x00, 0x00, 0x00, 0x33, 0xFF, 0x48, 0x8D, 0x51, 0x10, 0x4C, 0x8D,
        0x45, 0x6F, 0x48, 0x89, 0x7D, 0x6F, 0x48, 0x89, 0x7D, 0x77, 0x48, 0x8B,
        0xD9, 0x48, 0x89, 0x7D, 0x67, 0x48, 0x89, 0x7D, 0x7F, 0x48, 0x89, 0x7D,
        0xB7, 0x48, 0x89, 0x7D, 0xBF, 0x48, 0x89, 0x7D, 0xC7, 0xFF, 0x91, 0x98,
        0x01, 0x00, 0x00, 0x48, 0x8B, 0x4D, 0x6F, 0x4C, 0x8D, 0x43, 0x20, 0x48,
        0x8D, 0x53, 0x60, 0x4C, 0x8D, 0x4D, 0x77, 0x48, 0x8B, 0x01, 0xFF, 0x50,
        0x18, 0x48, 0x8B, 0x4D, 0x77, 0x4C, 0x8D, 0x43, 0x40, 0x48, 0x8D, 0x53,
        0x30, 0x4C, 0x8D, 0x4D, 0x67, 0x48, 0x8B, 0x01, 0xFF, 0x50, 0x48, 0x48,
        0x8B, 0x4D, 0x67, 0x48, 0x8B, 0x01, 0xFF, 0x50, 0x50, 0x48, 0x8B, 0x4D,
        0x67, 0x48, 0x8D, 0x55, 0x7F, 0x48, 0x8B, 0x01, 0xFF, 0x50, 0x68, 0x48,
        0x8B, 0x4D, 0x7F, 0x48, 0x8D, 0x53, 0x50, 0x4C, 0x8D, 0x45, 0xB7, 0x48,
        0x8B, 0x01, 0xFF, 0x10, 0x48, 0x8B, 0x83, 0x88, 0x01, 0x00, 0x00, 0x4C,
        0x8D, 0x45, 0xBF, 0x48, 0x8B, 0x4D, 0xB7, 0x48, 0x8D, 0x55, 0xCF, 0x48,
        0x89, 0x45, 0xDF, 0x8B, 0x83, 0x90, 0x01, 0x00, 0x00, 0x89, 0x45, 0xE7,
        0x48, 0xC7, 0x45, 0xD3, 0x01, 0x00, 0x00, 0x00, 0xC7, 0x45, 0xCF, 0x01,
        0x00, 0x00, 0x00, 0x89, 0x7D, 0xEB, 0x48, 0x8B, 0x01, 0xFF, 0x90, 0x68,
        0x01, 0x00, 0x00, 0x48, 0x8B, 0x4D, 0xBF, 0x48, 0x8D, 0x93, 0x8C, 0x00,
        0x00, 0x00, 0x4C, 0x8D, 0x45, 0xC7, 0x48, 0x8B, 0x01, 0xFF, 0x90, 0x88,
        0x00, 0x00, 0x00, 0x48, 0x8B, 0x4D, 0xC7, 0x4C, 0x8D, 0x45, 0xEF, 0x4C,
        0x89, 0x44, 0x24, 0x30, 0x48, 0x8D, 0x93, 0x0C, 0x01, 0x00, 0x00, 0x33,
        0xC0, 0x48, 0x89, 0x7C, 0x24, 0x28, 0x0F, 0x57, 0xC0, 0x48, 0x89, 0x45,
        0x37, 0x0F, 0x11, 0x45, 0xEF, 0x4C, 0x8D, 0x45, 0x07, 0x48, 0x89, 0x45,
        0xFF, 0xF2, 0x0F, 0x10, 0x45, 0x37, 0x0F, 0x57, 0xC9, 0x48, 0x8B, 0x01,
        0x45, 0x33, 0xC9, 0x4C, 0x89, 0x44, 0x24, 0x20, 0x41, 0xB8, 0x18, 0x01,
        0x00, 0x00, 0x0F, 0x29, 0x4D, 0x07, 0xF2, 0x0F, 0x11, 0x45, 0x17, 0xFF,
        0x90, 0xC8, 0x01, 0x00, 0x00, 0x48, 0x81, 0xC4, 0xD0, 0x00, 0x00, 0x00,
        0x5F, 0x5B, 0x5D,

        0x48, 0x83, 0xC4, 0x30, //add rsp, 0x30

        0xC3, //ret
    };

    TCHAR path[BUFSIZE]{0};
    GetModuleFileName(NULL, path, BUFSIZE);
    PathRemoveFileSpec(path); // just insers null terminator so buffer is same size;
    _tcscat_s(path, TEXT("\\EACEmulator.dll"));

    std::ifstream infile(String(path), std::ios_base::binary);
    infile.seekg(0, std::ios_base::end);
    size_t length = infile.tellg();
    infile.seekg(0, std::ios_base::beg);

    std::vector<char> buffer;
    buffer.reserve(length);
    std::copy(std::istreambuf_iterator<char>(infile),
        std::istreambuf_iterator<char>(),
        std::back_inserter(buffer));

    void * pMem = VirtualAllocEx(hProc, 0, sizeof(ddd) + sizeof(shellcode) + buffer.size() + 0x100, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

    if (!pMem) {
        printError(TEXT("VirtualAllocEx [2]"));
        return FALSE;
    }

    ddd dd;
    ddd* d = &dd;

    d->CLSID_CLRMetaHost_l = { 0x9280188d,0xe8e,0x4867,{0xb3, 0xc, 0x7f, 0xa8, 0x38, 0x84, 0xe8, 0xde} };
    d->IID_ICLRMetaHost_l = { 0xD332DB9E,0xB9B3,0x4125,{0x82, 0x07, 0xA1, 0x48, 0x84, 0xF5, 0x32, 0x16} };
    d->IID_ICLRRuntimeInfo_l = { 0xBD39D1D2, 0xBA2F, 0x486a, {0x89, 0xB0, 0xB4, 0xB0, 0xCB, 0x46, 0x68, 0x91} };
    d->CLSID_CorRuntimeHost_l = { 0xcb2f6723, 0xab3a, 0x11d2, {0x9c, 0x40, 0x00, 0xc0, 0x4f, 0xa3, 0x0a, 0x3e} };
    d->IID_ICorRuntimeHost_l = { 0xcb2f6722, 0xab3a, 0x11d2, {0x9c, 0x40, 0x00, 0xc0, 0x4f, 0xa3, 0x0a, 0x3e} };
    d->IID_AppDomain_l = { 0x05f696dc, 0x2b29, 0x3663, {0xad, 0x8b, 0xc4, 0x38, 0x9c, 0xf2, 0xa7, 0x13} };
    const wchar_t ver[] = L"v4.0.30319";
    memcpy_s(d->ver, sizeof(d->ver), ver, sizeof(ver));
    const wchar_t cla[] = L"EACEmulator.EACEmulator";
    memcpy_s((reinterpret_cast<BYTE*>(&d->cla) + 4), sizeof(d->cla) - 4, cla, sizeof(cla));
    *reinterpret_cast<DWORD*>(&d->cla) = sizeof(cla) - 2;
    const wchar_t fun[] = L"Main";
    memcpy_s((reinterpret_cast<BYTE*>(&d->fun) + 4), sizeof(d->fun) - 4, fun, sizeof(fun));
    *reinterpret_cast<DWORD*>(&d->fun) = sizeof(fun) - 2;
    d->data = pMem;
    d->data_len = buffer.size();

    HMODULE hMod = LoadLibrary(TEXT("mscoree.dll"));
    if (!hMod) return 1;
    d->CLRCreateInstance = (f_CLRCreateInstance)GetProcAddress(hMod, "CLRCreateInstance");

    WriteProcessMemory(hProc, pMem, buffer.data(), buffer.size(), 0);
    WriteProcessMemory(hProc, reinterpret_cast<void*>(reinterpret_cast<CHAR*>(pMem) + buffer.size()), d, sizeof(*d), 0);
    *reinterpret_cast<void**>(reinterpret_cast<CHAR*>(shellcode) + 2) = reinterpret_cast<void*>(reinterpret_cast<CHAR*>(pMem) + buffer.size());
    WriteProcessMemory(hProc, reinterpret_cast<void*>(reinterpret_cast<CHAR*>(pMem) + buffer.size() + sizeof(*d)), shellcode, sizeof(shellcode), 0);

    CreateRemoteThread(hProc, 0, 0, reinterpret_cast<LPTHREAD_START_ROUTINE>(reinterpret_cast<CHAR*>(pMem) + buffer.size() + sizeof(*d)), 0, 0, 0);

    CloseHandle(hProc);
    //ResumeThread(hThread);

    return 0;
}


BOOL injectDll(HANDLE hProc, const TCHAR* dll, bool hide) {

    HMODULE hKernel32 = LoadLibrary(TEXT("Kernel32.dll"));

    if (!hKernel32) {
        printError(TEXT("LoadLibrary(Kernel32.dll)"));
        return FALSE;
    }

    FARPROC pLoadLibrary = GetProcAddress(hKernel32, LoadLibraryName);

    if (!pLoadLibrary) {
        printError(TEXT("GetProcAddress(" LoadLibraryName ")"));
        return FALSE;
    }

    SIZE_T sWriteSize = (_tcslen(dll) + 1) * sizeof(TCHAR);

    LPVOID pMem = VirtualAllocEx(hProc, NULL, sWriteSize, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);

    if (!pMem) {
        printError(TEXT("VirtualAllocEx [1]"));
        return FALSE;
    }

    if (!WriteProcessMemory(hProc, pMem, dll, sWriteSize, NULL)) {
        VirtualFreeEx(hProc, pMem, 0, MEM_RELEASE);
        printError(TEXT("WriteProcessMemory [1]"));
        return FALSE;
    }

    HANDLE hThread = CreateRemoteThreadEx(hProc, NULL, 0, (LPTHREAD_START_ROUTINE)pLoadLibrary, pMem, 0, 0, 0);

    if (!hThread) {
        VirtualFreeEx(hProc, pMem, 0, MEM_RELEASE);
        printError(TEXT("CreateRemoteThreadEx [1]"));
        return FALSE;
    }

    DWORD dWaitResault = WaitForSingleObject(hThread, WAIT_TIMEOUT_2);

    if (dWaitResault == WAIT_FAILED) {
        CloseHandle(hThread);
        VirtualFreeEx(hProc, pMem, 0, MEM_RELEASE);
        printError(TEXT("WaitForSingleObject [1]"));
        return FALSE;
    } else if (dWaitResault == WAIT_TIMEOUT_2) {
        CloseHandle(hThread);
        VirtualFreeEx(hProc, pMem, 0, MEM_RELEASE);
        _tprintf(TEXT("[nezu.cc] ERROR: Dll injection timeout [1]\n"));
        return FALSE;
    }

    if (!VirtualFreeEx(hProc, pMem, 0, MEM_RELEASE)) {
        printError(TEXT("VirtualFreeEx [1]"));
        //return FALSE; // no big deal, continue injection
    }
    pMem = NULL;

    DWORD dRet;
    if (!GetExitCodeThread(hThread, &dRet)) {
        CloseHandle(hThread);
        printError(TEXT("GetExitCodeThread [1]"));
        return FALSE;
    }

    CloseHandle(hThread);

    if (!dRet) {
        _tprintf(TEXT("[nezu.cc] ERROR: injection failed\n"));
        return FALSE;
    }

    //if (hide) {
    //    void* pModule = nullptr;
    //    for (size_t i = 0; i < 10; i++) {
    //        pModule = FindModule(GetProcessId(hProc), PathFindFileName(cFullDllPath));
    //        if (pModule)
    //            break;
    //        Sleep(100);
    //    }

    //    if (!pModule) {
    //        //err printed in FindModule
    //        return FALSE;
    //    }

    //    HMODULE hNtdll = LoadLibrary(TEXT("Ntdll.dll"));
    //    if (!hNtdll) {
    //        printError(TEXT("LoadLibrary(Ntdll.dll)"));
    //        return FALSE;
    //    }

    //    f_NtQueryInformationProcess NtQueryInformationProcess = (f_NtQueryInformationProcess)GetProcAddress(hNtdll, "NtQueryInformationProcess");

    //    if (!NtQueryInformationProcess) {
    //        printError(TEXT("GetProcAddress(NtQueryInformationProcess)"));
    //        return FALSE;
    //    }

    //    PROCESS_BASIC_INFORMATION PBI = { 0 };
    //    ULONG uReturnLength = 0;

    //    if (NT_FAIL(NtQueryInformationProcess(hProc, ProcessBasicInformation, &PBI, sizeof(PBI), &uReturnLength))) {
    //        printError(TEXT("NtQueryInformationProcess(ProcessBasicInformation)"));
    //        return FALSE;
    //    }

    //    PEB	peb{ 0 };
    //    if (!ReadProcessMemory(hProc, PBI.pPEB, &peb, sizeof(PEB), NULL)) {
    //        printError(TEXT("ReadProcessMemory(PEB)"));
    //        return FALSE;
    //    }

    //    PEB_LDR_DATA ldrdata{ 0 };
    //    if (!ReadProcessMemory(hProc, peb.Ldr, &ldrdata, sizeof(PEB_LDR_DATA), nullptr)) {
    //        printError(TEXT("ReadProcessMemory(PEB_LDR_DATA)"));
    //        return FALSE;
    //    }

    //    LIST_ENTRY* pCurrentEntry = ldrdata.InLoadOrderModuleListHead.Flink;
    //    LIST_ENTRY* pLastEntry = ldrdata.InLoadOrderModuleListHead.Blink;
    //    LDR_DATA_TABLE_ENTRY* pLdrEntry{ 0 };

    //    while (true) {
    //        LDR_DATA_TABLE_ENTRY CurrentEntry{ 0 };
    //        ReadProcessMemory(hProc, pCurrentEntry, &CurrentEntry, sizeof(LDR_DATA_TABLE_ENTRY), nullptr);

    //        if (CurrentEntry.DllBase == pModule) {
    //            pLdrEntry = new LDR_DATA_TABLE_ENTRY();
    //            memcpy_s(pLdrEntry, sizeof(LDR_DATA_TABLE_ENTRY), &CurrentEntry, sizeof(CurrentEntry));
    //            break;
    //        }

    //        if (pCurrentEntry == pLastEntry)
    //            break;

    //        pCurrentEntry = CurrentEntry.InLoadOrder.Flink;
    //    }

    //    if (!pLdrEntry) {
    //        _tprintf(TEXT("[nezu.cc] ERROR: Failed to find LDR_DATA_TABLE_ENTRY for injected module\n"));
    //        return FALSE;
    //    }

    //    auto Unlink = [=](LIST_ENTRY entry) {
    //        LIST_ENTRY list;
    //        ReadProcessMemory(hProc, entry.Flink, &list, sizeof(LIST_ENTRY), nullptr);
    //        list.Blink = entry.Blink;
    //        WriteProcessMemory(hProc, entry.Flink, &list, sizeof(LIST_ENTRY), nullptr);

    //        ReadProcessMemory(hProc, entry.Blink, &list, sizeof(LIST_ENTRY), nullptr);
    //        list.Flink = entry.Flink;
    //        WriteProcessMemory(hProc, entry.Blink, &list, sizeof(LIST_ENTRY), nullptr);
    //    };

    //    Unlink(pLdrEntry->InInitOrder);
    //    Unlink(pLdrEntry->InLoadOrder);
    //    Unlink(pLdrEntry->InMemoryOrder);

    //    delete pLdrEntry;
    //}

    return TRUE;
}

void* FindModule(DWORD dwPID, const TCHAR* cName) {
    HANDLE hModuleSnap = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, dwPID);
    if (hModuleSnap == INVALID_HANDLE_VALUE) {
        printError(TEXT("CreateToolhelp32Snapshot (of modules)"));
        return(FALSE);
    }

    MODULEENTRY32 me32;
    me32.dwSize = sizeof(MODULEENTRY32);

    if (!Module32First(hModuleSnap, &me32)) {
        printError(TEXT("Module32First"));
        CloseHandle(hModuleSnap);
        return(FALSE);
    }

    do {

        if (!_tcscmp(me32.szModule, cName)) {
            CloseHandle(hModuleSnap);
            return (void*)me32.modBaseAddr;
        }

    } while (Module32Next(hModuleSnap, &me32));

    _tprintf(TEXT("[nezu.cc] failed to find module %s\n"), cName);

    CloseHandle(hModuleSnap);
    return NULL;
}

DWORD findProcess(const TCHAR* cName) {
    HANDLE hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (hProcessSnap == INVALID_HANDLE_VALUE) {
        printError(TEXT("CreateToolhelp32Snapshot"));
        return NULL;
    }

    PROCESSENTRY32 pe32;
    pe32.dwSize = sizeof(PROCESSENTRY32);

    if (!Process32First(hProcessSnap, &pe32)) {
        printError(TEXT("Process32First"));
        CloseHandle(hProcessSnap);
        return NULL;
    }

    do {

        if (!_tcscmp(pe32.szExeFile, cName)) {
            _tprintf(TEXT("[nezu.cc] process %s found with PID: %d\n"), cName, pe32.th32ProcessID);
            CloseHandle(hProcessSnap);
            return pe32.th32ProcessID;
        }

    } while (Process32Next(hProcessSnap, &pe32));

    _tprintf(TEXT("[nezu.cc] failed to find process %s\n"), cName);

    CloseHandle(hProcessSnap);
    return NULL;
}

void printError(const TCHAR* msg) {
    DWORD eNum;
    TCHAR sysMsg[256];
    TCHAR* p;

    eNum = GetLastError();
    FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
        NULL, eNum,
        MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), // Default language
        sysMsg, 256, NULL);

    // Trim the end of the line and terminate it with a null
    p = sysMsg;
    while ((*p > 31) || (*p == 9))
        ++p;
    do { *p-- = 0; } while ((p >= sysMsg) &&
        ((*p == '.') || (*p < 33)));

    // Display the message
    _tprintf(TEXT("\n[nezu.cc] ERROR: %s failed with error %d (%s)\n"), msg, eNum, sysMsg);
}