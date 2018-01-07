using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Windows {
    public sealed class WindowStation : IDisposable {
        IntPtr _handle;

        WindowStation(IntPtr hWinSta) {
            _handle = hWinSta;
        }

        public static WindowStation FromHandle(IntPtr hWinSta) {
            return new WindowStation(hWinSta);
        }

        public static WindowStation Open(string name, WindowStationAccessMask accessMask, bool inheritHandle = false) {
            var winSta = TryOpen(name, accessMask, inheritHandle);               
            return winSta ?? throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public static WindowStation TryOpen(string name, WindowStationAccessMask accessMask, bool inheritHandle = false) {
            var handle = User32.OpenWindowStation(name, inheritHandle, accessMask);
            if (handle == IntPtr.Zero)
                return null;
            return new WindowStation(handle);
        }

        public void Dispose() {
            User32.CloseWindowStation(_handle);
        }
    }
}
