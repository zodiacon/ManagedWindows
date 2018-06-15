using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;
using Zodiacon.ManagedWindows.Processes;

namespace HandleList {
    class Program {
        static void Main(string[] args) {
            if (args.Length == 0) {
                // all handles in the system
                var handles = SystemInformation.EnumHandles();
                Console.WriteLine($"Total handles: {handles.Length}");
                foreach (var handle in handles) {
                    Console.WriteLine($"H: 0x{handle.Handle:X4}\tPID: {handle.ProcessId}\tObject: 0x{handle.Object.ToUInt64():X}\tType: {handle.ObjectTypeIndex,2} Access: 0x{handle.AccessMask:X8}");
                }
            }
            else {
                int pid = int.Parse(args[0]);

                using (var process = NativeProcess.Open(ProcessAccessMask.QueryInformation, pid)) {
                    var handles = process.EnumHandles();
                    Console.WriteLine($"Total handles: {handles.Length}");
                    foreach (var handle in handles) {
                        Console.WriteLine($"H: 0x{handle.Handle:X4}\t Count: {handle.HandleCount}\tType: {handle.ObjectTypeIndex,2}\tPointer Count: 0x{handle.PointerCount:X}");
                    }
                }
            }
        }
    }
}
