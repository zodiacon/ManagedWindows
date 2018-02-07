using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class SystemModuleInfo {
        unsafe internal SystemModuleInfo(RTL_PROCESS_MODULE_INFORMATION_EX* info) {
            FullPath = new string((sbyte*)info->BaseInfo.FullPathName);
            ImageBase = info->BaseInfo.ImageBase;
            ImageSize = info->BaseInfo.ImageSize;
            Checksum = info->ImageChecksum;
        }

        public string FullPath { get; }
        public IntPtr ImageBase { get; }
        public uint ImageSize { get; }
        public uint Checksum { get; }
    }
}
