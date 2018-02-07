using System;
using Zodiacon.ManagedWindows.Core;

namespace ModuleList {
    class Program {
        static void Main(string[] args) {
            if (args.Length < 1) {
                EnumSystemModules();
                return;
            }

            if (!int.TryParse(args[0], out var pid)) {
                Usage();
                return;
            }

            try {
                foreach (var module in SystemInformation.EnumModules(pid)) {
                    Console.WriteLine($"Base: 0x{module.BaseAddress.ToInt64():X} Size: 0x{module.Size:X} Path: {module.FullPath}");
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"{ex.Message}.");
            }
        }

        private static void EnumSystemModules() {
            var modules = SystemInformation.EnumSystemModules();
            foreach (var module in modules) {
                Console.WriteLine($"Base: 0x{module.ImageBase.ToInt64():X} Size: 0x{module.ImageSize:X} Path: {module.FullPath}");
            }
        }

        static void Usage() {
            Console.WriteLine("Usage: ModuleList <pid>");
        }
    }
}
