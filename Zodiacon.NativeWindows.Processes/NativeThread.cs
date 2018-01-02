using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;

namespace Zodiacon.ManagedWindows.Processes {
    public sealed class NativeThread : WaitHandle {
        private NativeThread(SafeWaitHandle handle) {
            SafeWaitHandle = handle;
        }

        public static NativeThread Open(ThreadAccessMask accessMask, int tid, bool inheritHandle = false) {
            var thread = TryOpen(accessMask, tid, inheritHandle);
            if (thread == null)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return thread;
        }

        public static NativeThread TryOpen(ThreadAccessMask accessMask, int tid, bool inheritHandle = false) {
            var handle = Kernel32.OpenThread(accessMask, inheritHandle, tid);
            if (handle == null || handle.IsInvalid)
                return null;
            return new NativeThread(handle);
        }

        public static NativeThread FromHandle(IntPtr handle, bool owner = true) {
            return new NativeThread(new SafeWaitHandle(handle, owner));
        }

        public static int CurrentThreadId => Kernel32.GetCurrentThreadId();

        public int Id => Kernel32.GetThreadId(SafeWaitHandle);
        public int ProcessId => Kernel32.GetProcessIdOfThread(SafeWaitHandle);

        bool GetThreadTimes(out long start, out long exit, out long kernel, out long user) =>
            Kernel32.GetThreadTimes(SafeWaitHandle, out start, out exit, out kernel, out user);

        public DateTime CreateTime {
            get {
                if (SafeWaitHandle.IsInvalid)
                    throw new InvalidOperationException();
                GetThreadTimes(out long start, out long d, out d, out d);
                return new DateTime(start);
            }
        }

        public DateTime ExitTime {
            get {
                GetThreadTimes(out long d, out long end, out d, out d);
                return new DateTime(end);
            }
        }

        public TimeSpan KernelTime {
            get {
                GetThreadTimes(out long d, out d, out long kernel, out d);
                return new TimeSpan(kernel);
            }
        }

        public TimeSpan UserTime {
            get {
                GetThreadTimes(out long d, out d, out d, out long user);
                return new TimeSpan(user);
            }
        }

        public TimeSpan TotalTime {
            get {
                GetThreadTimes(out long d, out d, out long kernel, out long user);
                return new TimeSpan(user + kernel);
            }
        }

        private SafeWaitHandle OpenThreadHandle(ThreadAccessMask accessMask) => Kernel32.OpenThread(accessMask, false, Id);

        public void Terminate(uint exitCode = 0) {
            using (var handle = OpenThreadHandle(ThreadAccessMask.Terminate)) {
                Kernel32.TerminateThread(handle, exitCode).ThrowIfWin32Failed();
            }
        }

        public NativeThreadPriority Priority {
            get => Kernel32.GetThreadPriority(SafeWaitHandle);
            set => Kernel32.SetThreadPriority(SafeWaitHandle, value).ThrowIfWin32Failed();
        }

        public static IoCompletionResult SleepEx(int msec, bool alertable) {
            return Kernel32.SleepEx(msec, alertable);
        }

        unsafe bool GetThreadBasicInfo(bool useExistingHandle, out THREAD_BASIC_INFORMATION info) {
            var handle = useExistingHandle ? SafeWaitHandle : OpenThreadHandle(ThreadAccessMask.QueryInformation);
            if (handle.IsInvalid) {
                info = new THREAD_BASIC_INFORMATION();
                return false;
            }
            bool success = NtDll.NtQueryInformationThread(handle, ThreadInformationClass.BasicInformation, out info, sizeof(THREAD_BASIC_INFORMATION)) >= 0;
            if (!useExistingHandle)
                handle.Dispose();
            return success;
        }

        public unsafe void GetStackLimits(out long stackBase, out long stackLimit) => GetStackLimits(null, out stackBase, out stackLimit);

        public unsafe void GetStackLimits(SafeHandle hProcess, out long stackBase, out long stackLimit) {
            stackBase = stackLimit = 0;
            if (!GetThreadBasicInfo(true, out var info)) {
                return;
            }
            NativeProcess process;
            if (hProcess != null && !hProcess.IsInvalid)
                process = NativeProcess.FromHandle(hProcess.DangerousGetHandle(), false);
            else
                process = NativeProcess.TryOpen(ProcessAccessMask.VmRead | ProcessAccessMask.QueryInformation, ProcessId);

            if (process == null)
                return;

            void* teb;
            if (Environment.Is64BitProcess) {
                teb = info.TebBaseAddress;
                NT_TIB tib;
                if (process.ReadMemory(new IntPtr(teb), new IntPtr(&tib), sizeof(NT_TIB), false) == 0)
                    return;

                stackBase = tib.StackBase.ToInt64();
                stackLimit = tib.StackLimit.ToInt64();
            }
            else {
                teb = info.TebBaseAddress;
                NT_TIB32 tib;
                if (process.ReadMemory(new IntPtr(teb), new IntPtr(&tib), sizeof(NT_TIB32), false) == 0)
                    return;

                stackBase = tib.StackBase;
                stackLimit = tib.StackLimit;
            }

            process.Dispose();
        }
    }
}

