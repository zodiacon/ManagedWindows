using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class HeapInfo {
        readonly HeapList _heap;
        readonly int _pid;

        internal HeapInfo(HeapList heap, int pid) {
            _heap = heap;
            _pid = pid;
        }

        public bool IsDefaultHeap => _heap.Flags.HasFlag(HeapFlags.DefaultHeap);
        public bool IsSharedHeap => _heap.Flags.HasFlag(HeapFlags.SharedHeap);
        public HeapFlags HeapFlags => _heap.Flags;

        public long HeapId => _heap.HeapId.ToInt64();


        public IEnumerable<HeapBlock> EnumHeapBlocks() {
            var entry = new HeapEntry();
            entry.Init();

            if (!Win32.Heap32First(ref entry, _pid, _heap.HeapId))
                yield break;

            do {
                yield return new HeapBlock(entry);
            } while (Win32.Heap32Next(ref entry));

        }
    }
}
