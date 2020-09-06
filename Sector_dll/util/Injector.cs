using ManualMapInjection.Injection.Types;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Sector_dll.util
{
    class Injector
    {

        public static bool Inject(byte[] data, IntPtr drawCallback)
        {

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr baseAddress = handle.AddrOfPinnedObject();

            PIMAGE_NT_HEADERS64 imageNtHeaders = GetNtHeader(baseAddress);

            if (imageNtHeaders == null)
            {
                Log.Debug("[LoadImageToMemory] Invalid Image: No IMAGE_NT_HEADERS");
                return false;
            }

            IntPtr allocatedMemory = VirtualAlloc(IntPtr.Zero, new UIntPtr(imageNtHeaders.Value.OptionalHeader.SizeOfImage), 
                AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ExecuteReadWrite);

            if (allocatedMemory == IntPtr.Zero)
            {
                Log.Debug("[LoadImageToMemory] Failed to allocate remote memory for module");
                return false;
            }

            if (imageNtHeaders.Value.OptionalHeader.ImportTable.Size > 0)
            {
                var imageImportDescriptor = (PIMAGE_IMPORT_DESCRIPTOR)RvaToPointer(imageNtHeaders.Value.OptionalHeader.ImportTable.VirtualAddress, baseAddress);

                if (imageImportDescriptor != null)
                {
                    for (; imageImportDescriptor.Value.Name > 0; imageImportDescriptor++)
                    {
                        PCHAR moduleName = (PCHAR)RvaToPointer(imageImportDescriptor.Value.Name, baseAddress);
                        if (moduleName == null)
                        {
                            continue;
                        }

                        if (moduleName.ToString().Contains("-ms-win-crt-"))
                        {
                            moduleName = new PCHAR("ucrtbase.dll");
                        }

                        var moduleBase = LoadLibrary(moduleName.ToString());
                        if (moduleBase == IntPtr.Zero)
                        {
                        }

                        PIMAGE_THUNK_DATA imageThunkData;
                        PIMAGE_THUNK_DATA imageFuncData;

                        imageThunkData = (PIMAGE_THUNK_DATA)RvaToPointer(imageImportDescriptor.Value.OriginalFirstThunk, baseAddress);
                        imageFuncData = (PIMAGE_THUNK_DATA)RvaToPointer(imageImportDescriptor.Value.FirstThunk, baseAddress);


                        for (; imageThunkData.Value.AddressOfData > 0; imageThunkData++, imageFuncData++)
                        {
                            IntPtr functionAddress;
                            var bSnapByOrdinal = (imageThunkData.Value.Ordinal & /*IMAGE_ORDINAL_FLAG64*/ 0x8000000000000000) != 0;

                            if (bSnapByOrdinal)
                            {
                                var ordinal = (short)(imageThunkData.Value.Ordinal & 0xffff);
                                functionAddress = GetProcAddress(moduleBase, new IntPtr(ordinal));
                                if (functionAddress == IntPtr.Zero)
                                {
                                    Log.Danger("Failed to resolve " + moduleName.ToString() + "." + ordinal);
                                    return false;
                                }
                            }
                            else
                            {
                                var imageImportByName = (PIMAGE_IMPORT_BY_NAME)RvaToPointer(imageThunkData.Value.AddressOfData, baseAddress);
                                var mameOfImport = (PCHAR)imageImportByName.Address + /*imageImportByName->Name*/ 2;
                                functionAddress = GetProcAddress(moduleBase, mameOfImport.ToString());
                                if (functionAddress == IntPtr.Zero)
                                {
                                    Log.Danger("Failed to resolve " + moduleName.ToString() + "." + mameOfImport.ToString());
                                    return false;
                                }
                            }

                            Marshal.WriteInt64(imageFuncData.Address, functionAddress.ToInt64());
                        }
                    }

                }
                else
                {
                    Log.Danger("Size of table confirmed but pointer to data invalid");
                    return false;
                }
            }
            else
            {
                Log.Danger("no imports");
            }

            var imageBaseDelta = (ulong)(allocatedMemory.ToInt64() - (long)imageNtHeaders.Value.OptionalHeader.ImageBase);
            var relocationSize = imageNtHeaders.Value.OptionalHeader.BaseRelocationTable.Size;

            if (relocationSize > 0)
            {
                var relocationDirectory = (PIMAGE_BASE_RELOCATION)RvaToPointer(imageNtHeaders.Value.OptionalHeader.BaseRelocationTable.VirtualAddress, baseAddress);

                if (relocationDirectory != null)
                {
                    var relocationEnd = (PBYTE)relocationDirectory.Address + (int)relocationSize;

                    while (relocationDirectory.Address.ToInt64() < relocationEnd.Address.ToInt64())
                    {
                        var relocBase = (PBYTE)RvaToPointer(relocationDirectory.Value.VirtualAddress, baseAddress);
                        var numRelocs = (relocationDirectory.Value.SizeOfBlock - 8) >> 1;
                        var relocationData = (PWORD)((relocationDirectory + 1).Address);

                        for (uint i = 0; i < numRelocs; i++, relocationData++)
                        {
                            if (((relocationData.Value >> 12) & 0xF) == 10)
                            {
                                PULONG raw2 = (PULONG)(relocBase + (relocationData.Value & 0xFFF)).Address;
                                Marshal.WriteInt64(raw2.Address, unchecked((long)(raw2.Value + imageBaseDelta)));
                            } 
                        }

                        relocationDirectory = (PIMAGE_BASE_RELOCATION)relocationData.Address;
                    }
                }
                else
                {
                    return false;
                }

            }

            var imageSectionHeader = (PIMAGE_SECTION_HEADER)(imageNtHeaders.Address + /*OptionalHeader*/ 24 + imageNtHeaders.Value.FileHeader.SizeOfOptionalHeader);
            for (ushort i = 0; i < imageNtHeaders.Value.FileHeader.NumberOfSections; i++)
            {
                WriteProcessMemory(
                    new IntPtr(-1),
                    new IntPtr(allocatedMemory.ToInt64() + imageSectionHeader[i].VirtualAddress),
                    new IntPtr(baseAddress.ToInt64() + imageSectionHeader[i].PointerToRawData),
                    imageSectionHeader[i].SizeOfRawData,
                    out _);
            }

            var dllEntryPoint = allocatedMemory.ToInt64() + imageNtHeaders.Value.OptionalHeader.AddressOfEntryPoint;

            DllMain dllMain = Marshal.GetDelegateForFunctionPointer<DllMain>(new IntPtr(dllEntryPoint));

            dllMain(allocatedMemory, 1, drawCallback);

            //Log.Debug(dllEntryPoint.ToString("X"));

            return true;

        }

        private static IntPtr RvaToPointer(ulong rva, IntPtr baseAddress)
        {
            var imageNtHeaders = GetNtHeader(baseAddress);
            if (imageNtHeaders == null)
            {
                return IntPtr.Zero;
            }

            return ImageRvaToVa(imageNtHeaders.Address, baseAddress, new UIntPtr(rva), IntPtr.Zero);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DllMain(IntPtr hinstDLL, uint fdwReason, IntPtr lpReserved);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("Dbghelp.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr ImageRvaToVa(IntPtr NtHeaders, IntPtr Base, UIntPtr Rva, [Optional] IntPtr LastRvaSection);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, IntPtr ordinal);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
          IntPtr hProcess,
          IntPtr lpBaseAddress,
          IntPtr lpBuffer,
          uint dwSize,
          out IntPtr lpNumberOfBytesWritten);

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        private static PIMAGE_NT_HEADERS64 GetNtHeader(IntPtr address)
        {
            var imageDosHeader = GetDosHeader(address);

            if (imageDosHeader == null)
            {
                return null;
            }

            var imageNtHeaders = (PIMAGE_NT_HEADERS64)(address + imageDosHeader.Value.e_lfanew);

            if (!imageNtHeaders.Value.isValid)
            {
                return null;
            }

            return imageNtHeaders;
        }

        private static PIMAGE_DOS_HEADER GetDosHeader(IntPtr address)
        {
            var imageDosHeader = (PIMAGE_DOS_HEADER)address;

            if (!imageDosHeader.Value.isValid)
            {
                return null;
            }

            return imageDosHeader;
        }


    }

    [Flags]
    public enum DataSectionFlags : uint
    {
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeReg = 0x00000000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeDsect = 0x00000001,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeNoLoad = 0x00000002,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeGroup = 0x00000004,

        /// <summary>
        /// The section should not be padded to the next boundary. This flag is obsolete and is replaced by IMAGE_SCN_ALIGN_1BYTES. This is valid only for object files.
        /// </summary>
        TypeNoPadded = 0x00000008,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeCopy = 0x00000010,

        /// <summary>
        /// The section contains executable code.
        /// </summary>
        ContentCode = 0x00000020,

        /// <summary>
        /// The section contains initialized data.
        /// </summary>
        ContentInitializedData = 0x00000040,

        /// <summary>
        /// The section contains uninitialized data.
        /// </summary>
        ContentUninitializedData = 0x00000080,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        LinkOther = 0x00000100,

        /// <summary>
        /// The section contains comments or other information. The .drectve section has this type. This is valid for object files only.
        /// </summary>
        LinkInfo = 0x00000200,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeOver = 0x00000400,

        /// <summary>
        /// The section will not become part of the image. This is valid only for object files.
        /// </summary>
        LinkRemove = 0x00000800,

        /// <summary>
        /// The section contains COMDAT data. For more information, see section 5.5.6, COMDAT Sections (Object Only). This is valid only for object files.
        /// </summary>
        LinkComDat = 0x00001000,

        /// <summary>
        /// Reset speculative exceptions handling bits in the TLB entries for this section.
        /// </summary>
        NoDeferSpecExceptions = 0x00004000,

        /// <summary>
        /// The section contains data referenced through the global pointer (GP).
        /// </summary>
        RelativeGP = 0x00008000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MemPurgeable = 0x00020000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        Memory16Bit = 0x00020000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MemoryLocked = 0x00040000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MemoryPreload = 0x00080000,

        /// <summary>
        /// Align data on a 1-byte boundary. Valid only for object files.
        /// </summary>
        Align1Bytes = 0x00100000,

        /// <summary>
        /// Align data on a 2-byte boundary. Valid only for object files.
        /// </summary>
        Align2Bytes = 0x00200000,

        /// <summary>
        /// Align data on a 4-byte boundary. Valid only for object files.
        /// </summary>
        Align4Bytes = 0x00300000,

        /// <summary>
        /// Align data on an 8-byte boundary. Valid only for object files.
        /// </summary>
        Align8Bytes = 0x00400000,

        /// <summary>
        /// Align data on a 16-byte boundary. Valid only for object files.
        /// </summary>
        Align16Bytes = 0x00500000,

        /// <summary>
        /// Align data on a 32-byte boundary. Valid only for object files.
        /// </summary>
        Align32Bytes = 0x00600000,

        /// <summary>
        /// Align data on a 64-byte boundary. Valid only for object files.
        /// </summary>
        Align64Bytes = 0x00700000,

        /// <summary>
        /// Align data on a 128-byte boundary. Valid only for object files.
        /// </summary>
        Align128Bytes = 0x00800000,

        /// <summary>
        /// Align data on a 256-byte boundary. Valid only for object files.
        /// </summary>
        Align256Bytes = 0x00900000,

        /// <summary>
        /// Align data on a 512-byte boundary. Valid only for object files.
        /// </summary>
        Align512Bytes = 0x00A00000,

        /// <summary>
        /// Align data on a 1024-byte boundary. Valid only for object files.
        /// </summary>
        Align1024Bytes = 0x00B00000,

        /// <summary>
        /// Align data on a 2048-byte boundary. Valid only for object files.
        /// </summary>
        Align2048Bytes = 0x00C00000,

        /// <summary>
        /// Align data on a 4096-byte boundary. Valid only for object files.
        /// </summary>
        Align4096Bytes = 0x00D00000,

        /// <summary>
        /// Align data on an 8192-byte boundary. Valid only for object files.
        /// </summary>
        Align8192Bytes = 0x00E00000,

        /// <summary>
        /// The section contains extended relocations.
        /// </summary>
        LinkExtendedRelocationOverflow = 0x01000000,

        /// <summary>
        /// The section can be discarded as needed.
        /// </summary>
        MemoryDiscardable = 0x02000000,

        /// <summary>
        /// The section cannot be cached.
        /// </summary>
        MemoryNotCached = 0x04000000,

        /// <summary>
        /// The section is not pageable.
        /// </summary>
        MemoryNotPaged = 0x08000000,

        /// <summary>
        /// The section can be shared in memory.
        /// </summary>
        MemoryShared = 0x10000000,

        /// <summary>
        /// The section can be executed as code.
        /// </summary>
        MemoryExecute = 0x20000000,

        /// <summary>
        /// The section can be read.
        /// </summary>
        MemoryRead = 0x40000000,

        /// <summary>
        /// The section can be written to.
        /// </summary>
        MemoryWrite = 0x80000000
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_SECTION_HEADER
    {
        [FieldOffset(0)] [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public char[] Name;

        [FieldOffset(8)] public UInt32 VirtualSize;

        [FieldOffset(12)] public UInt32 VirtualAddress;

        [FieldOffset(16)] public UInt32 SizeOfRawData;

        [FieldOffset(20)] public UInt32 PointerToRawData;

        [FieldOffset(24)] public UInt32 PointerToRelocations;

        [FieldOffset(28)] public UInt32 PointerToLinenumbers;

        [FieldOffset(32)] public UInt16 NumberOfRelocations;

        [FieldOffset(34)] public UInt16 NumberOfLinenumbers;

        [FieldOffset(36)] public DataSectionFlags Characteristics;

        public string Section
        {
            get { return new string(Name); }
        }
    }

    public class PIMAGE_SECTION_HEADER : ManagedPtr<IMAGE_SECTION_HEADER>
    {
        public PIMAGE_SECTION_HEADER(IntPtr address) : base(address)
        {
        }

        public PIMAGE_SECTION_HEADER(object value) : base(value)
        {
        }

        public static explicit operator PIMAGE_SECTION_HEADER(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            return new PIMAGE_SECTION_HEADER(ptr);
        }
    }

    public class PULONG : ManagedPtr<ulong>
    {
        public PULONG(IntPtr address) : base(address)
        {
        }

        public PULONG(object value) : base(value)
        {
        }

        public static PULONG operator +(PULONG c1, int c2)
        {
            return new PULONG(c1.Address + c2 * c1.StructSize);
        }

        public static PULONG operator ++(PULONG a)
        {
            return a + 1;
        }

        public static explicit operator PULONG(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            return new PULONG(ptr);
        }
    }

    public class PWORD : ManagedPtr<ushort>
    {
        public PWORD(IntPtr address) : base(address)
        {
        }

        public PWORD(object value) : base(value)
        {
        }

        public static PWORD operator +(PWORD c1, int c2)
        {
            return new PWORD(c1.Address + c2 * c1.StructSize);
        }

        public static PWORD operator ++(PWORD a)
        {
            return a + 1;
        }

        public static explicit operator PWORD(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            return new PWORD(ptr);
        }
    }

    public class PBYTE : ManagedPtr<byte>
    {
        public PBYTE(IntPtr address) : base(address)
        {
        }

        public PBYTE(object value) : base(value)
        {
        }

        public static PBYTE operator +(PBYTE c1, int c2)
        {
            return new PBYTE(c1.Address + c2 * c1.StructSize);
        }

        public static PBYTE operator ++(PBYTE a)
        {
            return a + 1;
        }

        public static explicit operator PBYTE(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            return new PBYTE(ptr);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_BASE_RELOCATION
    {
        public UInt32 VirtualAddress;
        public UInt32 SizeOfBlock;
    }

    public class PIMAGE_BASE_RELOCATION : ManagedPtr<IMAGE_BASE_RELOCATION>
    {
        public PIMAGE_BASE_RELOCATION(IntPtr address) : base(address)
        {
        }

        public PIMAGE_BASE_RELOCATION(object value) : base(value)
        {
        }

        public static PIMAGE_BASE_RELOCATION operator +(PIMAGE_BASE_RELOCATION c1, int c2)
        {
            return new PIMAGE_BASE_RELOCATION(c1.Address + c2 * c1.StructSize);
        }

        public static PIMAGE_BASE_RELOCATION operator ++(PIMAGE_BASE_RELOCATION a)
        {
            return a + 1;
        }

        public static explicit operator PIMAGE_BASE_RELOCATION(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            return new PIMAGE_BASE_RELOCATION(ptr);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_IMPORT_BY_NAME
    {
        public short Hint;
        public char Name;
    }

    public class PIMAGE_IMPORT_BY_NAME : ManagedPtr<IMAGE_IMPORT_BY_NAME>
    {
        public PIMAGE_IMPORT_BY_NAME(IntPtr address) : base(address)
        {
        }

        public PIMAGE_IMPORT_BY_NAME(object value) : base(value)
        {
        }

        public static PIMAGE_IMPORT_BY_NAME operator +(PIMAGE_IMPORT_BY_NAME c1, int c2)
        {
            return new PIMAGE_IMPORT_BY_NAME(c1.Address + c2 * c1.StructSize);
        }

        public static PIMAGE_IMPORT_BY_NAME operator ++(PIMAGE_IMPORT_BY_NAME a)
        {
            return a + 1;
        }

        public static explicit operator PIMAGE_IMPORT_BY_NAME(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            return new PIMAGE_IMPORT_BY_NAME(ptr);
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_THUNK_DATA
    {
        [FieldOffset(0)] public ulong ForwarderString; // PBYTE 

        [FieldOffset(0)] public ulong Function; // PDWORD

        [FieldOffset(0)] public ulong Ordinal;

        [FieldOffset(0)] public ulong AddressOfData; // PIMAGE_IMPORT_BY_NAME
    }

    public class PIMAGE_THUNK_DATA : ManagedPtr<IMAGE_THUNK_DATA>
    {
        public PIMAGE_THUNK_DATA(IntPtr address) : base(address)
        {
        }

        public PIMAGE_THUNK_DATA(object value) : base(value)
        {
        }

        public static PIMAGE_THUNK_DATA operator +(PIMAGE_THUNK_DATA c1, int c2)
        {
            return new PIMAGE_THUNK_DATA(c1.Address + c2 * c1.StructSize);
        }

        public static PIMAGE_THUNK_DATA operator ++(PIMAGE_THUNK_DATA a)
        {
            return a + 1;
        }

        public static explicit operator PIMAGE_THUNK_DATA(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            return new PIMAGE_THUNK_DATA(ptr);
        }
    }

    public class PCHAR : ManagedPtr<char>
    {
        public PCHAR(IntPtr address) : base(address)
        {
        }

        public PCHAR(object value) : base(value)
        {
        }

        public PCHAR(string value) : base(Encoding.UTF8.GetBytes(value))
        {
        }

        public static PCHAR operator +(PCHAR c1, int c2)
        {
            return new PCHAR(c1.Address + c2 * c1.StructSize);
        }

        public static PCHAR operator ++(PCHAR a)
        {
            return a + 1;
        }

        public static explicit operator PCHAR(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            return new PCHAR(ptr);
        }

        public override string ToString()
        {
            return Marshal.PtrToStringAnsi(Address) ?? string.Empty;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_IMPORT_DESCRIPTOR
    {
        [FieldOffset(0)] public uint Characteristics;

        [FieldOffset(0)] public uint OriginalFirstThunk;

        [FieldOffset(4)] public uint TimeDateStamp;

        [FieldOffset(8)] public uint ForwarderChain;

        [FieldOffset(12)] public uint Name;

        [FieldOffset(16)] public uint FirstThunk;
    }

    public class PIMAGE_IMPORT_DESCRIPTOR : ManagedPtr<IMAGE_IMPORT_DESCRIPTOR>
    {
        public PIMAGE_IMPORT_DESCRIPTOR(IntPtr address) : base(address)
        {
        }

        public PIMAGE_IMPORT_DESCRIPTOR(object value) : base(value)
        {
        }

        public static PIMAGE_IMPORT_DESCRIPTOR operator +(PIMAGE_IMPORT_DESCRIPTOR c1, int c2)
        {
            return new PIMAGE_IMPORT_DESCRIPTOR(c1.Address + c2 * c1.StructSize);
        }

        public static PIMAGE_IMPORT_DESCRIPTOR operator ++(PIMAGE_IMPORT_DESCRIPTOR a)
        {
            return a + 1;
        }

        public static explicit operator PIMAGE_IMPORT_DESCRIPTOR(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            return new PIMAGE_IMPORT_DESCRIPTOR(ptr);
        }
    }

    public class PIMAGE_NT_HEADERS64 : ManagedPtr<IMAGE_NT_HEADERS64>
    {
        public PIMAGE_NT_HEADERS64(IntPtr address) : base(address)
        {
        }

        public PIMAGE_NT_HEADERS64(object value) : base(value)
        {
        }

        public static explicit operator PIMAGE_NT_HEADERS64(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            return new PIMAGE_NT_HEADERS64(ptr);
        }
    }

    public class PIMAGE_DOS_HEADER : ManagedPtr<IMAGE_DOS_HEADER>
    {
        public PIMAGE_DOS_HEADER(IntPtr address) : base(address)
        {
        }

        public PIMAGE_DOS_HEADER(object value) : base(value)
        {
        }

        public static explicit operator PIMAGE_DOS_HEADER(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            return new PIMAGE_DOS_HEADER(ptr);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_DOS_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public char[] e_magic; // Magic number
        public UInt16 e_cblp; // Bytes on last page of file
        public UInt16 e_cp; // Pages in file
        public UInt16 e_crlc; // Relocations
        public UInt16 e_cparhdr; // Size of header in paragraphs
        public UInt16 e_minalloc; // Minimum extra paragraphs needed
        public UInt16 e_maxalloc; // Maximum extra paragraphs needed
        public UInt16 e_ss; // Initial (relative) SS value
        public UInt16 e_sp; // Initial SP value
        public UInt16 e_csum; // Checksum
        public UInt16 e_ip; // Initial IP value
        public UInt16 e_cs; // Initial (relative) CS value
        public UInt16 e_lfarlc; // File address of relocation table
        public UInt16 e_ovno; // Overlay number
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public UInt16[] e_res1; // Reserved words
        public UInt16 e_oemid; // OEM identifier (for e_oeminfo)
        public UInt16 e_oeminfo; // OEM information; e_oemid specific
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)] public UInt16[] e_res2; // Reserved words
        public Int32 e_lfanew; // File address of new exe header

        private string _e_magic
        {
            get { return new string(e_magic); }
        }

        public bool isValid
        {
            get { return _e_magic == "MZ"; }
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_NT_HEADERS64
    {
        [FieldOffset(0)]
        public byte Signature1;
        [FieldOffset(1)]
        public byte Signature2;
        [FieldOffset(2)]
        public byte Signature3;
        [FieldOffset(3)]
        public byte Signature4;

        [FieldOffset(4)] public IMAGE_FILE_HEADER FileHeader;

        [FieldOffset(24)] public IMAGE_OPTIONAL_HEADER64 OptionalHeader;

        public bool isValid
        {
            get
            {
                return (char)Signature1 == 'P' && (char)Signature2 == 'E' && (char)Signature3 == '\0' && (char)Signature4 == '\0';
            }
        }
    }

    public enum MachineType : ushort
    {
        Native = 0,
        I386 = 0x014c,
        Itanium = 0x0200,
        x64 = 0x8664
    }
    public enum MagicType : ushort
    {
        IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b,
        IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b
    }
    public enum SubSystemType : ushort
    {
        IMAGE_SUBSYSTEM_UNKNOWN = 0,
        IMAGE_SUBSYSTEM_NATIVE = 1,
        IMAGE_SUBSYSTEM_WINDOWS_GUI = 2,
        IMAGE_SUBSYSTEM_WINDOWS_CUI = 3,
        IMAGE_SUBSYSTEM_POSIX_CUI = 7,
        IMAGE_SUBSYSTEM_WINDOWS_CE_GUI = 9,
        IMAGE_SUBSYSTEM_EFI_APPLICATION = 10,
        IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER = 11,
        IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER = 12,
        IMAGE_SUBSYSTEM_EFI_ROM = 13,
        IMAGE_SUBSYSTEM_XBOX = 14

    }
    public enum DllCharacteristicsType : ushort
    {
        RES_0 = 0x0001,
        RES_1 = 0x0002,
        RES_2 = 0x0004,
        RES_3 = 0x0008,
        IMAGE_DLL_CHARACTERISTICS_DYNAMIC_BASE = 0x0040,
        IMAGE_DLL_CHARACTERISTICS_FORCE_INTEGRITY = 0x0080,
        IMAGE_DLL_CHARACTERISTICS_NX_COMPAT = 0x0100,
        IMAGE_DLLCHARACTERISTICS_NO_ISOLATION = 0x0200,
        IMAGE_DLLCHARACTERISTICS_NO_SEH = 0x0400,
        IMAGE_DLLCHARACTERISTICS_NO_BIND = 0x0800,
        RES_4 = 0x1000,
        IMAGE_DLLCHARACTERISTICS_WDM_DRIVER = 0x2000,
        IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE = 0x8000
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_DATA_DIRECTORY
    {
        public UInt32 VirtualAddress;
        public UInt32 Size;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_OPTIONAL_HEADER64
    {
        [FieldOffset(0)]
        public MagicType Magic;

        [FieldOffset(2)]
        public byte MajorLinkerVersion;

        [FieldOffset(3)]
        public byte MinorLinkerVersion;

        [FieldOffset(4)]
        public uint SizeOfCode;

        [FieldOffset(8)]
        public uint SizeOfInitializedData;

        [FieldOffset(12)]
        public uint SizeOfUninitializedData;

        [FieldOffset(16)]
        public uint AddressOfEntryPoint;

        [FieldOffset(20)]
        public uint BaseOfCode;

        [FieldOffset(24)]
        public ulong ImageBase;

        [FieldOffset(32)]
        public uint SectionAlignment;

        [FieldOffset(36)]
        public uint FileAlignment;

        [FieldOffset(40)]
        public ushort MajorOperatingSystemVersion;

        [FieldOffset(42)]
        public ushort MinorOperatingSystemVersion;

        [FieldOffset(44)]
        public ushort MajorImageVersion;

        [FieldOffset(46)]
        public ushort MinorImageVersion;

        [FieldOffset(48)]
        public ushort MajorSubsystemVersion;

        [FieldOffset(50)]
        public ushort MinorSubsystemVersion;

        [FieldOffset(52)]
        public uint Win32VersionValue;

        [FieldOffset(56)]
        public uint SizeOfImage;

        [FieldOffset(60)]
        public uint SizeOfHeaders;

        [FieldOffset(64)]
        public uint CheckSum;

        [FieldOffset(68)]
        public SubSystemType Subsystem;

        [FieldOffset(70)]
        public DllCharacteristicsType DllCharacteristics;

        [FieldOffset(72)]
        public ulong SizeOfStackReserve;

        [FieldOffset(80)]
        public ulong SizeOfStackCommit;

        [FieldOffset(88)]
        public ulong SizeOfHeapReserve;

        [FieldOffset(96)]
        public ulong SizeOfHeapCommit;

        [FieldOffset(104)]
        public uint LoaderFlags;

        [FieldOffset(108)]
        public uint NumberOfRvaAndSizes;

        [FieldOffset(112)]
        public IMAGE_DATA_DIRECTORY ExportTable;

        [FieldOffset(120)]
        public IMAGE_DATA_DIRECTORY ImportTable;

        [FieldOffset(128)]
        public IMAGE_DATA_DIRECTORY ResourceTable;

        [FieldOffset(136)]
        public IMAGE_DATA_DIRECTORY ExceptionTable;

        [FieldOffset(144)]
        public IMAGE_DATA_DIRECTORY CertificateTable;

        [FieldOffset(152)]
        public IMAGE_DATA_DIRECTORY BaseRelocationTable;

        [FieldOffset(160)]
        public IMAGE_DATA_DIRECTORY Debug;

        [FieldOffset(168)]
        public IMAGE_DATA_DIRECTORY Architecture;

        [FieldOffset(176)]
        public IMAGE_DATA_DIRECTORY GlobalPtr;

        [FieldOffset(184)]
        public IMAGE_DATA_DIRECTORY TLSTable;

        [FieldOffset(192)]
        public IMAGE_DATA_DIRECTORY LoadConfigTable;

        [FieldOffset(200)]
        public IMAGE_DATA_DIRECTORY BoundImport;

        [FieldOffset(208)]
        public IMAGE_DATA_DIRECTORY IAT;

        [FieldOffset(216)]
        public IMAGE_DATA_DIRECTORY DelayImportDescriptor;

        [FieldOffset(224)]
        public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;

        [FieldOffset(232)]
        public IMAGE_DATA_DIRECTORY Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_FILE_HEADER
    {
        public UInt16 Machine;
        public UInt16 NumberOfSections;
        public UInt32 TimeDateStamp;
        public UInt32 PointerToSymbolTable;
        public UInt32 NumberOfSymbols;
        public UInt16 SizeOfOptionalHeader;
        public UInt16 Characteristics;
    }

}
