using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Processes;

namespace MemMap {
    class Program {
        static void Main(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine("Usage: memmap <pid>");
                return;
            }

            foreach (var region in new MemoryMap(int.Parse(args[0]))) {
                Console.WriteLine(region);
            }
        }
    }
}
