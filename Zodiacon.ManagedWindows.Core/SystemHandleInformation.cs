using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class SystemHandleInformation : IEquatable<SystemHandleInformation> {
        unsafe internal SystemHandleInformation(SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX* info) {
            Object = info->Object;
            Handle = info->HandleValue.ToInt32();
            ObjectTypeIndex = info->ObjectTypeIndex;
            AccessMask = info->GrantedAccess;
            ProcessId = info->UniqueProcessId.ToInt32();
            Attributes = info->HandleAttributes;
        }

        public UIntPtr Object { get; }
        public int Handle { get; }
        public int ObjectTypeIndex { get; }
        public int ProcessId { get; }
        public uint AccessMask { get; }
        public uint Attributes { get; }

        public bool Equals(SystemHandleInformation other) => ProcessId == other.ProcessId && Handle == other.Handle;

        public override bool Equals(object obj) => Equals((SystemHandleInformation)obj);

        public override int GetHashCode() => ProcessId ^ Handle;
    }
}
