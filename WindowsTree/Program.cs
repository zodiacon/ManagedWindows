using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Windows;

namespace WindowsTree {
    class Program {
        static void Main(string[] args) {
            EnumWindows(null, -2);
        }

        static void EnumWindows(NativeWindow root, int indent) {
            if (root != null) {
                Console.Write(new string(' ', indent));
                Console.WriteLine($"H: {root.Handle}  Visible: {root.IsVisible} thread: {root.ThreadId} Style: [{root.Style}] ExStyle: [{root.ExtendedStyle}] Text: {root.Text}");
            }
            if (root == null) {
                var windows = NativeWindow.EnumWindows();
                foreach (var window in windows) {
                    EnumWindows(window, indent + 2);
                }
            }
            else {
                var windows = root.EnumChildWindows();
                foreach (var window in windows)
                    EnumWindows(window, indent + 2);
            }
        }
    }
}
