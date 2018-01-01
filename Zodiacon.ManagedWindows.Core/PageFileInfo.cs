using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class PageFileInfo {
        readonly EnumPageFileInformation _info;
        internal PageFileInfo(ref EnumPageFileInformation info, string filename) {
            _info = info;
            FileName = filename;
        }

        public long TotalSize => _info.TotalSize.ToInt64() * Environment.SystemPageSize;
        public long TotalInUse => _info.TotalInUse.ToInt64() * Environment.SystemPageSize;
        public long PeakUsage => _info.PeakUsage.ToInt64() * Environment.SystemPageSize;
        public string FileName { get; }
    }
}
