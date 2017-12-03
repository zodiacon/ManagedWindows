using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Processes {
    public sealed class MemoryMap : IEnumerable<MemoryRegion>, IDisposable {
        SafeWaitHandle _hProcess;
        bool _owner;

        public MemoryMap(int pid) {
            _hProcess = Kernel32.OpenProcess(ProcessAccessMask.QueryInformation, false, pid);
            if (_hProcess.IsInvalid)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            _owner = true;
        }

        public MemoryMap(SafeWaitHandle hProcess) {
            _hProcess = hProcess;
        }

        public void Dispose() {
            if (_owner)
                _hProcess.Dispose();
        }

        public IEnumerator<MemoryRegion> GetEnumerator() {
            long address = 0;
            while (true) {
                if (Kernel32.VirtualQueryEx(_hProcess, new IntPtr(address), out var mbi, Marshal.SizeOf<MemoryBasicInformation>()) == IntPtr.Zero)
                    break;

                yield return new MemoryRegion {
                    StartAddress = mbi.BaseAddress.ToInt64(),
                    Size = mbi.RegionSize.ToInt64(),
                    Protect = mbi.Protect,
                    AllocateProtect = mbi.AllocationProtect,
                    Type = mbi.Type,
                    State = mbi.State,
                    AllocationBase = mbi.AllocationBase.ToInt64()
                };
                address += mbi.RegionSize.ToInt64();
            }
        }

        public IEnumerable<MemoryRegion> MemoryRegions => this;

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
