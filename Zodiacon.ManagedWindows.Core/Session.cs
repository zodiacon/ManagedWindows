using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class Session {
        internal Session() {
        }

        public int Id { get; internal set; }

        public SessionConnectionState State { get; internal set; }
        public string Name { get; internal set; }
        public string UserName { get; internal set; }
    }
}
