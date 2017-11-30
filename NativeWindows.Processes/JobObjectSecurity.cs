using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace Zodiacon.ManagedWindows.Processes {
    [Flags]
    public enum JobObjectRights {
    }

    public sealed class JobObjectSecurity : ObjectSecurity<JobObjectRights> {
        public JobObjectSecurity(bool isContainer, ResourceType resourceType) : base(isContainer, resourceType) {
        }

        public JobObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections) : base(isContainer, resourceType, name, includeSections) {
        }

        public  JobObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle safeHandle, AccessControlSections includeSections) : base(isContainer, resourceType, safeHandle, includeSections) {
        }

    }
}