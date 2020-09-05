#include "hlapi/hlapi.h"
#include "data.h"
#include <chrono>
#include <thread>

#pragma pack(1)
typedef struct _GUID {
    unsigned int  Data1;
    unsigned short Data2;
    unsigned short Data3;
    unsigned char  Data4[ 8 ];
} GUID;

#pragma pack(8)
// typedef HRESULT(__stdcall* f_CLRCreateInstance)(REFCLSID clsid, REFIID riid, LPVOID* ppInterface);
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

void writeConfirm(WinProcess& proc, uint64_t from, uint64_t to, uint64_t szie){
    printf("Writing");
    int i = 0;
    do {
        printf(".");
        FlushTlb(GetTlb());
        ssize_t written = VMemWrite(&proc.ctx->process, proc.proc.dirBase, from, to, szie);
        FlushTlb(GetTlb());
        std::this_thread::sleep_for(std::chrono::milliseconds(5));
        i++;
    } while(i < 50);
    printf("\n");
}

int main()
{
	setvbuf(stdout, NULL, _IONBF, 0); 
    printf("[nezu.cc] staring vmread loader\n");
    WinContext ctx(0);
    ctx.processList.Refresh();
    for (auto& proc : ctx.processList) {
        if (!strcasecmp("sectorsedge.exe", proc.proc.name)) {
            PEB peb = proc.GetPeb();
            if (proc.Read<short>(peb.ImageBaseAddress) == IMAGE_DOS_SIGNATURE) {

                printf("Found %s as: %ld\n", proc.proc.name, proc.proc.pid);
                printf("%lx %lx\n", peb.Ldr, peb.ImageBaseAddress);

                uint64_t SwapBuffers = proc.modules.GetModuleInfo("GDI32.dll")->GetProcAddress("SwapBuffers");
                printf("Found SwapBuffers as: %lx\n", SwapBuffers);
                if(!SwapBuffers) return 10;
                uint64_t VirtualAlloc = proc.modules.GetModuleInfo("KERNEL32.dll")->GetProcAddress("VirtualAlloc");
                printf("Found VirtualAlloc as: %lx\n", VirtualAlloc);
                if(!VirtualAlloc) return 10;
                WinDll* overlay = proc.modules.GetModuleInfo("gameoverlayrenderer64.dll");
                if(!overlay) return 10;
                printf("Found gameoverlayrenderer64.dll as: %lx\n", overlay->info.baseAddress);

                uint8_t headerBuf[0x1000];
                uint8_t x64;
                IMAGE_NT_HEADERS64* ntHeader64 = GetNTHeader(proc.ctx, &proc.proc, overlay->info.baseAddress, headerBuf, &x64);
                IMAGE_DOS_HEADER* dosHeader = (IMAGE_DOS_HEADER*)(void*)headerBuf;
                size_t shsize = ntHeader64->FileHeader.NumberOfSections * sizeof(IMAGE_SECTION_HEADER);
                IMAGE_SECTION_HEADER* sections = (IMAGE_SECTION_HEADER*)malloc(shsize);
                VMemRead(&proc.ctx->process, proc.proc.dirBase, (uint64_t)sections, overlay->info.baseAddress + dosHeader->e_lfanew + sizeof(IMAGE_NT_HEADERS64), shsize);
                
                uint64_t overlay_text_free = overlay->info.baseAddress;
                uint64_t overlay_text_free_size = 0;
                uint64_t overlay_data = overlay->info.baseAddress;
                bool last_text = false;
                // for (size_t i = 0; i < ntHeader64->FileHeader.NumberOfSections; i++) {
                //     uint64_t addy = overlay->info.baseAddress + sections[i].VirtualAddress;
                //     overlay_text_free_size = addy - overlay_text_free;
                //     if(!strcmp(".text", (char*)&sections[i].Name)){
                //         last_text = true;
                //     } else if(last_text) {
                //         break;
                //     }
                //     overlay_text_free = addy + sections[i].SizeOfRawData;
                // }
                // overlay_text_free -= 0x100;
                for (size_t i = 0; i < ntHeader64->FileHeader.NumberOfSections; i++) {
                    if(!strcmp(".data", (char*)&sections[i].Name)){
                        overlay_data = overlay->info.baseAddress + sections[i].VirtualAddress + 0x10; //seems to be unused so i'll use it
                    }
                    if(!strcmp(".text", (char*)&sections[i].Name)){
                        overlay_text_free = overlay->info.baseAddress + sections[i].VirtualAddress; //yolo
                    }
                }
                
                free(sections);

                printf("overlay_text_free 0x%lX\n", overlay_text_free);
                printf("overlay_data 0x%lX\n", overlay_data);
                if(!overlay_text_free || !overlay_data) {
                    return 10;
                }

                size_t chunk_size = 0x1000;
                void* buff = malloc(chunk_size);
                uint64_t base = peb.Ldr & 0xffffffff00000000;
                uint64_t SwapBuffersPtr = 0;
                for (uint64_t i = base - 0x100000000; i < base + 0x100000000; i += chunk_size)
                {
                    proc.Read(i, buff, chunk_size);
                    for (size_t j = 0; j < chunk_size; j+=0x20)
                    {
                        uint64_t val = *reinterpret_cast<uint64_t*>(reinterpret_cast<uint64_t>(buff) + j);
                        if(val == SwapBuffers){
                            SwapBuffersPtr = i + j;
                            break;
                        }
                    }
                }
                free(buff);

                if(SwapBuffersPtr){
                    printf("Found SwapBuffersPtr as: %lx\n", SwapBuffersPtr);
                    
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
                        0x48, 0xC7, 0xC2, 0x00, 0x00, 0x10, 0x00, 
                        0x41, 0xB8, 0x00, 0x30, 0x00, 0x00, 
                        0x41, 0xB9, 0x40, 0x00, 0x00, 0x00, 
                        0xFF, 0xD0, 
                        0x48, 0x83, 0xC4, 0x28, 
                        0x41, 0x5B, 
                        0x41, 0x5A, 
                        0x49, 0x89, 0x03, 
                        
                        0x48, 0xC7, 0xC1, 0x00, 0x00, 0x00, 0x00, 
                        0xC6, 0x00, 0x00, 
                        0x48, 0x05, 0x00, 0x10, 0x00, 0x00, 
                        0x48, 0xFF, 0xC1, 
                        0x48, 0x81, 0xF9, 0x50, 0x00, 0x00, 0x00, 
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
                    *(uint64_t*)(&alloc_shellcode[12]) = SwapBuffersPtr;
                    *(uint64_t*)(&alloc_shellcode[22]) = overlay_data;
                    *(uint64_t*)(&alloc_shellcode[48]) = VirtualAlloc;

                    uint64_t trans = VTranslate(&proc.ctx->process, proc.proc.dirBase, overlay_text_free);
                    printf("trans overlay_text_free: %lX\n", trans);
                    if(!trans) return 9;
                    
                    void* backup = malloc(sizeof(alloc_shellcode));
                    proc.Read(overlay_text_free, backup, sizeof(alloc_shellcode));

                    proc.Write(overlay_text_free, alloc_shellcode, sizeof(alloc_shellcode));
                    proc.Write(SwapBuffersPtr, overlay_text_free);
                    printf("waiting");
                    uint64_t allocation = 0;
                    do {
                        allocation = proc.Read<uint64_t>(overlay_data);
                        std::this_thread::sleep_for(std::chrono::milliseconds(10));
                        printf(".");
                    } while(allocation == 0);
                    printf("\nalocated at: 0x%lX\n", allocation);
                    proc.Write(overlay_text_free, backup, sizeof(alloc_shellcode));
                    proc.Write(overlay_data, (uint64_t)0);

                    free(backup);

                    trans = VTranslate(&proc.ctx->process, proc.proc.dirBase, allocation);
                    printf("trans allocation1: %lX\n", trans);
                    if(!trans) return 9;
                    trans = VTranslate(&proc.ctx->process, proc.proc.dirBase, allocation + 0x1000);
                    printf("trans allocation2: %lX\n", trans);
                    if(!trans) return 9;

                    std::this_thread::sleep_for(std::chrono::milliseconds(100));

                    ddd d{0};

                    d.CLSID_CLRMetaHost_l = { 0x9280188d,0xe8e,0x4867,{0xb3, 0xc, 0x7f, 0xa8, 0x38, 0x84, 0xe8, 0xde} };
                    d.IID_ICLRMetaHost_l = { 0xD332DB9E,0xB9B3,0x4125,{0x82, 0x07, 0xA1, 0x48, 0x84, 0xF5, 0x32, 0x16} };
                    d.IID_ICLRRuntimeInfo_l = { 0xBD39D1D2, 0xBA2F, 0x486a, {0x89, 0xB0, 0xB4, 0xB0, 0xCB, 0x46, 0x68, 0x91} };
                    d.CLSID_CorRuntimeHost_l = { 0xcb2f6723, 0xab3a, 0x11d2, {0x9c, 0x40, 0x00, 0xc0, 0x4f, 0xa3, 0x0a, 0x3e} };
                    d.IID_ICorRuntimeHost_l = { 0xcb2f6722, 0xab3a, 0x11d2, {0x9c, 0x40, 0x00, 0xc0, 0x4f, 0xa3, 0x0a, 0x3e} };
                    d.IID_AppDomain_l = { 0x05f696dc, 0x2b29, 0x3663, {0xad, 0x8b, 0xc4, 0x38, 0x9c, 0xf2, 0xa7, 0x13} };

                    const char ver[] = "v\0004\000.\0000\000.\0003\0000\0003\0001\0009\000\000";
                    memcpy(d.ver, ver, sizeof(ver));
                    const char cla[] = "H\000a\000c\000k\000.\000E\000v\000i\000l\000\000";
                    memcpy((reinterpret_cast<char*>(&d.cla) + 4), cla, sizeof(cla));
                    *reinterpret_cast<uint32_t*>(&d.cla) = sizeof(cla) - 2;
                    const char fun[] = "M\000a\000i\000n\000\000";
                    memcpy((reinterpret_cast<char*>(&d.fun) + 4), fun, sizeof(fun));
                    *reinterpret_cast<uint32_t*>(&d.fun) = sizeof(fun) - 2;

                    d.data_len = sizeof(rawData);
                    d.data = allocation;
                    d.CLRCreateInstance  = proc.GetModuleInfo("MSCOREE.DLL")->GetProcAddress("CLRCreateInstance");
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
                    *(uint64_t*)(&injection_shellcode[12]) = SwapBuffersPtr;
                    *(uint64_t*)(&injection_shellcode[38]) = d.data + d.data_len;

                    proc.Write(allocation, rawData, sizeof(rawData));
                    proc.Write(allocation + d.data_len, &d, sizeof(d));
                    proc.Write(allocation + d.data_len + sizeof(d), injection_shellcode, sizeof(injection_shellcode));

                    proc.Write(SwapBuffersPtr, d.data + d.data_len + sizeof(d));

                }

            }
        }
    }

}