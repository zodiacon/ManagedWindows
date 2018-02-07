using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows;
using System.Threading;
using static Zodiacon.ManagedWindows.Processes.Kernel32;
using static Zodiacon.ManagedWindows.Processes.User32;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using Zodiacon.ManagedWindows.Core;

namespace Zodiacon.ManagedWindows.Processes {
    public sealed class NativeProcess : WaitHandle, IEquatable<NativeProcess> {
        int _id;

        public event EventHandler ProcessExited;

        public static NativeProcess FromHandle(IntPtr hProcess, bool ownsHandle = true) {
            return new NativeProcess(hProcess, ownsHandle);
        }

        public static NativeProcess Open(ProcessAccessMask accessMask, int pid, bool inheritHandle = false) {
            return new NativeProcess(pid, accessMask, inheritHandle);
        }

        public static NativeProcess TryOpen(ProcessAccessMask accessMask, int pid, bool inheritHandle = false) {
            var handle = OpenProcess(accessMask, inheritHandle, pid);
            if (handle == null || handle.IsInvalid)
                return null;

            return new NativeProcess(handle);
        }

        public int Id => _id == 0 ? (_id = GetProcessId(SafeWaitHandle)) : _id;

        private NativeProcess(IntPtr hProcess, bool ownsHandle) {
            SafeWaitHandle = new SafeWaitHandle(hProcess, ownsHandle);
        }

        private NativeProcess(SafeWaitHandle hProcess) {
            SafeWaitHandle = hProcess;
        }

