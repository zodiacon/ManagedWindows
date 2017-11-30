using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ManagedWindows.Processes.Tests {
    [TestClass]
    public class Processes {
        [TestMethod]
        public void TestEnumProcesses() {
            var processes = NativeProcess.EnumProcesses();
            foreach (var pi in processes) {
                Console.WriteLine($"{pi.Name} ({pi.Id}) Threads: {pi.Threads} Parent: {pi.ParentId}");
            }
        }

        [TestMethod]
        public void TestOpenProcess() {
            var explorer = NativeProcess.EnumProcesses().First(pi => pi.Name.ToLower() == "explorer.exe");
            var process = NativeProcess.Open(ProcessAccessMask.QueryInformation, explorer.Id);
            Console.WriteLine(process.StartTime);
        }
    }
}
