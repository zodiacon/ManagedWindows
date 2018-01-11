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

    [Flags]
    public enum WindowStyle : uint {
        Border = 0x800000,
        Caption = 0xc00000,
        Child = 0x40000000,
        ClipChildren = 0x2000000,
        ClipSiblings = 0x4000000,
        Disabled = 0x8000000,
        DialogFrame = 0x400000,
        Group = 0x20000,
        HScroll = 0x100000,
        Maximize = 0x1000000,
        MaximizeBox = 0x10000,
        Minimize = 0x20000000,
        MinimizeBox = 0x20000,
        Overlapped = 0,
        Popup = 0x80000000U,
        SizeBox = 0x40000,
        SysMenu = 0x80000,
        TabStop = 0x10000,
        Visible = 0x10000000,
        VScroll = 0x200000,
        PopupWindow = Popup | Border | SysMenu
    }

    [Flags]
    public enum ExtendedWindowStyle : uint {
        None = 0,
        AcceptFiles = 0x10,
        AppWindow = 0x40000,
        ClientEdge = 0x200,
        Composited = 0x2000000,
        ContextHelp = 0x400,
        ControlParent = 0x10000,
        DialogModalFrame = 1,
        Layered = 0x80000,
        LayoutRTL = 0x400000,
        LeftScrollBar = 0x4000,
        MDIChild = 0x40,
        NoActivate = 0x8000000,
        NoInheritLayout = 0x100000,
        NoParentNotify = 4,
        NoRedirectionBitmap = 0x200000,
        WindowEdge = 0x100,
        Transparent = 0x20,
        Topmost = 8,
        ToolWindow = 0x80,
        StaticEdge = 0x20000,
        RTLReading = 0x2000,
        PalleteWindow = WindowEdge | ToolWindow | Topmost,
        OverlappedWindow = WindowEdge | ClientEdge
    }

    public enum ShowWindowType : int {
        ForceMinimize = 11,
        Hide = 0,
        Maximize = 3,
        Minimize = 6,
        Restore = 9,
        Show = 5,
        ShowDefault = 10,
        ShowMaximized = 3,
        ShowMinimized = 2,
        ShowMinimizedNoActivate = 7,
        ShowNoActivate = 4,
        ShowNormal = 1
    }

    enum GetWindowLongIndex {
        ExtendedStyle = -20,
        hInstance = -6,
        hWndParent = -8,
        ID = -12,
        Style = -16,
        UserData = -21,
        WindowProc = -4,
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

        [DllImport(Library, SetLastError = true)]
        public static extern UIntPtr GetWindowLongPtr(IntPtr hWnd, GetWindowLongIndex index);

        [DllImport(Library, SetLastError = true)]
        public static extern UIntPtr SetWindowLongPtr(IntPtr hWnd, GetWindowLongIndex index, UIntPtr value);

        [DllImport(Library, SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowType type);

        [DllImport(Library, SetLastError = true)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, ShowWindowType type);

        [DllImport(Library)]
        public static extern bool FlashWindow(IntPtr hWnd, bool invert);

    }
}