        private NativeProcess(int pid, ProcessAccessMask accessMask = ProcessAccessMask.QueryLimitedInformation, bool inheritHandle = false) {
            _id = pid;
            if (pid > 0) {
                SafeWaitHandle = OpenProcess(accessMask, inheritHandle, pid);
                if (SafeWaitHandle.IsInvalid)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
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

        public string FullImageName => TryGetFullImageName(ImageNameType.Normal) ?? throw new Win32Exception(Marshal.GetLastWin32Error());

        public string TryGetFullImageName(ImageNameType type = ImageNameType.Normal) {
            var name = new StringBuilder(300);
            int size = name.Capacity;
            QueryFullProcessImageName(SafeWaitHandle, type, name, ref size);
            return name.ToString();
        }

        public bool IsRunning => !WaitOne(0);
        public bool IsTerminated => !IsRunning;

        bool GetProcessTimes(out long start, out long exit, out long kernel, out long user) =>
            Kernel32.GetProcessTimes(SafeWaitHandle, out start, out exit, out kernel, out user);

        public DateTime? CreateTime {
            get {
                if (SafeWaitHandle.IsInvalid)
                    throw new InvalidOperationException();
                return GetProcessTimes(out long start, out long d, out d, out d) ? new DateTime(start) : default;
            }
        }

        public DateTime ExitTime {
            get {
                GetProcessTimes(out long d, out long end, out d, out d);
                return new DateTime(end);
            }
        }

        public TimeSpan? KernelTime => GetProcessTimes(out long d, out d, out long kernel, out d) ? new TimeSpan(kernel) : default;

        public TimeSpan? UserTime => GetProcessTimes(out long d, out d, out d, out long user) ? new TimeSpan(user) : default;

        public TimeSpan? TotalTime => GetProcessTimes(out long d, out d, out long kernel, out long user) ? new TimeSpan(user + kernel) : default;

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

        void OnProcessExited() => ProcessExited?.Invoke(this, EventArgs.Empty);

        public int SessionId => ProcessIdToSessionId(Id, out var sessionId) ? sessionId : -1;

        public bool IsManaged => EnumModules()?.FirstOrDefault(module => module.Name.Equals("mscoree.dll", StringComparison.InvariantCultureIgnoreCase)) != null;

        public bool IsInAnyJob => IsProcessInJob(SafeWaitHandle, IntPtr.Zero, out var injob) && injob;

        public unsafe ProtectionLevel Protection {
            get {
                var level = ProtectionLevel.None;
                NtDll.NtQueryInformationProcess(SafeWaitHandle, ProcessInformationClass.ProtectionInformation, &level, sizeof(ProtectionLevel));
                return level;
            }
        }

        public bool IsProtected => Protection != ProtectionLevel.None;

        public ModuleInfo[] EnumModules() {
            return SystemInformation.EnumModules(Id);
        }

        public bool IsImmersive => IsImmersiveProcess(SafeWaitHandle);

        public ProcessMemoryCounters GetMemoryCounters() =>
            GetProcessMemoryInfo(SafeWaitHandle, out var counters, Marshal.SizeOf<ProcessMemoryCounters>())
                ? counters : throw new Win32Exception(Marshal.GetLastWin32Error());

        public bool Equals(NativeProcess other) {
            if (other == null)
                return false;

            return Id == other.Id && CreateTime == other.CreateTime;
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;
            return Equals((NativeProcess)obj);
        }

        public override int GetHashCode() {
            return Id ^ CreateTime.GetHashCode();
        }

        public bool IsBeingDebugged => CheckRemoteDebuggerPresent(SafeWaitHandle, out var debugged) ? debugged : throw new Win32Exception(Marshal.GetLastWin32Error());

        public void BreakInto() {
            using (var handle = SafeWaitHandle.Duplicate((uint)ProcessAccessMask.SetInformation)) {
                DebugBreakProcess(handle);
            }
        }

        public unsafe int ParentProcessId {
            get {
                PROCESS_BASIC_INFORMATION info;
                if (NtDll.NtQueryInformationProcess(SafeWaitHandle, ProcessInformationClass.BasicInformation, &info, sizeof(PROCESS_BASIC_INFORMATION)) < 0)
                    return 0;
                return info.ParentProcessId.ToInt32();
            }
        }

        public long ReadMemory(IntPtr address, IntPtr buffer, long size, bool requestHandle = true) {
            bool ok = ReadProcessMemory(SafeWaitHandle, address, buffer, new IntPtr(size), out var bytesRead);
            if (!ok)
                return 0;
            return bytesRead.ToInt64();
        }

        public bool Is64Bit {
            get {
                if (!Environment.Is64BitOperatingSystem)
                    return false;
                IsWow64Process(SafeWaitHandle, out bool wow64).ThrowIfWin32Failed();
                return !wow64;
            }
        }

        public unsafe string CommandLine {
            get {
                var buffer = Marshal.AllocHGlobal(1 << 16);
                if (NtDll.NtQueryInformationProcess(SafeWaitHandle, ProcessInformationClass.CommandLineInformation, buffer.ToPointer(), 1 << 16) < 0)
                    return null;

                var commandLine = new string(((UNICODE_STRING*)buffer.ToPointer())->Buffer);
                Marshal.FreeHGlobal(buffer);
                return commandLine;
            }
        }

        public unsafe HandleInfo[] EnumHandles() {
            var size = 1 << 17;
            int status;
            IntPtr buffer = default;

            try {
                do {
                    buffer = Marshal.AllocHGlobal(size);
                    int actualSize;
                    status = NtDll.NtQueryInformationProcess(SafeWaitHandle, ProcessInformationClass.HandleInformation, buffer.ToPointer(), size, &actualSize);
                    if (status == NtDll.StatusInfoLengthMismatch) {
                        buffer = Marshal.ReAllocHGlobal(buffer, new IntPtr(size = actualSize + (1 << 10)));
                        continue;
                    }
                    break;
                } while (true);
                if (status < 0)
                    throw new Win32Exception(status, "Failed to get handle list");

                var info = (PROCESS_HANDLE_SNAPSHOT_INFORMATION*)buffer.ToPointer();
                var count = info->NumberOfHandles.ToInt32();
                var handles = (PROCESS_HANDLE_TABLE_ENTRY_INFO*)((byte*)info + sizeof(PROCESS_HANDLE_SNAPSHOT_INFORMATION));
                var result = new HandleInfo[count];
                for (uint i = 0; i < count; ++i) {
                    result[i] = new HandleInfo(&handles[i]);
                }
                return result;
            }
            finally {
                Marshal.FreeHGlobal(buffer);
            }
        }

    }
}
