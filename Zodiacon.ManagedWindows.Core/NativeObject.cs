using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class NativeObject : IDisposable {
        IntPtr _handle;
        NativeObject(IntPtr handle) {
            if (handle == IntPtr.Zero)
                throw new ArgumentException("Invalid handle", nameof(handle));
            _handle = handle;
        }

        public static NativeObject FromHandle(IntPtr handle) {
            return new NativeObject(handle);
        }

        public void Dispose() {
            Win32.CloseHandle(_handle);
        }

        public unsafe string Name {
            get {
                var size = 1000;
                var buffer = Marshal.AllocHGlobal(size);
                try {
                    var status = NtDll.NtQueryObject(_handle, ObjectInformationClass.NameInformation, buffer.ToPointer(), size);
                    if (status < 0)
                        throw new Win32Exception(status);
                    var ustr = &((OBJECT_NAME_INFORMATION*)buffer.ToPointer())->Name;
                    return ustr->ToString();
                }
                finally {
                    Marshal.FreeHGlobal(buffer);
                }
            }
        }

        public unsafe NativeObjectType ObjectType {
            get {
                var size = 256;
                var buffer = Marshal.AllocHGlobal(size);
                try {
                    var status = NtDll.NtQueryObject(_handle, ObjectInformationClass.TypeInformation, buffer.ToPointer(), size);
                    if (status < 0)
                        throw new Win32Exception(status);
                    return NativeObjectType.FromTypeInformation((OBJECT_TYPE_INFORMATION*)buffer.ToPointer());
                }
                finally {
                    Marshal.FreeHGlobal(buffer);
                }
            }
        }
    }
}
