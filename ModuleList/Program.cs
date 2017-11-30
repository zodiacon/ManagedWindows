using ManagedWindows.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleList {
    class Program {
        static void Main(string[] args) {
            if (args.Length < 1) {
                Usage();
                return;
            }

            if (!int.TryParse(args[0], out var pid)) {
                Usage();
                return;
            }

            try {
                foreach (var module in NativeProcess.EnumModules(pid)) {
                    Console.WriteLine($"Base: 0x{module.BaseAddress.ToInt64():X} Size: 0x{module.Size:X} Path: {module.FullPath}");
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"{ex.Message}.");
            }
        }

        static void Usage() {
            Console.WriteLine("Usage: ModuleList <pid>");
        }
    }
}
