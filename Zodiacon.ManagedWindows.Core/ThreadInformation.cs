using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class ThreadInformation {
        readonly ThreadEntry _entry;

        internal ThreadInformation(ThreadEntry entry) {
            _entry = entry;
        }

        public int Id => _entry.ThreadId;
        public int ProcessId => _entry.ProcessId;
        public int BasePriority => _entry.BasePriority;
    }
}
