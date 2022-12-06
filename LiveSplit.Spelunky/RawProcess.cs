

using System;
using System.Diagnostics;

namespace LiveSplit.Spelunky
{
  public class RawProcess : IDisposable
  {
    private IntPtr ProcessHandle;
    private Process Process;
    private const int BUF_SCAN_SIZE = 4096;

    public string ProcessName { get; private set; }

    public int BaseAddress { get; private set; }

    public bool HasExited => this.Process.HasExited;

    public string FilePath => this.Process.MainModule.FileName;

    public RawProcess(string processName)
    {
      this.ProcessName = processName;
      Process[] processesByName = Process.GetProcessesByName(processName);
      Process process = processesByName.Length != 0 ? processesByName[0] : throw new Exception("Failed to find process by name: " + processName);
      this.Process = process;
      this.BaseAddress = process.MainModule.BaseAddress.ToInt32();
      this.ProcessHandle = Kernel32.OpenProcess(Kernel32.ProcessPermissionsEnum.PROCESS_ALL_ACCESS, false, process.Id);
    }

    private int? FindSignatureMatch(byte[] buf, int bufUsed, byte?[] signature)
    {
      int num1 = bufUsed - signature.Length + 1;
      int num2 = 0;
label_7:
      if (num2 >= num1)
        return new int?();
      int num3 = 0;
      for (int index = 0; index < signature.Length; ++index)
      {
        byte? nullable = signature[index];
        if (!nullable.HasValue || (int) nullable.Value == (int) buf[index + num2])
        {
          ++num3;
        }
        else
        {
          ++num2;
          goto label_7;
        }
      }
      return new int?(num2);
    }

    public int? FindBytes(byte?[] signature, int startAddr = 0, int endAddr = 50331648)
    {
      int num1 = startAddr;
      Kernel32.MEMORY_BASIC_INFORMATION lpBuffer = new Kernel32.MEMORY_BASIC_INFORMATION();
      byte[] bytes = new byte[4096];
      int num2 = 0;
      while (Kernel32.VirtualQueryEx((int) this.ProcessHandle, (IntPtr) num1, ref lpBuffer, Kernel32.MBI_LENGTH) > 0)
      {
        int num3 = (int) lpBuffer.BaseAddress + (int) lpBuffer.RegionSize;
        if (lpBuffer.State == Kernel32.StateEnum.MEM_COMMIT)
        {
          if ((int) lpBuffer.BaseAddress > num1)
            num1 = (int) lpBuffer.BaseAddress;
          int bufUsed;
          if (num1 < endAddr)
          {
            for (; num1 < num3; num1 += bufUsed)
            {
              bufUsed = Math.Min(bytes.Length, num3 - num1);
              if (this.ReadBytes(num1, ref bytes) != 0)
              {
                int? signatureMatch = this.FindSignatureMatch(bytes, bufUsed, signature);
                if (signatureMatch.HasValue)
                  return new int?(num1 + signatureMatch.Value);
              }
              else
                break;
            }
          }
          else
            break;
        }
        num1 = num3;
        ++num2;
      }
      return new int?();
    }

    public int WriteInt32(int address, int value) => this.WriteBytes(address, BitConverter.GetBytes(value));

    public int WriteBool(int address, bool value) => this.WriteBytes(address, BitConverter.GetBytes(value));

    public int WriteDouble(int address, double value) => this.WriteBytes(address, BitConverter.GetBytes(value));

    public int WriteSingle(int address, float value) => this.WriteBytes(address, BitConverter.GetBytes(value));

    public void SetMemoryWritable(int address, int numBytes)
    {
      Kernel32.PagePermissionsEnum lpflOldProtect = (Kernel32.PagePermissionsEnum) 0;
      if (!Kernel32.VirtualProtectEx((int) this.ProcessHandle, (IntPtr) address, numBytes, Kernel32.PagePermissionsEnum.PAGE_EXECUTE_READWRITE, ref lpflOldProtect))
        throw new Exception("Failed to set memory writeable: " + (object) Kernel32.GetLastError());
    }

    public int WriteBytes(int address, byte[] bytes)
    {
      this.AssertValid();
      int lpNumberOfBytesWritten = 0;
      Kernel32.WriteProcessMemory((int) this.ProcessHandle, address, bytes, bytes.Length, ref lpNumberOfBytesWritten);
      return lpNumberOfBytesWritten;
    }

    public int ReadInt32(int address) => BitConverter.ToInt32(this.ReadBytes(address, 4), 0);

    public bool ReadBool(int address) => BitConverter.ToBoolean(this.ReadBytes(address, 1), 0);

    public float ReadSingle(int address) => BitConverter.ToSingle(this.ReadBytes(address, 4), 0);

    public double ReadDouble(int address) => BitConverter.ToDouble(this.ReadBytes(address, 8), 0);

    public byte[] ReadBytes(int address, int count)
    {
      byte[] bytes = new byte[count];
      this.ReadBytes(address, ref bytes);
      return bytes;
    }

    public int ReadBytes(int address, ref byte[] bytes)
    {
      this.AssertValid();
      int lpNumberOfBytesRead = 0;
      Kernel32.ReadProcessMemory((int) this.ProcessHandle, address, bytes, bytes.Length, ref lpNumberOfBytesRead);
      return lpNumberOfBytesRead;
    }

    public void AssertValid()
    {
      if (this.ProcessHandle == IntPtr.Zero)
        throw new ObjectDisposedException(nameof (RawProcess));
      if (this.Process.HasExited)
        throw new Exception("Target process has exited");
    }

    public void Dispose()
    {
      if (this.ProcessHandle == IntPtr.Zero)
        return;
      Kernel32.CloseHandle((int) this.ProcessHandle);
      this.ProcessHandle = IntPtr.Zero;
    }
  }
}
