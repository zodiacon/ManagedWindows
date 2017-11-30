using Microsoft.Win32.SafeHandles;
using System;
using System.Threading;
using static Zodiacon.ManagedWindows.Processes.Kernel32;

namespace Zodiacon.ManagedWindows.Processes {
    public sealed class NativeJob : WaitHandle {
        public static NativeJob CreateNew(string name = null, JobObjectSecurity security = null) {
            return new NativeJob(CreateJobObject(security, name));
        }

        public static NativeJob FromHandle(IntPtr hJob) {
            return new NativeJob(hJob);
        }

        public static NativeJob Open(JobAccessMask accessMask, string name, bool inheritHandle = false) {
            return new NativeJob(OpenJobObject(accessMask, name, inheritHandle));
        }

        private NativeJob(IntPtr hJob) {
            SafeWaitHandle = new SafeWaitHandle(hJob, true);
        }

        private NativeJob(SafeWaitHandle hJob) {
            SafeWaitHandle = hJob;
        }
    }

}

