using System.Runtime.InteropServices;

namespace QuakeReloaded;

internal class Win32Utils
{
    [StructLayout(LayoutKind.Explicit)]
    struct IMAGE_DOS_HEADER
    {
        [FieldOffset(0)]
        public short e_magic;

        [FieldOffset(60)]
        public int e_lfanew;
    }

    public struct IMAGE_EXPORT_DIRECTORY
    {
        public uint Characteristics;
        public uint TimeDateStamp;
        public ushort MajorVersion;
        public ushort MinorVersion;
        public uint Name;
        public uint Base;
        public uint NumberOfFunctions;
        public uint NumberOfNames;
        public uint AddressOfFunctions;
        public uint AddressOfNames;
        public uint AddressOfNameOrdinals;
    }

    public static unsafe string GetImageExportName(IntPtr moduleBaseAddress)
    {
        IMAGE_DOS_HEADER* dosHeader = (IMAGE_DOS_HEADER*)moduleBaseAddress.ToPointer();

        if (dosHeader->e_magic != 0x5A4D) // MZ
            throw new ArgumentException("The specified module is not a valid PE module", nameof(moduleBaseAddress));

        var exportDirectory = (IMAGE_EXPORT_DIRECTORY*)(moduleBaseAddress + *((int*)(moduleBaseAddress + dosHeader->e_lfanew + 0x88)));
        string dllName = new string((sbyte*)(moduleBaseAddress + exportDirectory->Name));

        return dllName;
    }
}
