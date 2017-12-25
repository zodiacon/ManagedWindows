using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class PerformanceInformation {
        PERFORMANCE_INFORMATION _info;
        internal PerformanceInformation() {
            Refresh();    
        }

        public void Refresh() {
            Win32.GetPerformanceInfo(out _info, Marshal.SizeOf<PERFORMANCE_INFORMATION>()).ThrowIfWin32Failed();
        }

        public uint HandleCount => _info.HandleCount;
        public uint ProcessCount => _info.ProcessCount;
        public uint ThreadCount => _info.ThreadCount;

        public long CommitTotal => _info.CommitTotal.ToInt64();
        public long CommitLimit => _info.CommitLimit.ToInt64();
        public long CommitPeak => _info.CommitPeak.ToInt64();
        public long PhysicalTotal => _info.PhysicalTotal.ToInt64() * (long)_info.PageSize;
        public long PhysicalAvailable => _info.PhysicalAvailable.ToInt64() * (long)_info.PageSize;
        public long SystemCache => _info.SystemCache.ToInt64() * (long)_info.PageSize;
        public long KernelTotal => _info.KernelTotal.ToInt64() * (long)_info.PageSize;
        public long KernelPaged => _info.KernelPaged.ToInt64() * (long)_info.PageSize;
        public long KernelNonPaged => _info.KernelNonpaged.ToInt64() * (long)_info.PageSize;
        public int PageSize => _info.PageSize.ToInt32();
    }
}
