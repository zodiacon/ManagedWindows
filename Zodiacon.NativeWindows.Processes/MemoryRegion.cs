using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Processes {
    [DebuggerDisplay("0x{StartAddress:X} Size: 0x{Size:X} {Protect}")]
    public sealed class MemoryRegion {
        public long StartAddress { get; internal set; }
        public long AllocationBase { get; internal set; }
        public long Size { get; internal set; }
        public PageProtection Protect { get; internal set; }
        public PageProtection AllocateProtect { get; internal set; }
        public PageType Type { get; internal set; }
        public PageState State { get; internal set; }

        public override string ToString() => $"{StartAddress,16:X} Size: {Size,16:X} {Protect,-20} {Type,-8} {State}";
    }
}
