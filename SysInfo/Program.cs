using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;

namespace SysInfo {
    class Program {
        static void Main(string[] args) {
            var si = SystemInfo.GetNativeSystemInfo();
            Console.WriteLine($"Processor Architecture: {si.ProcessorArchitecture}");
            Console.WriteLine($"Page Size: {si.PageSize >> 10} KB");
            Console.WriteLine($"Minimum Application Address: 0x{si.MinimumApplicationAddress:X}");
            Console.WriteLine($"Maximum Application Address: 0x{si.MaximumApplicationAddress:X}");
            Console.WriteLine($"Active Processor Mask: 0x{si.ActiveProcessorMask:X}");
            Console.WriteLine($"Number of Processors: {si.NumberOfProcessors}");
        }
    }
}
