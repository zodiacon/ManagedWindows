using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Processes {
    [DebuggerDisplay("A: 0x{StartAddress:X} Size: 0x{Size:X} {Protect}")]
    public sealed class MemoryRegion {
        public long StartAddress { get; set; }
        public long Size { get; set; }
        public PageProtection Protect { get; set; }
        public PageProtection AllocateProtect { get; set; }
        public PageType Type { get; set; }
        public PageState State { get; set; }

        public override string ToString() => $"{StartAddress,16:X} Size: {Size,16:X} {Protect,-20} {Type,-8} {State}";
    }
}
