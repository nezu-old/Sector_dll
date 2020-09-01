#include "hlapi/hlapi.h"

int main()
{
	
    printf("[nezu.cc] staring vmread loader\n");
    WinContext ctx(0);
    ctx.processList.Refresh();

    for (auto& i : ctx.processList) {
        if (!strcasecmp("sectorsedge.exe", i.proc.name)) {
            PEB peb = i.GetPeb();
            short magic = i.Read<short>(peb.ImageBaseAddress);
                if(magic == IMAGE_DOS_SIGNATURE){

                printf("Found %s as: %lx\n", i.proc.name, i.proc.pid);

                printf("\tExports:\n");
                for (auto& o : i.modules) {
                    printf("\t%.8lx\t%.8lx\t%lx\t%s\n", o.info.baseAddress, o.info.entryPoint, o.info.sizeOfModule, o.info.name);
                    if (!strcmp("friendsui.DLL", o.info.name)) {
                    for (auto& u : o.exports)
                        printf("\t\t%lx\t%s\n", u.address, u.name);
                    }
                }
            }
        }
    }

}