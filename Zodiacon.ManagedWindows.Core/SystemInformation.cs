using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public static class SystemInformation {
        public static ProcessInfo[] EnumProcesses() {
            using (var handle = Win32.CreateToolhelp32Snapshot(CreateToolhelpSnapshotFlags.SnapProcess)) {
                if (handle.DangerousGetHandle() == Win32.InvalidFileHandle)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var processes = new List<ProcessInfo>(128);
                var pe = new ProcessEntry();
                pe.Init();

                if (!Win32.Process32First(handle, ref pe))
                    return null;

                do {
                    processes.Add(new ProcessInfo {
                        Id = pe.th32ProcessID,
                        ParentId = pe.th32ParentProcessID,
                        Threads = pe.cntThreads,
                        Name = pe.szExeFile
                    });
                } while (Win32.Process32Next(handle, ref pe));

                return processes.ToArray();
            }
        }

        public static ThreadInfo[] EnumThreads() {
            using (var handle = Win32.CreateToolhelp32Snapshot(CreateToolhelpSnapshotFlags.SnapThread)) {
                if (handle.DangerousGetHandle() == Win32.InvalidFileHandle)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var threads = new List<ThreadInfo>(1024);
                var te = new ThreadEntry();
                te.Init();

                if (!Win32.Thread32First(handle, ref te))
                    return null;

                do {
                    threads.Add(new ThreadInfo(te));
                } while (Win32.Thread32Next(handle, ref te));

                return threads.ToArray();
            }
        }

        public static ModuleInfo[] EnumModules(int pid) {
            using (var handle = Win32.CreateToolhelp32Snapshot(CreateToolhelpSnapshotFlags.SnapModules |
                (Environment.Is64BitProcess ? CreateToolhelpSnapshotFlags.SnapModules32 : CreateToolhelpSnapshotFlags.None), pid)) {
                if (handle.DangerousGetHandle() == Win32.InvalidFileHandle)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var modules = new List<ModuleInfo>(128);
                var me = new ModuleEntry();
                me.Init();

                if (!Win32.Module32First(handle, ref me))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                do {
                    modules.Add(new ModuleInfo {
                        Pid = pid,
                        Name = me.szModule,
                        FullPath = me.szExePath,
                        BaseAddress = me.modBaseAddr,
                        Size = me.modBaseSize,
                        Handle = me.hModule
                    });
                } while (Win32.Module32Next(handle, ref me));

                return modules.ToArray();
            }
        }

        public static HeapInfo[] EnumHeaps(int pid) {
            using (var handle = Win32.CreateToolhelp32Snapshot(CreateToolhelpSnapshotFlags.SnapHeapList, pid)) {
                if (handle.DangerousGetHandle() == Win32.InvalidFileHandle)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var heaps = new List<HeapInfo>(8);
                var hi = new HeapList();
                hi.Init();

                if (!Win32.Heap32ListFirst(handle, ref hi))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                do {
                    var heapInfo = new HeapInfo(hi, pid);
                    heaps.Add(heapInfo);
                } while (Win32.Heap32ListNext(handle, ref hi));

                return heaps.ToArray();
            }
        }

        public static long PerformanceCounter => Win32.QueryPerformanceCunter(out var counter) ? counter : 0;

        public static long PerformanceFrequency => Win32.QueryPerformanceFrequency(out var freq) ? freq : 0;

        public static FirmwareType FirmwareType => Win32.GetFirmwareType(out var type) ? type : FirmwareType.Unknown;

        public static PerformanceInformation GetPerformanceInformation() {
            return new PerformanceInformation();
        }

        public static SystemInfo GetSystemInfo() {
            return SystemInfo.GetSystemInfo();
        }

        public static SystemInfo GetNativeSystemInfo() {
            return SystemInfo.GetNativeSystemInfo();
        }

    }
}
