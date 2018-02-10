using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class GenericMapping {
        unsafe internal GenericMapping(GENERIC_MAPPING* mapping) {
            Read = mapping->GenericRead;
            Write = mapping->GenericWrite;
            Execute = mapping->GenericExecute;
            All = mapping->GenericAll;
        }

        public readonly uint Read, Write, Execute, All;
    }
}
