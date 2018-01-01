using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;
using Zodiacon.ManagedWindows.Processes;

namespace ThreadList {
    class Program {
        static void Main(string[] args) {
            foreach (var thread in SystemInformation.EnumThreads()) {
                Console.Write($"TID={thread.Id}, PID={thread.ProcessId}");
                var nt = NativeThread.TryOpen(ThreadAccessMask.QueryInformation, thread.Id);
                if (nt != null) {
                    nt.GetStackLimits(out var min, out var max);
                    Console.Write($" Stack: 0x{min:X} 0x{max:X}");
                }
                Console.WriteLine();
            }
        }
    }
}
