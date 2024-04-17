using External.Memory.Helpers;
using External.Memory.WinApi;
using External.Memory.WinApi.Enums;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace External.Memory
{
    public sealed class ProcessMemory
    {
        public IntPtr ProcessHandle { get; private set; }
        public int ProcessId { get; private set; }

        public ProcessMemory(string processName) : this(ProcessHelper.GetProcessIdByName(processName))
        {
        }

        public ProcessMemory(IntPtr processHandle) : this((int)Kernel32Lib.GetProcessId(processHandle))
        {
        }

        public ProcessMemory(int processId)
        {
            if (processId < 0)
                throw new InvalidOperationException("Process ID is negative");

            var processHandle = Kernel32Lib.OpenProcess((uint)ProcessAccessFlags.PROCESS_ALL_ACCESS, false, processId);

            if (processHandle == IntPtr.Zero)
                throw new InvalidOperationException("Couldnt obtain process handle. Check if process is running, or lack of application permissions");

            ProcessId = processId;
            ProcessHandle = processHandle;
        }

        public IntPtr GetModuleHandle(string moduleName)
        {
            return ModuleHelper.GetModuleHandle(ProcessId, moduleName);
        }

        public IntPtr GetModuleBaseAddress(string moduleName)
        {
            return ModuleHelper.GetModuleBaseAddress(ProcessId, moduleName);
        }

        public IntPtr GetProcAddress(string proc)
        {
            return Kernel32Lib.GetProcAddress(ProcessHandle, proc);
        }

        public IntPtr GetProcAddress(IntPtr handle, string proc)
        {
            return Kernel32Lib.GetProcAddress(handle, proc);
        }

        #region reading memory
        public T Read<T>(IntPtr address) where T : struct
        {
            var buffer = ReadBytes(address, Marshal.SizeOf<T>());
            return BytesToStructure<T>(buffer);
        }

        public string ReadString(IntPtr address, int length)
        {
            var buffer = ReadBytes(address, length);
            return Encoding.ASCII.GetString(buffer);
        }

        public byte[] ReadBytes(IntPtr address, int length)
        {
            byte[] buffer = new byte[length];
            Kernel32Lib.ReadProcessMemory(ProcessHandle, address, buffer, buffer.Length, out var _);
            return buffer;
        }
        #endregion

        #region writing memory
        public void Write<T>(IntPtr address, T value) where T : notnull
        {
            byte[] buffer = StructureToBytes(value);
            Write(address, buffer);
        }

        private void Write(IntPtr address, byte[] buffer)
        {
            Kernel32Lib.WriteProcessMemory(ProcessHandle, address, buffer, buffer.Length, out var _);
        }
        #endregion

        private T BytesToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        private byte[] StructureToBytes(object value)
        {
            int size = Marshal.SizeOf(value);
            byte[] array = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);
            return array;
        }
    }
}