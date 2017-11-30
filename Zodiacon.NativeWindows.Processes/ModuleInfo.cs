using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Processes {
    public sealed class ModuleInfo {
        internal ModuleInfo() { }

        public string Name { get; internal set; }
        public string FullPath { get; internal set; }
        public IntPtr BaseAddress { get; internal set; }
        public IntPtr Handle { get; internal set; }
        public int Pid { get; internal set; }
        public uint Size { get; internal set; }
    }
}
