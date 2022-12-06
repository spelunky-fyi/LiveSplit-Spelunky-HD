

using System;
using System.Runtime.InteropServices;

namespace LiveSplit.Spelunky
{
  public static class Kernel32
  {
    public static readonly uint MBI_LENGTH = (uint) Marshal.SizeOf(typeof (Kernel32.MEMORY_BASIC_INFORMATION));

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(
      Kernel32.ProcessPermissionsEnum dwDesiredAccess,
      bool bInheritHandle,
      int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(
      int hProcess,
      int lpBaseAddress,
      byte[] lpBuffer,
      int dwSize,
      ref int lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    public static extern bool WriteProcessMemory(
      int hProcess,
      int lpBaseAddress,
      byte[] lpBuffer,
      int dwSize,
      ref int lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    public static extern int VirtualQueryEx(
      int hProcess,
      IntPtr lpAddress,
      ref Kernel32.MEMORY_BASIC_INFORMATION lpBuffer,
      uint dwLength);

    [DllImport("kernel32.dll")]
    public static extern bool VirtualProtectEx(
      int hProcess,
      IntPtr lpAddress,
      int dwSize,
      Kernel32.PagePermissionsEnum flNewProtect,
      ref Kernel32.PagePermissionsEnum lpflOldProtect);

    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(int hObject);

    [DllImport("kernel32.dll")]
    public static extern int GetLastError();

    public enum AllocationProtectEnum : uint
    {
      PAGE_NOACCESS = 1,
      PAGE_READONLY = 2,
      PAGE_READWRITE = 4,
      PAGE_WRITECOPY = 8,
      PAGE_EXECUTE = 16, // 0x00000010
      PAGE_EXECUTE_READ = 32, // 0x00000020
      PAGE_EXECUTE_READWRITE = 64, // 0x00000040
      PAGE_EXECUTE_WRITECOPY = 128, // 0x00000080
      PAGE_GUARD = 256, // 0x00000100
      PAGE_NOCACHE = 512, // 0x00000200
      PAGE_WRITECOMBINE = 1024, // 0x00000400
    }

    public enum StateEnum : uint
    {
      MEM_COMMIT = 4096, // 0x00001000
      MEM_RESERVE = 8192, // 0x00002000
      MEM_FREE = 65536, // 0x00010000
    }

    public enum TypeEnum : uint
    {
      MEM_PRIVATE = 131072, // 0x00020000
      MEM_MAPPED = 262144, // 0x00040000
      MEM_IMAGE = 16777216, // 0x01000000
    }

    public struct MEMORY_BASIC_INFORMATION
    {
      public IntPtr BaseAddress;
      public IntPtr AllocationBase;
      public Kernel32.AllocationProtectEnum AllocationProtect;
      public IntPtr RegionSize;
      public Kernel32.StateEnum State;
      public Kernel32.AllocationProtectEnum Protect;
      public Kernel32.TypeEnum Type;
    }

    public enum ProcessPermissionsEnum : uint
    {
      PROCESS_TERMINATE = 1,
      PROCESS_CREATE_THREAD = 2,
      PROCESS_VM_OPERATION = 8,
      PROCESS_VM_READ = 16, // 0x00000010
      PROCESS_VM_WRITE = 32, // 0x00000020
      PROCESS_DUP_HANDLE = 64, // 0x00000040
      PROCESS_CREATE_PROCESS = 128, // 0x00000080
      PROCESS_SET_QUOTA = 256, // 0x00000100
      PROCESS_SET_INFORMATION = 512, // 0x00000200
      PROCESS_QUERY_INFORMATION = 1024, // 0x00000400
      PROCESS_SUSPEND_RESUME = 2048, // 0x00000800
      PROCESS_QUERY_LIMITED_INFORMATION = 4096, // 0x00001000
      SYNCHRONIZE = 1048576, // 0x00100000
      PROCESS_ALL_ACCESS = 1056763, // 0x00101FFB
    }

    public enum PagePermissionsEnum : uint
    {
      PAGE_NOACCESS = 1,
      PAGE_READONLY = 2,
      PAGE_READWRITE = 4,
      PAGE_WRITECOPY = 8,
      PAGE_EXECUTE = 16, // 0x00000010
      PAGE_EXECUTE_READ = 32, // 0x00000020
      PAGE_EXECUTE_READWRITE = 64, // 0x00000040
      PAGE_EXECUTE_WRITECOPY = 128, // 0x00000080
      PAGE_TARGETS_NO_UPDATE = 1024, // 0x00000400
      PAGE_TARGETS_INVALID = 1073741824, // 0x40000000
    }

    public enum PageModifiers : uint
    {
      PAGE_GUARD = 256, // 0x00000100
      PAGE_NOCACHE = 512, // 0x00000200
      PAGE_WRITECOMBINE = 1024, // 0x00000400
    }
  }
}
