using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;

namespace Zodiacon.ManagedWindows.Processes {
    static partial class NtDll {
        const string Library = "Ntdll";

        [DllImport(Library, ExactSpelling = true)]
        public unsafe static extern int NtQueryInformationThread(SafeHandle hThread, ThreadInformationClass infoClass, out THREAD_BASIC_INFORMATION info, int size, int* actualSize = null);

        [DllImport(Library, ExactSpelling = true)]
        public unsafe static extern int NtQueryInformationProcess(SafeHandle hProcess, ProcessInformationClass infoClass, void* buffer, int size, int* actualSize = null);
    }
}
