#include "hlapi/hlapi.h"
#include <chrono>
#include <thread>
#include <fstream>
#include <vector>

#pragma pack(1)
typedef struct _GUID {
    unsigned int  Data1;
    unsigned short Data2;
    unsigned short Data3;
    unsigned char  Data4[ 8 ];
} GUID;

#pragma pack(8)
struct ddd {
    GUID CLSID_CLRMetaHost_l;// = { 0x9280188d,0xe8e,0x4867,{0xb3, 0xc, 0x7f, 0xa8, 0x38, 0x84, 0xe8, 0xde} };
    GUID IID_ICLRMetaHost_l;// = { 0xD332DB9E,0xB9B3,0x4125,{0x82, 0x07, 0xA1, 0x48, 0x84, 0xF5, 0x32, 0x16} };
    GUID IID_ICLRRuntimeInfo_l;// = { 0xBD39D1D2, 0xBA2F, 0x486a, {0x89, 0xB0, 0xB4, 0xB0, 0xCB, 0x46, 0x68, 0x91} };
    GUID CLSID_CorRuntimeHost_l;// = { 0xcb2f6723, 0xab3a, 0x11d2, {0x9c, 0x40, 0x00, 0xc0, 0x4f, 0xa3, 0x0a, 0x3e} };
    GUID IID_ICorRuntimeHost_l;// = { 0xcb2f6722, 0xab3a, 0x11d2, {0x9c, 0x40, 0x00, 0xc0, 0x4f, 0xa3, 0x0a, 0x3e} };
    GUID IID_AppDomain_l;// = { 0x05f696dc, 0x2b29, 0x3663, {0xad, 0x8b, 0xc4, 0x38, 0x9c, 0xf2, 0xa7, 0x13} };
    wchar_t ver[10];// = L"v4.0.30319"
    wchar_t cla[32];// = L"Hack.Evil"
    wchar_t fun[32];// = L"Main"
    uint64_t data;
    unsigned int data_len;
    uint64_t CLRCreateInstance;
};

