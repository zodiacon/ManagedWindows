using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ManagedWindows.Core {
    public enum GenericAccessRights : uint {
        Synchronize = 0x1000000,
        WriteDac = 0x40000,
        WriteOwner = 0x80000,
        Delete = 0x100000,
        ReadControl = 0x20000,
        StandardRightsRequired = 0xf0000
    }

}
