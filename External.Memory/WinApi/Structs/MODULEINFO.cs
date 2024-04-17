using System;
using System.Runtime.InteropServices;

namespace External.Memory.WinApi.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MODULEINFO
    {
        public IntPtr lpBaseOfDll;
        public uint SizeOfImage;
        public IntPtr EntryPoint;
    }
}
