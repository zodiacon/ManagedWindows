using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public enum GenericAccessRights : uint {
        Synchronize = 0x1000000,
        WriteDac = 0x40000,
        WriteOwner = 0x80000,
        Delete = 0x100000,
        ReadControl = 0x20000,
        StandardRightsRequired = 0xf0000
    }

    [Flags]
    enum CreateToolhelpSnapshotFlags {
        None = 0,
        SnapProcess = 2,
        SnapThread = 4,
        SnapModules = 8,
        SnapModules32 = 0x10,
        SnapHeapList = 1,
    };

    public enum ProcessorArchitecture : short {
        Amd64 = 9,
        Arm = 5,
        Itanium = 6,
        x86 = 0,
        Unknown = -1
    }

    [StructLayout(LayoutKind.Sequential)]
    struct SYSTEM_INFO {
        public readonly ProcessorArchitecture ProcessorArchitecture;
        short Reserved;
        public readonly int PageSize;
        public readonly IntPtr MinimumApplicationAddress;
        public readonly IntPtr MaximumApplicationAddress;
        public readonly UIntPtr ActiveProcessorMask;
        public readonly uint NumberOfProcessors;
        int ProcessorType;
        public readonly int AllocationGranularity;
        public readonly ushort ProcessorLevel;
        public readonly ushort ProcessorRevision;

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

        internal void Init() {
            dwSize = Marshal.SizeOf<ProcessEntry>();
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    struct ThreadEntry {
        int Size;
        int Usage;
        public int ThreadId;
        public int ProcessId;
        public int BasePriority;
        int Deltapriority;
        int Flags;

        internal void Init() {
            Size = Marshal.SizeOf<ThreadEntry>();
        }
    }

    [Flags]
    public enum HeapFlags {
        None = 0,
        DefaultHeap = 1,
        SharedHeap = 2,
    }

    [StructLayout(LayoutKind.Sequential)]
    struct HeapList {
        IntPtr Size;
        public int ProcessId;
        public IntPtr HeapId;
        public HeapFlags Flags;

        public void Init() {
            Size = new IntPtr(Marshal.SizeOf<HeapList>());
        }
    }

    [Flags]
    public enum HeapEntryFlags {
        None = 0,
        Fixed = 1,
        Free = 2,
        Moveable = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    struct HeapEntry {
        IntPtr Size;
        public IntPtr Handle;       // Handle of this heap block
        public IntPtr Address;      // Linear address of start of block
        public IntPtr BlockSize;  // Size of block in bytes
        public HeapEntryFlags Flags;
        uint dwLockCount;
        uint dwResvd;
        public int ProcessID;       // owning process
        IntPtr HeapID;              // heap block is in

        public void Init() {
            Size = new IntPtr(Marshal.SizeOf<HeapEntry>());
        }
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

    [StructLayout(LayoutKind.Sequential)]
    struct PERFORMANCE_INFORMATION {
        int cb;
        public IntPtr CommitTotal;
        public IntPtr CommitLimit;
        public IntPtr CommitPeak;
        public IntPtr PhysicalTotal;
        public IntPtr PhysicalAvailable;
        public IntPtr SystemCache;
        public IntPtr KernelTotal;
        public IntPtr KernelPaged;
        public IntPtr KernelNonpaged;
        public IntPtr PageSize;
        public uint HandleCount;
        public uint ProcessCount;
        public uint ThreadCount;
    }

    public enum FirmwareType {
        Unknown = 0,
        Bios = 1,
        Uefi = 2,
    }

    [Flags]
    public enum TokenAccessMask : uint {
    }

    [Flags]
    enum DuplicateHandleOptions {
        None = 0,
        CloseSource = 1,
        SameAccess = 2
    }

    [SuppressUnmanagedCodeSecurity]
    static class Win32 {
        public static readonly IntPtr InvalidFileHandle = new IntPtr(-1);

        [DllImport("kernel32", SetLastError = true)]
        public static extern void GetSystemInfo(out SYSTEM_INFO si);

        [DllImport("kernel32", SetLastError = true)]
        public static extern void GetNativeSystemInfo(out SYSTEM_INFO si);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool Process32First(SafeFileHandle hSnapshot, ref ProcessEntry pe);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool Process32Next(SafeFileHandle hSnapshot, ref ProcessEntry pe);

        [DllImport("kernel32", SetLastError = true)]
        internal static extern SafeFileHandle CreateToolhelp32Snapshot(CreateToolhelpSnapshotFlags flags, int pid = 0);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool Module32First(SafeFileHandle hSnapshot, ref ModuleEntry pe);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool Module32Next(SafeFileHandle hSnapshot, ref ModuleEntry pe);

        [DllImport("kernel32")]
        internal static extern bool Thread32First(SafeFileHandle hSnapshot, ref ThreadEntry pe);

        [DllImport("kernel32")]
        internal static extern bool Thread32Next(SafeFileHandle hSnapshot, ref ThreadEntry pe);

        [DllImport("kernel32")]
        internal static extern bool Heap32First(ref HeapEntry he, int pid, IntPtr heapId);

        [DllImport("kernel32")]
        internal static extern bool Heap32Next(ref HeapEntry he);

        [DllImport("kernel32")]
        internal static extern bool Heap32ListFirst(SafeFileHandle hSnapshot, ref HeapList list);

        [DllImport("kernel32")]
        internal static extern bool Heap32ListNext(SafeFileHandle hSnapshot, ref HeapList list);

        [DllImport("psapi")]
        internal static extern bool GetPerformanceInfo(out PERFORMANCE_INFORMATION info, int size);

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool OpenProcessToken(SafeWaitHandle hProcess, TokenAccessMask accessMask, out SafeKernelHandle handle);

        [DllImport("kernel32")]
        internal static extern bool QueryPerformanceCunter(out long counter);

        [DllImport("kernel32")]
        internal static extern bool QueryPerformanceFrequency(out long counter);

        [DllImport("kernel32")]
        internal static extern bool GetFirmwareType(out FirmwareType type);
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool DuplicateHandle(IntPtr hSourceProcess, SafeHandle hSource, IntPtr hTargetProcess, out SafeKernelHandle hTarget, 
            uint accessMask, bool inheritHandle, DuplicateHandleOptions options = DuplicateHandleOptions.None);

        [DllImport("kernel32")]
        public static extern IntPtr GetCurrentProcess();

    }
}
