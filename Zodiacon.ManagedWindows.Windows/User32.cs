using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Windows {
    [Flags]
    public enum WindowStationAccessMask {
        AllAccess = 0x37f,
        AccessClipboard = 4,
        AccessGlobalAtoms = 0x20,
        CreateDesktop = 8,
        EnumDesktops = 1,
        Enumerate = 0x100,
        ExitWindows = 0x40,
        ReadAttributes = 2,
        ReadScreen = 0x200,
        WriteAttributes = 0x10,
    }

    delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr param);

    [SuppressUnmanagedCodeSecurity]
    static class User32 {
        const string Library = "user32";

        [DllImport(Library, SetLastError = true)]
        public static extern bool CloseWindowStation(IntPtr hWinSta);

        [DllImport(Library, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr OpenWindowStation(string name, bool inherit, WindowStationAccessMask accessMask);

        [DllImport(Library, SetLastError = true)]
        public static extern bool IsWindow(IntPtr hwnd);

        [DllImport(Library, SetLastError = true)]
        public static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport(Library, SetLastError = true)]
        public static extern int SendMessage(IntPtr hwnd, uint message, uint wParam, uint lParam);

        [DllImport(Library, SetLastError = true)]
        public static extern bool PostMessage(IntPtr hwnd, uint message, uint wParam, uint lParam);

        [DllImport(Library, SetLastError = true)]
        public static extern bool CloseWindow(IntPtr hwnd);

        [DllImport(Library, SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport(Library, SetLastError = true)]
        public static extern IntPtr GetShellWindow();

        [DllImport(Library, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport(Library, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport(Library)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport(Library)]
        public static extern bool IsZoomed(IntPtr hWnd);

        [DllImport(Library)]
        public static extern bool IsHungAppWindow(IntPtr hWnd);

        [DllImport(Library)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport(Library, SetLastError = true)]
        public static extern bool EnumWindows(EnumWindowsProc proc, IntPtr param);

        [DllImport(Library, SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rect rc);

        [DllImport(Library, SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, out Rect rc);

        [DllImport(Library, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

        [DllImport(Library, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SetWindowText(IntPtr hWnd, string text);

        [DllImport(Library, SetLastError = true)]
        public unsafe static extern int GetWindowThreadProcessId(IntPtr hWnd, int* pid = null);

        [DllImport(Library, SetLastError = true)]
        public static extern bool EnumChildWindows(IntPtr hWnd, EnumWindowsProc proc, IntPtr param);

    }
}
