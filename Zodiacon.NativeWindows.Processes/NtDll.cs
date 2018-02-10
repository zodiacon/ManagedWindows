using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;

namespace Zodiacon.ManagedWindows.Processes {
    [Flags]
    public enum HandleAttributes {
        None = 0,
        Inheritable = 1,
        ProtectFromClose = 2
    }

    [StructLayout(LayoutKind.Sequential)]
    struct PROCESS_HANDLE_TABLE_ENTRY_INFO {
        public IntPtr HandleValue;
        public IntPtr HandleCount;
        public UIntPtr PointerCount;
        public uint GrantedAccess;
        public uint ObjectTypeIndex;
        public HandleAttributes HandleAttributes;
        uint Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct PROCESS_HANDLE_SNAPSHOT_INFORMATION {
        public IntPtr NumberOfHandles;
        UIntPtr Reserved;
    }

    static partial class NtDll {
        const string Library = "Ntdll";

        public const int StatusInfoLengthMismatch = unchecked((int)0xc0000004);

        [DllImport(Library, ExactSpelling = true)]
        public unsafe static extern int NtQueryInformationThread(SafeHandle hThread, ThreadInformationClass infoClass, out THREAD_BASIC_INFORMATION info, int size, int* actualSize = null);

        [DllImport(Library, ExactSpelling = true)]
        public unsafe static extern int NtQueryInformationProcess(SafeHandle hProcess, ProcessInformationClass infoClass, void* buffer, int size, int* actualSize = null);

        [DllImport(Library, ExactSpelling = true)]
        public unsafe static extern int NtQueryInformationProcessEx(SafeHandle hProcess, ProcessInformationClass infoClass, void* buffer, int size, int* actualSize = null);
    }
}
