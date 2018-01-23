using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
                        Name = pe.th32ProcessID == 0 ? "[System Idle Process]" : pe.szExeFile
                    });
                } while (Win32.Process32Next(handle, ref pe));

                return processes.ToArray();
            }
        }

        public static ThreadInfo[] EnumThreads(int pid = -1) {
            using (var handle = Win32.CreateToolhelp32Snapshot(CreateToolhelpSnapshotFlags.SnapThread)) {
                if (handle.DangerousGetHandle() == Win32.InvalidFileHandle)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var threads = new List<ThreadInfo>(1024);
                var te = new ThreadEntry();
                te.Init();

                if (!Win32.Thread32First(handle, ref te))
                    return null;

                do {
                    if (pid >= 0 && te.ProcessId != pid)
                        continue;
                    if (te.ThreadId != 0)
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

        public static long PerformanceCounter => Win32.QueryPerformanceCounter(out var counter) ? counter : 0;

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

        public static PageFileInfo[] EnumPageFiles() {
            var pageFiles = new List<PageFileInfo>(4);

            Win32.EnumPageFiles((IntPtr context, ref EnumPageFileInformation info, string filename) => {
                pageFiles.Add(new PageFileInfo(ref info, filename));
                return true;
            }, IntPtr.Zero);

            return pageFiles.ToArray();
        }

        public static IntPtr[] EnumThreadWindows(int threadId) {
            var windows = new List<IntPtr>(64);

            Win32.EnumThreadWindows(threadId, (hWnd, param) => {
                windows.Add(hWnd);
                return true;
            }, IntPtr.Zero).ThrowIfWin32Failed();

            return windows.ToArray();
        }

        public static string[] EnumWindowStations() {
            var winStations = new List<string>(4);
            Win32.EnumWindowStations((name, param) => {
                winStations.Add(name);
                return true;
            }, IntPtr.Zero).ThrowIfWin32Failed();

            return winStations.ToArray();
        }

        public unsafe static Session[] EnumSessions() {
            var sessions = new List<Session>(4);
            int level = 1;
            Win32.WTSEnumerateSessionsEx(IntPtr.Zero, ref level, 0, out var info, out int count).ThrowIfWin32Failed();
            for (int i = 0; i < count; i++) {
                var session = new Session {
                    Id = info[i].SessionId,
                    State = info[i].State,
                    Name = new string(info[i].pSessionName),
                    UserName = new string(info[i].pUserName)
                };
                sessions.Add(session);
            }

            Win32.WTSFreeMemoryEx(WtsTypeClass.SessionInfoLevel1, info, count);

            return sessions.ToArray();
        }

        public static unsafe SystemHandleInfo[] EnumHandles() {
            IntPtr buffer = IntPtr.Zero;
            try {
                var size = 1 << 23;
                buffer = Marshal.AllocHGlobal(size);
                int actualSize;
                if (NtDll.NtQuerySystemInformation(SystemInformationClass.ExtendedHandleInformation, buffer, size, &actualSize) < 0)
                    return null;

                var info = (SYSTEM_HANDLE_INFORMATION_EX*)buffer.ToPointer();
                var count = info->HandleCount.ToInt32();
                Debug.Assert(count > 0);
                var handleInfo = (SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX*)((byte*)info + sizeof(SYSTEM_HANDLE_INFORMATION_EX));
                var handles = new SystemHandleInfo[count];
                for (uint i = 0; i < count; i++)
                    handles[i] = new SystemHandleInfo(&handleInfo[i]);
                return handles;
            }
            finally {
                if (buffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(buffer);
            }
        }

        public unsafe static IReadOnlyList<ProcessExtendedInformation> EnumProcessesAndThreads() {
            var size = 1 << 18;
            IntPtr buffer = IntPtr.Zero;
            try {
                do {
                    buffer = Marshal.AllocHGlobal(size);
                    int status = NtDll.NtQuerySystemInformation(SystemInformationClass.ProcessInformation, buffer, size);
                    if (status == unchecked((int)0xc0000004)) {  // buffer too small
                        buffer = Marshal.ReAllocHGlobal(buffer, new IntPtr(size *= 2));
                        continue;
                    }
                    if (status < 0) {
                        return null;
                    }
                    break;
                } while (true);

                var list = new List<ProcessExtendedInformation>(256);
                var process = (SYSTEM_PROCESS_INFORMATION64*)buffer.ToPointer();
                do {
                    list.Add(new ProcessExtendedInformation(process));
                    if (process->NextEntryOffset == 0)
                        break;
                    process = (SYSTEM_PROCESS_INFORMATION64*)((byte*)process + process->NextEntryOffset);
                } while (true);
                return list;
            }
            finally {
                if (buffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(buffer);
            }
        }
    }
}
