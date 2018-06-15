using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Devices {

    [StructLayout(LayoutKind.Sequential)]
    struct SP_DEVINFO_DATA {
        int cbSize;
        public Guid ClassGuid;
        public uint DevInst;    // DEVINST handle
        IntPtr Reserved;

        public static SP_DEVINFO_DATA Create() {
            var obj = new SP_DEVINFO_DATA {
                cbSize = Marshal.SizeOf<SP_DEVINFO_DATA>()
            };
            return obj;
        }
    }

    enum DeviceInterfaceDataFlags {
        None = 0,
        Active = 1,
        Default = 2,
        Removed = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    struct SP_DEVICE_INTERFACE_DATA {
        int cbSize;
        public Guid InterfaceClassGuid;
        public DeviceInterfaceDataFlags Flags;
        IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    unsafe struct SP_DEVICE_INTERFACE_DETAIL_DATA {
        int cbSize;
        public fixed char DevicePath[1];
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    unsafe struct SP_DEVINFO_LIST_DETAIL_DATA {
        int cbSize;
        public Guid ClassGuid;
        IntPtr RemoteMachineHandle;
        public fixed char RemoteMachineName[259];
    }

    static class SetupApi {
        [DllImport("setupapi", CharSet = CharSet.Unicode)]
        unsafe public static extern bool SetupDiDeleteDeviceInfo(IntPtr DeviceInfoSet, SP_DEVINFO_DATA* DeviceInfoData);

        [DllImport("setupapi", CharSet = CharSet.Unicode)]
        public static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint index, out SP_DEVINFO_DATA DeviceInfoData);

    }
}
