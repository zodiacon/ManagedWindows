using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Processes {
    [SuppressUnmanagedCodeSecurity]
    public static partial class User32 {
        const string Library = "User32";

        [DllImport(Library)]
        public static extern bool IsImmersiveProcess(SafeWaitHandle hProcess);
    }
}
