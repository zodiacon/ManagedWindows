using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Windows {
    public sealed class NativeWindow {
        IntPtr _handle;

        private NativeWindow(IntPtr hWnd) {
            _handle = hWnd;
        }

        public static NativeWindow FromHandle(IntPtr hWnd) {
            return User32.IsWindow(hWnd) ? new NativeWindow(hWnd) : throw new ArgumentException("Handle is not a valid Window");
        }

        public static NativeWindow ShellWindow {
            get {
                var hshell = User32.GetShellWindow();
                if (hshell == IntPtr.Zero)
                    throw new InvalidOperationException("No shell window present");
                return new NativeWindow(hshell);
            }
        }

        public static NativeWindow ForegroundWindow {
            get {
                var hWnd = User32.GetForegroundWindow();
                if (hWnd == IntPtr.Zero)
                    throw new InvalidOperationException("No foreground window");
                return new NativeWindow(hWnd);
            }
        }

        public IntPtr Handle => _handle;

        public static NativeWindow DesktopWindow => new NativeWindow(User32.GetDesktopWindow());

        public bool IsValid => User32.IsWindow(_handle);

        public NativeWindow Parent {
            get {
                var parent = User32.GetParent(_handle);
                return parent == IntPtr.Zero ? null : new NativeWindow(parent);
            }
        }
            
        public void Destroy() {
            Debug.Assert(IsValid);
            User32.DestroyWindow(_handle).ThrowIfWin32Failed();
        }

        public void Minimize() {
            Debug.Assert(IsValid);
            User32.CloseWindow(_handle).ThrowIfWin32Failed();
        }

        public int SendMessage(uint message, uint wParam = 0, uint lParam = 0) => User32.SendMessage(_handle, message, wParam, lParam);

        public bool PostMessage(uint message, uint wParam = 0, uint lParam = 0) => User32.PostMessage(_handle, message, wParam, lParam);

        public static NativeWindow[] EnumWindows() {
            var windows = new List<NativeWindow>(64);

            User32.EnumWindows((h, _) => {
                windows.Add(new NativeWindow(h));
                return true;
            }, IntPtr.Zero);

            return windows.ToArray();
        }

        public NativeWindow[] EnumChildWindows() {
            var windows = new List<NativeWindow>(16);

            User32.EnumChildWindows(_handle, (h, _) => {
                windows.Add(new NativeWindow(h));
                return true;
            }, IntPtr.Zero);

            return windows.ToArray();
        }

        public bool IsVisible => User32.IsWindowVisible(_handle);
        public bool IsMaximized => User32.IsZoomed(_handle);
        public bool IsHung => User32.IsHungAppWindow(_handle);

        public unsafe int ThreadId => User32.GetWindowThreadProcessId(_handle);

        public Rect WindowRect => User32.GetWindowRect(_handle, out var rc) ? rc : throw new Win32Exception(Marshal.GetLastWin32Error());

        public Rect ClientRect => User32.GetClientRect(_handle, out var rc) ? rc : throw new Win32Exception(Marshal.GetLastWin32Error());

        public bool IsMinimized => User32.IsIconic(_handle);
        public string Text {
            get {
                var text = new StringBuilder(512);
                User32.GetWindowText(_handle, text, text.Capacity);
                return text.ToString();
            }
            set => User32.SetWindowText(_handle, value).ThrowIfWin32Failed();
        }

        public WindowStyle Style {
            get => (WindowStyle)(User32.GetWindowLongPtr(_handle, GetWindowLongIndex.Style).ToUInt32() & 0xffff0000);
            set => User32.SetWindowLongPtr(_handle, GetWindowLongIndex.Style, new UIntPtr((uint)value));
        }

        public ExtendedWindowStyle ExtendedStyle {
            get => (ExtendedWindowStyle)User32.GetWindowLongPtr(_handle, GetWindowLongIndex.ExtendedStyle).ToUInt32();
            set => User32.SetWindowLongPtr(_handle, GetWindowLongIndex.ExtendedStyle, new UIntPtr((uint)value));
        }

        public int ID {
            get => (int)User32.GetWindowLongPtr(_handle, GetWindowLongIndex.ID).ToUInt32();
            set => User32.SetWindowLongPtr(_handle, GetWindowLongIndex.ID, new UIntPtr((uint)value));
        }

        public void Show(ShowWindowType type = ShowWindowType.Show) => User32.ShowWindow(_handle, type).ThrowIfWin32Failed();

        public void ShowAsync(ShowWindowType type = ShowWindowType.Show) => User32.ShowWindowAsync(_handle, type).ThrowIfWin32Failed();
        public bool Flash(bool invert) => User32.FlashWindow(_handle, invert);
    }
}
