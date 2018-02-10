using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class ProcessInformation {
        internal ProcessInformation() { }

        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public int Threads { get; internal set; }
        public int ParentId { get; internal set; }
    }
}
