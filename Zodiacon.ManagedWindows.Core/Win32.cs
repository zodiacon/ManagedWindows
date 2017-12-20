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

        public void Init() {
            dwSize = Marshal.SizeOf<ProcessEntry>();
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

    [SuppressUnmanagedCodeSecurity]
    static class Win32 {
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

    }
}
