using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;

namespace HeapList {
    class Program {
        static void Main(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine("Usage: HeapList <pid>");
                return;
            }

            try {
                foreach (var heap in SystemInformation.EnumHeaps(int.Parse(args[0]))) {
                    Console.WriteLine($"Heap {heap.HeapId} Default: {heap.IsDefaultHeap} Shared: {heap.IsSharedHeap}");
                    foreach (var block in heap.EnumHeapBlocks()) {
                        Console.WriteLine($"\tHandle: 0x{block.HeapHandle:X} Address: 0x{block.Address:X} Size: 0x{block.BlockSize:X} Flags: {block.Flags}");
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
