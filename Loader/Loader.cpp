#include <windows.h>
#include "nt.h"
#include <tlhelp32.h>
#include <tchar.h>
#include <stdio.h>
#include <shlwapi.h>
#include <string>
#include "detours.h"
#pragma comment(lib, "Shlwapi.lib")
#pragma comment(lib, "detours.lib")

#ifdef UNICODE
#define LoadLibraryName  "LoadLibraryW"
#else
#define LoadLibraryName  "LoadLibraryA"
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
BOOL injectDll(HANDLE hProc, const TCHAR* dll, const char* entryName = NULL);

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

    if (argc > 1) {

        std::string command;
        for (int i = 1; i < argc; i++) {
            command += argv[i];

            if (i != argc - 1)
                command += " ";
        }
        printf("CMD: %s\n", command.c_str());

        PROCESS_INFORMATION pi = { 0 };
        STARTUPINFOA si = { 0 };
        si.cb = sizeof(si);
        if (!CreateProcessA(NULL, (char*)command.c_str(), NULL, NULL, FALSE, CREATE_SUSPENDED, NULL, NULL, &si, &pi)) {
            printError(TEXT("CreateProcessA"));
            return 11;
        }
        hProc = pi.hProcess;
        hThread = pi.hThread;
    } else {
        ShellExecute(0, 0, TEXT("steam://rungameid/1024890"), 0, 0, SW_SHOW);
        exit(0);
        return 0;
    }
    //DWORD dwPid = findProcess(TEXT("sectorsedge.exe"));
    //
    //if (!dwPid) {
    //    _tprintf(TEXT("[nezu.cc] Game not found, launching game\n"));
    //    ShellExecute(0, 0, TEXT("steam://rungameid/1024890"), 0, 0, SW_SHOW);
    //    //ShellExecute(0, 0, TEXT("C:\\Program Files (x86)\\Steam\\steamapps\\common\\SectorsEdge\\sectorsedge.exe"), 0, 0, SW_SHOW);
    //    do {
    //        dwPid = findProcess(TEXT("sectorsedge.exe"));
    //        Sleep(10);
    //    } while (!dwPid);
    //}

    //HANDLE hProc = OpenProcess(PROCESS_ALL_ACCESS, FALSE, dwPid);

    //if (!hProc) {
    //    printError(TEXT("OpenProcess"));
    //    return 1;
    //}

    TCHAR path[BUFSIZE];
    TCHAR managedPath[BUFSIZE];

    DWORD length = GetModuleFileName(NULL, path, BUFSIZE);
    PathRemoveFileSpec(path); // just insers null terminator so buffer is same size;
    _tcscpy_s(managedPath, path);

    _tcscat_s(path, TEXT("\\NativeLoader.dll"));
    _tcscat_s(managedPath, TEXT("\\Sector_dll.dll"));
    

    if (injectDll(hProc, path) && injectDll(hProc, managedPath, "Entry")) {
        _tprintf(TEXT("[nezu.cc] Injected\n"));
        CloseHandle(hProc);
    } else {
        TerminateProcess(hProc, 1);
        CloseHandle(hProc);
        MessageBox(NULL, TEXT("Failed to fully inject, closing process to prevent detection"), TEXT("Fuck"), MB_ICONERROR | MB_OK);
    }
    //getchar();
    //Sleep(1000);


    //ResumeThread(hThread);



    //Sleep(10000);
    return 0;
}


