using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class ThreadExtendedInformation {
        unsafe internal ThreadExtendedInformation(ProcessExtendedInformation process, SYSTEM_THREAD_INFORMATION64* info) {
            Process = process;
            ThreadId = info->ClinetId.Thread.ToInt32();
            BasePriority = info->BasePriority;
            CreateTime = new DateTime(info->CreateTime);
            KernelTime = new TimeSpan(info->KernelTime);
            UserTime = new TimeSpan(info->UserTime);
            WaitTime = info->WaitTime;
            Priority = info->Priority;
            WaitReason = info->WaitReason;
            State = info->ThreadState;
            ContextSwicthes = info->ContextSwitches;
        }

        unsafe internal ThreadExtendedInformation(ProcessExtendedInformation process, SYSTEM_THREAD_INFORMATION32* info) {
            Process = process;
            ThreadId = info->ClinetId.Thread.ToInt32();
            BasePriority = info->BasePriority;
            CreateTime = new DateTime(info->CreateTime);
            KernelTime = new TimeSpan(info->KernelTime);
            UserTime = new TimeSpan(info->UserTime);
            WaitTime = info->WaitTime;
            Priority = info->Priority;
            WaitReason = info->WaitReason;
            State = info->ThreadState;
            ContextSwicthes = info->ContextSwitches;
        }

        public ProcessExtendedInformation Process { get; }
        public int ThreadId { get; }
        public int BasePriority { get; }
        public TimeSpan KernelTime { get; }
        public TimeSpan UserTime { get; }
        public DateTime CreateTime { get; }
        public uint WaitTime { get; }
        public IntPtr StartAddress { get; }
        public int Priority { get; }
        public uint ContextSwicthes { get; }
        public ThreadState State { get; }
        public WaitReason WaitReason { get; }
    }
}
