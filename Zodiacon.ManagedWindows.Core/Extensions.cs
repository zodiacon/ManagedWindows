using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows {
    public static class Extensions {
        public static IntPtr ThrowIfFailed(this IntPtr handle) {
            if (handle == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return handle;
        }

        public static T ThrowIfFailed<T>(this T result) {
            if (Marshal.GetLastWin32Error() != 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return result;
        }

        public static T ThrowIfZero<T>(this T result) where T : struct {
            if (result.Equals(default(T)))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return result;
        }

        public static void ThrowIfWin32Failed(this bool success) {
            if (!success)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }
}
