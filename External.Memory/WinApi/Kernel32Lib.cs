using External.Memory.WinApi.Structs;
using System;
using System.Runtime.InteropServices;

namespace External.Memory.WinApi
{
    internal static class Kernel32Lib
    {
        // reads process memory
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
           IntPtr hProcess,
           IntPtr lpBaseAddress,
           [Out] byte[] lpBuffer,
           int nSize,
           out int lpNumberOfBytesRead
           );

        // writes process memory
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int size,
            out int lpNumberOfBytesWritten
            );

        // opens access to  a process handle
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            uint dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
            int dwProcessId);

        // closes process handle access
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(
            IntPtr hObject
            );

        // gets proc address
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(
            IntPtr hModule,
            string procName
            );

        // gets the process id by its handle
        [DllImport("kernel32.dll")]
        public static extern uint GetProcessId(
            IntPtr processHandle
            );

        // creates toolhelp32
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateToolhelp32Snapshot(
            uint dwFlags,
            uint th32ProcessID
            );

        // enumerates first module
        [DllImport("kernel32.dll")]
        public static extern bool Module32First(
            IntPtr hSnapshot,
            ref MODULEENTRY32 lpme
            );

        // enumerates further modules
        [DllImport("kernel32.dll")]
        public static extern bool Module32Next(
            IntPtr hSnapshot,
            ref MODULEENTRY32 lpme
            );

        // enumerates first process
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Process32First(
            IntPtr hSnapshot,
            ref PROCESSENTRY32 lppe
            );

        // enumerates further processes
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Process32Next(
            IntPtr hSnapshot,
            ref PROCESSENTRY32 lppe
            );

    }
}