BOOL injectDll(HANDLE hProc, const TCHAR* dll, const char * entryName) {

    //__declspec(noinline) void square(dataa* d) {
    //    typedef FARPROC(*f_gpa)(void*, char*);
    //    typedef void(*f_entry)(void);
    //    FARPROC proc = ((f_gpa)d->pGetProcAddress)(d->pModule, d->entryName);
    //    ((f_entry)proc)();
    //}

    BYTE bShellcode[] = {
        0x48, 0x83, 0xEC, 0x28,  // sub  rsp, 28h
        0x48, 0x8B, 0xC1,        // mov  rax, rcx
        0x48, 0x8D, 0x51, 0x10,  // lea  rdx,[rcx + 16]
        0x48, 0x8B, 0x49, 0x08,  // mov  rcx,[rcx + 8]
        0xFF, 0x10,              // call qword ptr[rax]
        0x48, 0x83, 0xC4, 0x28,  // add  rsp, 28h
        0x48, 0xFF, 0xE0,        // jmp  rax
    };

    TCHAR cFullDllPath[BUFSIZE] = TEXT("");

    DWORD dPathLen = GetFullPathName(dll, BUFSIZE, cFullDllPath, NULL);

    if (!dPathLen) {
        printError(TEXT("GetFullPathName"));
        return FALSE;
    }

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

    SIZE_T sWriteSize = (dPathLen + 1) * sizeof(TCHAR);

    LPVOID pMem = VirtualAllocEx(hProc, NULL, sWriteSize, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);

    if (!pMem) {
        printError(TEXT("VirtualAllocEx [1]"));
        return FALSE;
    }

    if (!WriteProcessMemory(hProc, pMem, cFullDllPath, sWriteSize, NULL)) {
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

    //Sleep(200);

    void* pModule = nullptr;
    for (size_t i = 0; i < 10; i++) {
        pModule = FindModule(GetProcessId(hProc), PathFindFileName(cFullDllPath));
        if (pModule) 
            break;
        Sleep(100);
    }

    if (!pModule) {
        //err printed in FindModule
        return FALSE;
    }

    HMODULE hNtdll = LoadLibrary(TEXT("Ntdll.dll"));
    if (!hNtdll) {
        printError(TEXT("LoadLibrary(Ntdll.dll)"));
        return FALSE;
    }

    f_NtQueryInformationProcess NtQueryInformationProcess = (f_NtQueryInformationProcess)GetProcAddress(hNtdll, "NtQueryInformationProcess");

    if (!NtQueryInformationProcess) {
        printError(TEXT("GetProcAddress(NtQueryInformationProcess)"));
        return FALSE;
    }

    PROCESS_BASIC_INFORMATION PBI = { 0 };
    ULONG uReturnLength = 0;

    if (NT_FAIL(NtQueryInformationProcess(hProc, ProcessBasicInformation, &PBI, sizeof(PBI), &uReturnLength))) {
        printError(TEXT("NtQueryInformationProcess(ProcessBasicInformation)"));
        return FALSE;
    }

    PEB	peb{ 0 };
    if (!ReadProcessMemory(hProc, PBI.pPEB, &peb, sizeof(PEB), NULL)) {
        printError(TEXT("ReadProcessMemory(PEB)"));
        return FALSE;
    }

    PEB_LDR_DATA ldrdata{ 0 };
    if (!ReadProcessMemory(hProc, peb.Ldr, &ldrdata, sizeof(PEB_LDR_DATA), nullptr)) {
        printError(TEXT("ReadProcessMemory(PEB_LDR_DATA)"));
        return FALSE;
    }

    LIST_ENTRY* pCurrentEntry = ldrdata.InLoadOrderModuleListHead.Flink;
    LIST_ENTRY* pLastEntry = ldrdata.InLoadOrderModuleListHead.Blink;
    LDR_DATA_TABLE_ENTRY* pLdrEntry{ 0 };

    while (true) {
        LDR_DATA_TABLE_ENTRY CurrentEntry{ 0 };
        ReadProcessMemory(hProc, pCurrentEntry, &CurrentEntry, sizeof(LDR_DATA_TABLE_ENTRY), nullptr);

        if (CurrentEntry.DllBase == pModule) {
            pLdrEntry = new LDR_DATA_TABLE_ENTRY();
            memcpy_s(pLdrEntry, sizeof(LDR_DATA_TABLE_ENTRY), &CurrentEntry, sizeof(CurrentEntry));
            break;
        }

        if (pCurrentEntry == pLastEntry)
            break;

        pCurrentEntry = CurrentEntry.InLoadOrder.Flink;
    }

    if (!pLdrEntry) {
        _tprintf(TEXT("[nezu.cc] ERROR: Failed to find LDR_DATA_TABLE_ENTRY for injected module\n"));
        return FALSE;
    }

    auto Unlink = [=](LIST_ENTRY entry) {
        LIST_ENTRY list;
        ReadProcessMemory(hProc, entry.Flink, &list, sizeof(LIST_ENTRY), nullptr);
        list.Blink = entry.Blink;
        WriteProcessMemory(hProc, entry.Flink, &list, sizeof(LIST_ENTRY), nullptr);

        ReadProcessMemory(hProc, entry.Blink, &list, sizeof(LIST_ENTRY), nullptr);
        list.Flink = entry.Flink;
        WriteProcessMemory(hProc, entry.Blink, &list, sizeof(LIST_ENTRY), nullptr);
    };

    Unlink(pLdrEntry->InInitOrder);
    Unlink(pLdrEntry->InLoadOrder);
    Unlink(pLdrEntry->InMemoryOrder);

    delete pLdrEntry;

    if (!entryName) {
        return TRUE;
    }

    FARPROC pGetProcAddress = GetProcAddress(hKernel32, "GetProcAddress");

    if (!pGetProcAddress) {
        printError(TEXT("GetProcAddress(GetProcAddress)"));
        return FALSE;
    }

#define CODECAVE_SIZE (sizeof(bShellcode) + sizeof(injectionData))

    pMem = VirtualAllocEx(hProc, NULL, CODECAVE_SIZE, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

    if (!pMem) {
        printError(TEXT("VirtualAllocEx [2]"));
        return FALSE;
    }

    injectionData injectionData;
    injectionData.pGetProcAddress = pGetProcAddress;
    injectionData.pModule = pModule;
    strcpy_s(injectionData.entryName, entryName);

    BYTE bCodeCave[CODECAVE_SIZE];
    memcpy(bCodeCave, bShellcode, sizeof(bShellcode));
    memcpy(bCodeCave + sizeof(bShellcode), &injectionData, sizeof(injectionData));

    if (!WriteProcessMemory(hProc, pMem, bCodeCave, sizeof(bCodeCave), NULL)) {
        VirtualFreeEx(hProc, pMem, 0, MEM_RELEASE);
        printError(TEXT("WriteProcessMemory [2]"));
        return FALSE;
    }

    HANDLE hThread2 = CreateRemoteThreadEx(hProc, NULL, 0, (LPTHREAD_START_ROUTINE)pMem, (void*)((DWORD64)pMem + sizeof(bShellcode)), 0, 0, 0);

    if (!hThread2) {
        VirtualFreeEx(hProc, pMem, 0, MEM_RELEASE);
        printError(TEXT("CreateRemoteThreadEx [2]"));
        return FALSE;
    }

    //DWORD dWaitResault2 = WaitForSingleObject(hThread2, WAIT_TIMEOUT_2);

    //if (dWaitResault2 == WAIT_FAILED) {
    //    CloseHandle(hThread2);
    //    VirtualFreeEx(hProc, pMem, 0, MEM_RELEASE);
    //    printError(TEXT("WaitForSingleObject [2]"));
    //    return FALSE;
    //} else if (dWaitResault2 == WAIT_TIMEOUT_2) {
    //    CloseHandle(hThread2);
    //    VirtualFreeEx(hProc, pMem, 0, MEM_RELEASE);
    //    _tprintf(TEXT("[nezu.cc] ERROR: Dll injection timeout [2]\n"));
    //    return FALSE;
    //}

    //CloseHandle(hThread2);

    //if (!VirtualFreeEx(hProc, pMem, 0, MEM_RELEASE)) {
    //    printError(TEXT("VirtualFreeEx [2]"));
    //    //return FALSE; // no big deal, continue
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