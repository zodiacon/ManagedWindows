using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;

namespace Zodiacon.ManagedWindows.Processes {
    public sealed class HandleInformation {
        unsafe internal HandleInformation(PROCESS_HANDLE_TABLE_ENTRY_INFO* info) {
            Handle = info->HandleValue.ToInt64();
            HandleCount = info->HandleCount.ToInt32();
            ObjectTypeIndex = info->ObjectTypeIndex;
            AccessMask = info->GrantedAccess;
            PointerCount = info->PointerCount.ToUInt32();
            Attributes = info->HandleAttributes;
        }

        public long Handle { get; }
        public int HandleCount { get; }
        public uint ObjectTypeIndex { get; }
        public uint AccessMask { get;}
        public uint PointerCount { get; }
        public HandleAttributes Attributes { get; }
    }
}
