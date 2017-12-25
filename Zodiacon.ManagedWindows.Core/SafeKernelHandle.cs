using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class SafeKernelHandle : SafeHandleZeroOrMinusOneIsInvalid {
        public SafeKernelHandle(bool ownsHandle) : base(ownsHandle) {
        }

        protected override bool ReleaseHandle() => Win32.CloseHandle(handle);
    }
}
