using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;
using Zodiacon.ManagedWindows.Processes;

namespace ProcessList {
    class Program {
        static void Main(string[] args) {
            var processes = SystemInformation.EnumProcessesExtended();
            foreach (var pi in processes) {
                Console.Write($"{pi.ImageName} ({pi.ProcessId}) Threads: {pi.Threads} Parent: {pi.ParentProcessId} Virtual Size: {pi.VirtualSize}");
                if (pi.ProcessId > 0) {
                    using (var process = NativeProcess.TryOpen(ProcessAccessMask.QueryLimitedInformation, pi.ProcessId)) {
                        if (process != null) {
                            Console.Write($" Start: {process.CreateTime} Managed: {process.IsManaged} In Job: {process.IsInAnyJob} Protection: {process.Protection}");
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