int main(int argc, char* argv[])
{
	setvbuf(stdout, NULL, _IONBF, 0); 
    printf("[nezu.cc] staring vmread loader\n");

    if(argc != 2) {
        printf("usage: %s <file.dll>\n", argv[0]);
        return 0;
    }

    std::vector<char> rawData;
    try {
        std::ifstream infile(argv[1], std::ios_base::binary);
        infile.seekg(0, std::ios_base::end);
        size_t length = infile.tellg();
        infile.seekg(0, std::ios_base::beg);
        if (length < 0x1000) {
            printf("failed to load the dll\n");
            return 1;
        }
        rawData.reserve(length);
        std::copy(std::istreambuf_iterator<char>(infile),
            std::istreambuf_iterator<char>(),
            std::back_inserter(rawData));
    }
    catch (const std::system_error&) {
        printf("failed to load the dll\n");
        return 1;
    }

    WinContext ctx(0);
    ctx.processList.Refresh();
    for (auto& proc : ctx.processList) {
        if (!strcasecmp("sectorsedge.exe", proc.proc.name)) {
            PEB peb = proc.GetPeb();
            if (proc.Read<short>(peb.ImageBaseAddress) == IMAGE_DOS_SIGNATURE) {

                printf("Found %s as: %ld\n", proc.proc.name, proc.proc.pid);
                printf("%lx %lx\n", peb.Ldr, peb.ImageBaseAddress);

                // for (auto &m : proc.modules)
                // {
                //     printf("wtf: %s\n", m.info.name);
                // }
                
                uint64_t SwapBuffers = proc.modules.GetModuleInfo("GDI32.dll")->GetProcAddress("SwapBuffers");
                printf("Found SwapBuffers as: %lx\n", SwapBuffers);
                if(!SwapBuffers) return 10;
                uint64_t VirtualAlloc = proc.modules.GetModuleInfo("KERNEL32.dll")->GetProcAddress("VirtualAlloc");
                printf("Found VirtualAlloc as: %lx\n", VirtualAlloc);
                if(!VirtualAlloc) return 10;
                uint64_t VirtualFree = proc.modules.GetModuleInfo("KERNEL32.dll")->GetProcAddress("VirtualFree");
                printf("Found VirtualFree as: %lx\n", VirtualFree);
                if(!VirtualFree) return 10;
                WinDll* overlay = proc.modules.GetModuleInfo("gameoverlayrenderer64.dll");
                WinDll* overlay1 = proc.modules.GetModuleInfo("GameOverlayRenderer64.dll");
                if(!overlay) overlay = overlay1;
                if(!overlay) return 10;
                printf("Found gameoverlayrenderer64.dll as: %lx\n", overlay->info.baseAddress);

                uint8_t headerBuf[0x1000];
                uint8_t x64;
                IMAGE_NT_HEADERS64* ntHeader64 = GetNTHeader(proc.ctx, &proc.proc, overlay->info.baseAddress, headerBuf, &x64);
                IMAGE_DOS_HEADER* dosHeader = (IMAGE_DOS_HEADER*)(void*)headerBuf;
                size_t shsize = ntHeader64->FileHeader.NumberOfSections * sizeof(IMAGE_SECTION_HEADER);
                IMAGE_SECTION_HEADER* sections = (IMAGE_SECTION_HEADER*)malloc(shsize);
                VMemRead(&proc.ctx->process, proc.proc.dirBase, (uint64_t)sections, overlay->info.baseAddress + dosHeader->e_lfanew + sizeof(IMAGE_NT_HEADERS64), shsize);
                
                uint64_t overlay_text = overlay->info.baseAddress;
                uint64_t overlay_text_size = 0;
                uint64_t overlay_data = overlay->info.baseAddress;

                for (size_t i = 0; i < ntHeader64->FileHeader.NumberOfSections; i++) {
                    if(!strcmp(".data", (char*)&sections[i].Name)){
                        overlay_data = overlay->info.baseAddress + sections[i].VirtualAddress + 0x10; //seems to be unused so i'll use it
                    }
                    if(!strcmp(".text", (char*)&sections[i].Name)){
                        overlay_text = overlay->info.baseAddress + sections[i].VirtualAddress; //yolo
                        overlay_text_size = sections[i].SizeOfRawData;
                    }
                }
                
                free(sections);

                printf("overlay_text 0x%lX\n", overlay_text);
                printf("overlay_data 0x%lX\n", overlay_data);
                if(!overlay_text || !overlay_data) {
                    return 10;
                }

                size_t chunk_size = 0x1000;
                void* buff = malloc(chunk_size);
                uint64_t base = peb.Ldr & 0xffffffff00000000;
                std::vector<uint64_t> SwapBuffersPtr;
                printf("Searching for SwapBuffersPtr...\n");
                for (uint64_t i = base - 0x100000000; i < base + 0x100000000; i += chunk_size)
                {
                    if(!VTranslate(&proc.ctx->process, proc.proc.dirBase, i)) continue;
                    proc.Read(i, buff, chunk_size);
                    for (size_t j = 0; j < chunk_size; j+=0x8)
                    {
                        uint64_t val = *reinterpret_cast<uint64_t*>(reinterpret_cast<uint64_t>(buff) + j);
                        if(val == SwapBuffers) {
                            SwapBuffersPtr.push_back(i + j);
                            break;
                        }
                    }
                }
                free(buff);

                if(SwapBuffersPtr.size()){
                    for (uint64_t ptr : SwapBuffersPtr) {
                        printf("Found SwapBuffersPtr as: %lx\n", ptr);
                    }
                    
                    unsigned char alloc_shellcode[] = { 
                        0x48, 0xB8, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, //SwapBuffers  2
                        0x49, 0xBA, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, //&SwapBuffers 12
                        0x49, 0xBB, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, //&rerurn      22
                        0x50, 
                        0x57, 
                        0x51, 
                        0x52, 
                        0x41, 0x50, 
                        0x41, 0x51, 
                        0x41, 0x52, 
                        0x41, 0x53, 
                        0x48, 0x83, 0xEC, 0x28, 
                        0x48, 0xB8, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, //VirtualAlloc 48
                        0x48, 0xC7, 0xC1, 0x00, 0x00, 0x00, 0x00, 
                        0x48, 0xC7, 0xC2, 0x00, 0x00, 0x50, 0x00, 
                        0x41, 0xB8, 0x00, 0x30, 0x00, 0x00, 
                        0x41, 0xB9, 0x04, 0x00, 0x00, 0x00, 
                        0xFF, 0xD0, 
                        0x48, 0x83, 0xC4, 0x28, 
                        0x41, 0x5B, 
                        0x41, 0x5A, 
                        0x49, 0x89, 0x03, 
                        
                        0x48, 0xC7, 0xC1, 0x00, 0x00, 0x00, 0x00, 
                        0xC6, 0x00, 0x00, 
                        0x48, 0x05, 0x00, 0x10, 0x00, 0x00, 
                        0x48, 0xFF, 0xC1, 
                        0x48, 0x81, 0xF9, 0xff, 0x04, 0x00, 0x00, 
                        0x7E, 0xEB,

                        0x41, 0x59, 
                        0x41, 0x58, 
                        0x5A, 
                        0x59, 
                        0x5F, 
                        0x58, 
                        0x49, 0x89, 0x02, 
                        0x41, 0xFF, 0x22 
                    };
                    *(uint64_t*)(&alloc_shellcode[2]) = SwapBuffers;
                    *(uint64_t*)(&alloc_shellcode[12]) = SwapBuffersPtr[0];
                    *(uint64_t*)(&alloc_shellcode[22]) = overlay_data;
                    *(uint64_t*)(&alloc_shellcode[48]) = VirtualAlloc;

                    uint64_t trans = 0;
                    
                    void* buff = malloc(0x1000);
                    int ccCountBest = 0;
                    uint64_t bestcc = 0;
                    for (size_t i = 0; i < overlay_text_size; i += 0x1000)
                    {
                        trans = VTranslate(&proc.ctx->process, proc.proc.dirBase, overlay_text + i);
                        if (trans) {
                            proc.Read(overlay_text + i, buff, 0x1000);
                            int ccCount = 0;
                            uint64_t curcc = 0;
                            for (size_t j = 0; j < 0x1000; j++)
                            {
                                if(((unsigned char*)buff)[j] == 0xCC){
                                    if(!curcc) curcc = j;
                                    ccCount++;
                                } else {
                                    if(ccCount > ccCountBest){
                                        ccCountBest = ccCount;
                                        bestcc = curcc + i + overlay_text;
                                    }
                                    curcc = 0;
                                    ccCount = 0;
                                }
                            }
                            
                        }
                    }
                    free(buff);

                    printf("Best CC 0x%lX: %d\n", bestcc, ccCountBest);

                    overlay_text = bestcc;
                    
                    void* backup = malloc(sizeof(alloc_shellcode));
                    proc.Read(overlay_text, backup, sizeof(alloc_shellcode));
                    uint64_t allocation = 0;
                    size_t SwapBuffersPtrIndex = 0;

                    while (!allocation) {
                        proc.Write(overlay_text, alloc_shellcode, sizeof(alloc_shellcode));
                        proc.Write(SwapBuffersPtr[SwapBuffersPtrIndex], overlay_text);
                        printf("waiting for allocation");
                        int timeout = 10;
                        do {
                            allocation = proc.Read<uint64_t>(overlay_data);
                            if(allocation) break;
                            std::this_thread::sleep_for(std::chrono::milliseconds(50));
                            printf(".");
                        } while(--timeout);
                        printf("\n");
                        if(!allocation){
                            printf("Failed to inject using SwapBuffers[%ld], trying next one...\n", SwapBuffersPtrIndex);
                            proc.Write(SwapBuffersPtr[SwapBuffersPtrIndex], SwapBuffers);
                            SwapBuffersPtrIndex++;
                            if(SwapBuffersPtrIndex >= SwapBuffersPtr.size()) {
                                proc.Write(overlay_text, backup, sizeof(alloc_shellcode));
                                free(backup);
                                printf("Failed to inject, bo more SwapBuffers pointers found\n");
                                return 9;
                            }
                            *(uint64_t*)(&alloc_shellcode[12]) = SwapBuffersPtr[SwapBuffersPtrIndex];
                        }
                    }
                    
                    printf("allocated at: 0x%lX\n", allocation);
                    proc.Write(overlay_text, backup, sizeof(alloc_shellcode));
                    proc.Write(overlay_data, (uint64_t)0);

                    free(backup);

                    trans = VTranslate(&proc.ctx->process, proc.proc.dirBase, allocation);
                    printf("allocation test: %lX\n", trans);
                    if(!trans) return 9;

                    ddd d{0};

                    d.CLSID_CLRMetaHost_l = { 0x9280188d,0xe8e,0x4867,{0xb3, 0xc, 0x7f, 0xa8, 0x38, 0x84, 0xe8, 0xde} };
                    d.IID_ICLRMetaHost_l = { 0xD332DB9E,0xB9B3,0x4125,{0x82, 0x07, 0xA1, 0x48, 0x84, 0xF5, 0x32, 0x16} };
                    d.IID_ICLRRuntimeInfo_l = { 0xBD39D1D2, 0xBA2F, 0x486a, {0x89, 0xB0, 0xB4, 0xB0, 0xCB, 0x46, 0x68, 0x91} };
                    d.CLSID_CorRuntimeHost_l = { 0xcb2f6723, 0xab3a, 0x11d2, {0x9c, 0x40, 0x00, 0xc0, 0x4f, 0xa3, 0x0a, 0x3e} };
                    d.IID_ICorRuntimeHost_l = { 0xcb2f6722, 0xab3a, 0x11d2, {0x9c, 0x40, 0x00, 0xc0, 0x4f, 0xa3, 0x0a, 0x3e} };
                    d.IID_AppDomain_l = { 0x05f696dc, 0x2b29, 0x3663, {0xad, 0x8b, 0xc4, 0x38, 0x9c, 0xf2, 0xa7, 0x13} };

                    const char ver[] = "v\0004\000.\0000\000.\0003\0000\0003\0001\0009\000\000";
                    memcpy(d.ver, ver, sizeof(ver));
                    const char cla[] = "E\000A\000C\000\000";
                    memcpy((reinterpret_cast<char*>(&d.cla) + 4), cla, sizeof(cla));
                    *reinterpret_cast<uint32_t*>(&d.cla) = sizeof(cla) - 2;
                    const char fun[] = "M\000a\000i\000n\000\000";
                    memcpy((reinterpret_cast<char*>(&d.fun) + 4), fun, sizeof(fun));
                    *reinterpret_cast<uint32_t*>(&d.fun) = sizeof(fun) - 2;

                    d.data_len = (unsigned int)rawData.size();
                    d.data = allocation;
                    d.CLRCreateInstance  = proc.modules.GetModuleInfo("MSCOREE.DLL")->GetProcAddress("CLRCreateInstance");
                    printf("Found CLRCreateInstance as: %lx\n", d.CLRCreateInstance);

                    unsigned char injection_shellcode[] =
                    {
                        0x48, 0xB8, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, //mov rax, orig_func            ///ofs 2  dec
                        0x49, 0xBA, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, //mov r10, orig_hook_place      ///ofs 12 dec
                        0x41, 0x52, //push   r10
                        0x50, //push   rax
                        0x53, //push   rbx
                        0x51, //push   rcx
                        0x52, //push   rdx
                        0x41, 0x50, //push   r8
                        0x41, 0x51, //push   r9
                        0x55, //push   rbp
                        0x57, //push   rdi

                        0x48, 0x83, 0xEC, 0x38, //sub rsp, 0x38
                        0x48, 0xB9, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, //mov rcx, data_array           ///ofs 38 dec

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
                        
                        0x48, 0x83, 0xC4, 0x38, //add rsp, 0x38

                        0x48, 0x83, 0xEC, 0x20, //sub rsp, 0x20
                        0x48, 0xB8, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, //mov rax, &VirtualFree
                        0x48, 0xB9, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, //mov rcx, allocated_mem
                        0x48, 0xC7, 0xC2, 0x00, 0x00, 0x00, 0x00, //mov rdx, 0x0
                        0x41, 0xB8, 0x00, 0x80, 0x00, 0x00, //mov r8d, 0x8000 //MEM_RELEASE
                        0xFF, 0xD0, //call rax
                        0x48, 0x83, 0xC4, 0x20, //add rsp, 0x20

                        0x5F, //pop    rdi
                        0x5D, //pop    rbp
                        0x41, 0x59, //pop    r9
                        0x41, 0x58, //pop    r8
                        0x5A, //pop    rdx
                        0x59, //pop    rcx
                        0x5B, //pop    rbx
                        0x58, //pop    rax
                        0x41, 0x5A, //pop    r10

                        0x49, 0x89, 0x02, //mov    QWORD PTR [r10],rax
                        0x41, 0xFF, 0x22, //jmp    QWORD PTR [r10]
                    };

                    *(uint64_t*)(&injection_shellcode[2]) = SwapBuffers;
                    *(uint64_t*)(&injection_shellcode[12]) = SwapBuffersPtr[SwapBuffersPtrIndex];
                    *(uint64_t*)(&injection_shellcode[38]) = d.data + d.data_len;

                    *(uint64_t*)(&injection_shellcode[383]) = VirtualFree;
                    *(uint64_t*)(&injection_shellcode[393]) = allocation;

                    proc.Write(allocation, rawData.data(), rawData.size());
                    proc.Write(allocation + d.data_len, &d, sizeof(d));
                    // proc.Write(allocation + d.data_len + sizeof(d), injection_shellcode, sizeof(injection_shellcode));
                    // proc.Write(SwapBuffersPtr[SwapBuffersPtrIndex], d.data + d.data_len + sizeof(d));

                    backup = malloc(sizeof(injection_shellcode));
                    proc.Read(overlay_text, backup, sizeof(injection_shellcode));

                    proc.Write(overlay_text, injection_shellcode, sizeof(injection_shellcode));
                    proc.Write(SwapBuffersPtr[SwapBuffersPtrIndex], overlay_text);

                    printf("waiting for injection");
                    int timeout = 500;
                    uint64_t SwapBuffersPtrVal = 0;
                    do {
                        SwapBuffersPtrVal = proc.Read<uint64_t>(SwapBuffersPtr[SwapBuffersPtrIndex]);
                        if(SwapBuffersPtrVal == SwapBuffers) break;
                        std::this_thread::sleep_for(std::chrono::milliseconds(200));
                        printf(".");
                    } while(--timeout);
                    printf("\n");
                    if(SwapBuffersPtrVal != SwapBuffers) {
                        printf("Failed to confirm injection, SwapBuffers not called or crashed\n");
                        proc.Write(overlay_text, backup, sizeof(injection_shellcode));
                        free(backup);
                        return 8;
                    }
                    proc.Write(overlay_text, backup, sizeof(injection_shellcode));
                    free(backup);
                    printf("injected!\n");
                    return 0;
                } else {
                    printf("Failed top find SwapBuffersPtr\n");
                }

            }
        }
    }

}
