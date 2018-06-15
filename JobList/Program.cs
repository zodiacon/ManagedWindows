using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;

namespace JobList {
    sealed class HandleComparer : IEqualityComparer<SystemHandleInformation> {
        public bool Equals(SystemHandleInformation x, SystemHandleInformation y) {
            return x.Object == y.Object;
        }

        public int GetHashCode(SystemHandleInformation obj) {
            return obj.Object.GetHashCode();
        }
    }

    static class Program {
        static void Main(string[] args) {
            var types = SystemInformation.EnumObjectTypes();
            var job = types.First(t => t.Name == "Job").Index;
            var jobs = SystemInformation.EnumHandles().Where(h => h.ObjectTypeIndex == job).Distinct(new HandleComparer());
            foreach (var handle in jobs) {
                Console.WriteLine($"0x{handle.Handle:X4} {handle.ProcessId,7} 0x{handle.Object:X}");
            }
        }
    }
}
