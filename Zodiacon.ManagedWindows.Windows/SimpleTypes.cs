using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Windows {
    [StructLayout(LayoutKind.Sequential)]
    public struct Point {
        public int X, Y;

        public Point(int x, int y) {
            X = x; Y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect {
        public int Left, Top, Right, Bottom;

        public Rect(int left, int top, int right, int bottom) {
            Left = left; Right = right; Top = top; Bottom = bottom;
        }

        public int Width => Right - Left;
        public int Height => Bottom - Top;

        public static readonly Rect Empty = new Rect();

        public Point TopLeft => new Point(Left, Top);
        public Point TopRight => new Point(Right, Top);
        public Point BottomRight => new Point(Right, Bottom);
        public Point BottomLeft => new Point(Left, Bottom);
        public Point CenterPoint => new Point(Left + Width / 2, Top + Height / 2);
    }
}
