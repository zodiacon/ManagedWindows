using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class SystemInfo {
        readonly SYSTEM_INFO _info;
        private SystemInfo(SYSTEM_INFO info) {
            _info = info;
        }

        public static SystemInfo GetSystemInfo() {
            Win32.GetSystemInfo(out var si);
            return new SystemInfo(si);
        }

        public static SystemInfo GetNativeSystemInfo() {
            Win32.GetNativeSystemInfo(out var si);
            return new SystemInfo(si);
        }

        public ProcessorArchitecture ProcessorArchitecture => _info.ProcessorArchitecture;
        public long MinimumApplicationAddress => _info.MinimumApplicationAddress.ToInt64();
        public long MaximumApplicationAddress => _info.MaximumApplicationAddress.ToInt64();
        public ulong ActiveProcessorMask => _info.ActiveProcessorMask.ToUInt64();
        public uint NumberOfProcessors => _info.NumberOfProcessors;
        public int PageSize => _info.PageSize;
    }
}
