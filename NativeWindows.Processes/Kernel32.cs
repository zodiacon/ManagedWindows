using Zodiacon.ManagedWindows.Core;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Processes {
    [Flags]
    enum CreateToolhelpSnapshotFlags {
        None = 0,
        SnapProcess = 2,
        SnapThread = 4,
        SnapModules = 8,
        SnapModules32 = 0x10,
        SnapHeapList = 1,
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessMemoryCounters {
        public int Size;
        public uint PageFaultCount;
        public IntPtr PeakWorkingSetSize;
        public IntPtr WorkingSetSize;
        public IntPtr QuotaPeakPagedPoolUsage;
        public IntPtr QuotaPagedPoolUsage;
        public IntPtr QuotaPeakNonPagedPoolUsage;
        public IntPtr QuotaNonPagedPoolUsage;
        IntPtr PagefileUsage;
        public IntPtr PeakPagefileUsage;
        public IntPtr PrivateUsage;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct ModuleEntry {
        int Size;
        public int th32ModuleID;
        public int th32ProcessID;
        public int GlblcntUsage;
        public int ProccntUsage;
        public IntPtr modBaseAddr;
        public uint modBaseSize;
        public IntPtr hModule;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szModule;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szExePath;

        public void Init() {
            Size = Marshal.SizeOf<ModuleEntry>();
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct ProcessEntry {
        public int dwSize;
        public uint cntUsage;
        public int th32ProcessID;
        public UIntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public int cntThreads;
        public int th32ParentProcessID;
        public ProcessPriorityClass pcPriClassBase;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szExeFile;

        public void Init() {
            dwSize = Marshal.SizeOf<ProcessEntry>();
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IO_COUNTERS {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }

    [Flags]
    public enum JobAccessMask : uint {
        Query = 4,
        SetAttributes = 2,
        Terminate = 8,
        AssignProcess = 1,
        AllAccess = 0x1f001f,
        Synchronize = GenericAccessRights.Synchronize,
        WriteDac = GenericAccessRights.WriteDac,
        WriteOwner = GenericAccessRights.WriteOwner,
        Delete = GenericAccessRights.Delete,
        ReadControl = GenericAccessRights.ReadControl
    }

    [Flags]
    public enum ProcessAccessMask : uint {
        Terminate = 1,
        VMWrite = 0x20,
        VmRead = 0x10,
        VmOperatoin = 0x8,
        CreateProcess = 0x80,
        CreateThread = 2,
        DupHandle = 0x40,
        QueryInformation = 0x40,
        QueryLimitedInformation = 0x1000,
        SetInformation = 0x200,
        SetQuota = 0x100,
        SuspendResume = 0x800,
        AllAccess = 0xffff | GenericAccessRights.Synchronize | GenericAccessRights.StandardRightsRequired,

        Synchronize = GenericAccessRights.Synchronize,
        WriteDac = GenericAccessRights.WriteDac,
        WriteOwner = GenericAccessRights.WriteOwner,
        Delete = GenericAccessRights.Delete,
        ReadControl = GenericAccessRights.ReadControl
    }

    public enum ImageNameType {
        Normal,
        Native = 1
    }

    [Flags]
    public enum ThreadAccessMask : uint {
        AllAccess = 0x7ffff,
        Terminate = 1,
        SuspendResume = 2,
        SetThreadToken = 0x80,
        SetLimitedInformation = 0x400,
        SetInformation = 0x20,
        SetContext = 0x10,
        QueryLimitedInformation = 0x800,
        QueryInformation = 0x40,
        Impersonate = 0x100,
        GetContext = 8,
        DirectImpersonation = 0x200,

        Synchronize = GenericAccessRights.Synchronize,
        WriteDac = GenericAccessRights.WriteDac,
        WriteOwner = GenericAccessRights.WriteOwner,
        Delete = GenericAccessRights.Delete,
        ReadControl = GenericAccessRights.ReadControl
    }

    [Flags]
    public enum ThreadCreateFlags : uint {
        None = 0,
        StackSizeIsReservation = 0x80
    }

    public delegate uint ThreadProc(IntPtr param);

    [SuppressUnmanagedCodeSecurity]
    public static partial class Kernel32 {
        const string Library = "kernel32";

        [DllImport(Library, SetLastError = true)]
        public static extern SafeWaitHandle OpenProcess(ProcessAccessMask desiredAccess, bool inheritHandle, int processId);

        [DllImport(Library, SetLastError = true)]
        public static extern int GetProcessId(SafeWaitHandle hProcess);

        [DllImport(Library, SetLastError = true)]
        public static extern bool TerminateProcess(SafeWaitHandle hProcess, uint exitCode);

        [DllImport(Library, SetLastError = true)]
        public static extern bool GetProcessTimes(SafeWaitHandle hProcess, out long startTime, out long exitTime, out long kernelTime, out long userTime);

        [DllImport(Library, SetLastError = true)]
        public static extern bool GetExitCodeProcess(IntPtr hProcess, ref uint exitCode);

        [DllImport(Library, SetLastError = true)]
        public static extern ProcessPriorityClass GetPriorityClass(SafeWaitHandle hProcess);

        [DllImport(Library, SetLastError = true)]
        public static extern bool GetProcessHandleCount(IntPtr hProcess, ref uint handleCount);

        [DllImport(Library, SetLastError = true)]
        public static extern bool GetProcessIoCounters(IntPtr hProcess, out IO_COUNTERS ioCounters);

        [DllImport(Library, SetLastError = true)]
        public static extern bool SetPriorityClass(SafeWaitHandle hProcess, ProcessPriorityClass priority);

        [DllImport(Library, SetLastError = true)]
        public static extern bool IsProcessInJob(SafeWaitHandle hProcess, SafeWaitHandle hJob, out bool inJob);

        [DllImport(Library, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern SafeWaitHandle CreateJobObject(JobObjectSecurity security, string name);

        [DllImport(Library, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern SafeWaitHandle OpenJobObject(JobAccessMask accessMask, string name, bool inheritHandle = false);

        [DllImport(Library, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool QueryFullProcessImageName(SafeWaitHandle handle, ImageNameType type, StringBuilder imagePath, ref int size);

        [DllImport(Library, SetLastError = true)]
        public static extern SafeWaitHandle OpenThread(ThreadAccessMask accessMask, bool inheritHandle, int threadId);
        [DllImport(Library, SetLastError = true)]
        public static extern IntPtr CreateThread(IntPtr securityAttributes, UIntPtr stackSize, ThreadProc proc, IntPtr parameter, uint flags, out uint id);

        [DllImport(Library, SetLastError = true)]
        public static extern int GetThreadId(SafeWaitHandle hThread);

        [DllImport(Library, SetLastError = true)]
        internal static extern SafeFileHandle CreateToolhelp32Snapshot(CreateToolhelpSnapshotFlags flags, int pid = 0);

        [DllImport(Library, SetLastError = true)]
        internal static extern bool GetProcessMemoryInfo(SafeWaitHandle hProcess, out ProcessMemoryCounters counters, int size);


        [DllImport(Library, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool Process32First(SafeFileHandle hSnapshot, ref ProcessEntry pe);
        [DllImport(Library, CharSet = CharSet.Unicode)]
        internal static extern bool Process32Next(SafeFileHandle hSnapshot, ref ProcessEntry pe);
        [DllImport(Library, CharSet = CharSet.Unicode)]
        internal static extern bool Module32First(SafeFileHandle hSnapshot, ref ModuleEntry pe);
        [DllImport(Library, CharSet = CharSet.Unicode)]
        internal static extern bool Module32Next(SafeFileHandle hSnapshot, ref ModuleEntry pe);

    }
}
