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
        QueryInformation = 0x400,
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

    [Flags]
    public enum PageProtection {
        Invalid = 0,
        ReadOnly = 2,
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
        ExecuteWriteCopy = 0x80,
        NoAccess = 1,
        ReadWrite = 4,
        WriteCopy = 8,
        Guard = 0x100,
        NoCache = 0x200,
        WriteCombine = 0x400
    }

    public enum PageType {
        Invalid = 0,
        Image = 0x1000000,
        Mapped = 0x40000,
        Private = 0x20000
    }

    public enum PageState {
        Free = 0x10000,
        Committed = 0x1000,
        Reserved = 0x2000
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MemoryBasicInformation {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public PageProtection AllocationProtect;
        public IntPtr RegionSize;
        public PageState State;
        public PageProtection Protect;
        public PageType Type;
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

        [DllImport(Library, CharSet = CharSet.Unicode)]
        internal static extern bool ProcessIdToSessionId(int pid, out int sessionid);

        [DllImport(Library, SetLastError = true)]
        internal static extern IntPtr VirtualQueryEx(SafeWaitHandle hProcess, IntPtr address, out MemoryBasicInformation mbi, int size);

    }
}
