using ManagedWindows.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedWindows;
using System.Threading;
using static ManagedWindows.Processes.Kernel32;
using static ManagedWindows.Processes.User32;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace ManagedWindows.Processes {
    public sealed class NativeProcess : WaitHandle, IEquatable<NativeProcess> {
        int _id;

        public event EventHandler ProcessExited;

        public static NativeProcess FromHandle(IntPtr hProcess) {
            return new NativeProcess(hProcess);
        }

        public static NativeProcess Open(ProcessAccessMask accessMask, int pid, bool inheritHandle = false) {
            return new NativeProcess(pid, accessMask, inheritHandle);
        }

        public int Id => _id == 0 ? (_id = GetProcessId(SafeWaitHandle)) : _id;

        private NativeProcess(IntPtr hProcess) {
            SafeWaitHandle = new SafeWaitHandle(hProcess, true);
        }

        private NativeProcess(int pid, ProcessAccessMask accessMask = ProcessAccessMask.QueryLimitedInformation, bool inheritHandle = false) {
            _id = pid;
            if (pid > 0)
                SafeWaitHandle = OpenProcess(accessMask, inheritHandle, pid);
        }

        public void RegisterForExit() {
            ThreadPool.RegisterWaitForSingleObject(this, (state, timeout) => {
                OnProcessExited();
            }, null, Timeout.Infinite, true);
        }

        public bool IsInJob(NativeJob job) {
            IsProcessInJob(SafeWaitHandle, job.SafeWaitHandle, out var inJob).ThrowIfWin32Failed();
            return inJob;
        }

        public string FullImageName {
            get {
                var name = new StringBuilder(300);
                int size = name.Capacity;
                QueryFullProcessImageName(SafeWaitHandle, ImageNameType.Normal, name, ref size).ThrowIfWin32Failed();
                return name.ToString();
            }
        }

        public bool IsRunning => !WaitOne(0);
        public bool IsTerminated => !IsRunning;

        bool GetProcessTimes(out long start, out long exit, out long kernel, out long user) => 
            Kernel32.GetProcessTimes(SafeWaitHandle, out start, out exit, out kernel, out user);

        public DateTime StartTime {
            get {
                if (SafeWaitHandle.IsInvalid)
                    throw new InvalidOperationException();
                GetProcessTimes(out long start, out long d, out d, out d);
                return new DateTime(start);
            }
        }

        public DateTime ExitTime {
            get {
                GetProcessTimes(out long d, out long end, out d, out d);
                return new DateTime(end);
            }
        }

        public TimeSpan KernelTime {
            get {
                GetProcessTimes(out long d, out d, out long kernel, out d);
                return new TimeSpan(kernel);
            }
        }

        public TimeSpan UserTime {
            get {
                GetProcessTimes(out long d, out d, out d, out long user);
                return new TimeSpan(user);
            }
        }

        public TimeSpan TotalTime {
            get {
                GetProcessTimes(out long d, out d, out long kernel, out long user);
                return new TimeSpan(user + kernel);
            }
        }

        public void Terminate(uint exitCode = 0) {
            using (var handle = OpenProcessHandle(ProcessAccessMask.Terminate)) {
                TerminateProcess(handle, exitCode).ThrowIfWin32Failed();
            }
        }

        public ProcessPriorityClass PriorityClass {
            get => GetPriorityClass(SafeWaitHandle).ThrowIfZero();
            set => SetPriorityClass(SafeWaitHandle, value).ThrowIfWin32Failed();
        }

        private SafeWaitHandle OpenProcessHandle(ProcessAccessMask accessMask) {
            return OpenProcess(accessMask, false, Id).ThrowIfFailed();
        }

        void OnProcessExited() {
            ProcessExited?.Invoke(this, EventArgs.Empty);
        }

        public static ProcessInfo[] EnumProcesses() {
            using (var handle = CreateToolhelp32Snapshot(CreateToolhelpSnapshotFlags.SnapProcess)) {
                if (handle == null)
                    return null;

                var processes = new List<ProcessInfo>(128);
                var pe = new ProcessEntry();
                pe.Init();

                if (!Process32First(handle, ref pe))
                    return null;

                do {
                    processes.Add(new ProcessInfo {
                        Id = pe.th32ProcessID,
                        ParentId = pe.th32ParentProcessID,
                        Threads = pe.cntThreads,
                        Name = pe.szExeFile
                    });
                } while (Process32Next(handle, ref pe));

                return processes.ToArray();
            }
        }

        public ModuleInfo[] EnumModules() {
            return EnumModules(Id);
        }

        public static ModuleInfo[] EnumModules(int pid) {
            using (var handle = CreateToolhelp32Snapshot(CreateToolhelpSnapshotFlags.SnapModules | 
                (Environment.Is64BitProcess ? CreateToolhelpSnapshotFlags.SnapModules32 : CreateToolhelpSnapshotFlags.None), pid)) {
                if (handle == null)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var modules = new List<ModuleInfo>(128);
                var me = new ModuleEntry();
                me.Init();

                if (!Module32First(handle, ref me))
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
                } while (Module32Next(handle, ref me));

                return modules.ToArray();
            }
        }

        public bool IsImmersive => IsImmersiveProcess(SafeWaitHandle);

        public ProcessMemoryCounters GetMemoryCounters() => 
            GetProcessMemoryInfo(SafeWaitHandle, out var counters, Marshal.SizeOf<ProcessMemoryCounters>())
                ? counters : throw new Win32Exception(Marshal.GetLastWin32Error());

        public bool Equals(NativeProcess other) {
            if (other == null)
                return false;

            return Id == other.Id && StartTime == other.StartTime;
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;
            return Equals((NativeProcess)obj);
        }

        public override int GetHashCode() {
            return Id ^ StartTime.GetHashCode();
        }
    }
}
