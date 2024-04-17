using External.Memory.WinApi;
using External.Memory.WinApi.Enums;
using External.Memory.WinApi.Structs;
using System;
using System.Runtime.InteropServices;

namespace External.Memory.Helpers
{
    internal static class ModuleHelper
    {
        public static IntPtr GetModuleHandle(int processId, string module)
        {
            var entry = GetModuleEntry(processId, module);
            return entry?.hModule ?? IntPtr.Zero;
        }

        public static IntPtr GetModuleBaseAddress(int processId, string module)
        {
            var entry = GetModuleEntry(processId, module);
            return entry?.modBaseAddr ?? IntPtr.Zero;
        }

        private static MODULEENTRY32? GetModuleEntry(int processId, string module)
        {
            var snapshotHandle = Kernel32Lib.CreateToolhelp32Snapshot((uint)(ToolHelp32SnapshotFlags.TH32CS_SNAPMODULE | ToolHelp32SnapshotFlags.TH32CS_SNAPMODULE32), (uint)processId);

            if (snapshotHandle == IntPtr.Zero)
                return null;

            var moduleEntry32 = new MODULEENTRY32();
            moduleEntry32.dwSize = (uint)Marshal.SizeOf<MODULEENTRY32>();

            if (!Kernel32Lib.Module32First(snapshotHandle, ref moduleEntry32))
                return null;

            do
            {
                if (string.Equals(moduleEntry32.szModule, module, StringComparison.OrdinalIgnoreCase))
                    return moduleEntry32;

            } while (Kernel32Lib.Module32Next(snapshotHandle, ref moduleEntry32));

            return null;
        }
    }
}
