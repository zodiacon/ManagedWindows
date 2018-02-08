using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;

namespace ProcessThreadList {
    class Program {
        static void Main(string[] args) {
            var info = SystemInformation.EnumProcessesExtended();
            Console.WriteLine($"Total Processes {info.Count}");

            foreach (var process in info) {
                Console.WriteLine($"PID: {process.ProcessId} Parent: {process.ParentProcessId} Threads: {process.ThreadCount} Image: {process.ImageName}"
                    + $" Base Pri: {process.BasePriority} Created: {process.CreateTime.ToLocalTime()} Kernel: {process.KernelTime} User: {process.UserTime}");

                if (process.Threads != null)
                    foreach (var thread in process.Threads) {
                        Console.WriteLine($"  Id: {thread.ThreadId} Created: {thread.CreateTime.ToLocalTime()} Pri: {thread.Priority} BasePri: {thread.BasePriority} "
                           + $"User: {thread.UserTime} Kernel: {thread.KernelTime} CtxSwitch: {thread.ContextSwitches} Address: {thread.StartAddress} State: {thread.State}"
                           + $" Wait Reason: {thread.WaitReason} Wait time: {thread.WaitTime}");
                    }
            }
        }
    }
}
