using External.Memory.WinApi;
using External.Memory.WinApi.Enums;
using External.Memory.WinApi.Structs;
using System;
using System.Runtime.InteropServices;

namespace External.Memory.Helpers
{
    internal static class ProcessHelper
    {
        public static int GetProcessIdByName(string processName)
        {
            var snapshotHandle = Kernel32Lib.CreateToolhelp32Snapshot((uint)ToolHelp32SnapshotFlags.TH32CS_SNAPPROCESS, 0);
            if (snapshotHandle == IntPtr.Zero)
                return -1;

            var processEntry32 = new PROCESSENTRY32();
            processEntry32.dwSize = (uint)Marshal.SizeOf<PROCESSENTRY32>();

            if (!Kernel32Lib.Process32First(snapshotHandle, ref processEntry32))
                return -1;

            do
            {
                if (string.Equals(processEntry32.szExeFile, processName, StringComparison.OrdinalIgnoreCase))
                {
                    return (int)processEntry32.th32ProcessID;
                }

            } while (Kernel32Lib.Process32Next(snapshotHandle, ref processEntry32));


            return -1;
        }
    }
}
