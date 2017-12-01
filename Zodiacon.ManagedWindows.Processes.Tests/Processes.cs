using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zodiacon.ManagedWindows.Processes;

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

        [TestMethod]
        public void TestMemoryRegions() {
            var explorer = NativeProcess.EnumProcesses().First(pi => pi.Name.ToLower() == "explorer.exe");
            var memoryMap = new MemoryMap(explorer.Id);
            foreach(var region in memoryMap)
                Console.WriteLine(region);
        }
    }
}
