using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class HeapBlock {
        HeapEntry _entry;

        internal HeapBlock(HeapEntry entry) {
            _entry = entry;
        }

        public IntPtr HeapHandle => _entry.Handle;
        public long Address => _entry.Address.ToInt64();
        public long BlockSize => _entry.BlockSize.ToInt64();
        public HeapEntryFlags Flags => _entry.Flags;
        public int ProcessId => _entry.ProcessID;
    }
}
