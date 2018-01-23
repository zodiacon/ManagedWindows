using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class ProcessExtendedInformation {
        unsafe internal ProcessExtendedInformation(SYSTEM_PROCESS_INFORMATION64* info) {
            ProcessId = info->UniqueProcessId.ToInt32();
            ParentProcessId = info->InheritedFromUniqueProcessId.ToInt32();
            ThreadCount = (int)info->NumberOfThreads;
            ImageName = ProcessId == 0 ? "Idle" : new string(info->ImageName.Buffer);

            BasePriority = info->BasePriority;
            CreateTime = new DateTime(info->CreateTime);
            KernelTime = new TimeSpan(info->KernelTime);
            UserTime = new TimeSpan(info->UserTime);
            HardPageFaults = info->HardFaultCount;
            SessionId = info->SessionId;
            HandleCount = info->HandleCount;
            CycleTime = info->CycleTime;
            WorkingSetSize = info->WorkingSetSize;
            PrivateWorkingSetSize = info->WorkingSetPrivateSize;

            if (ThreadCount > 0) {
                var thread = (SYSTEM_THREAD_INFORMATION64*)((byte*)info + sizeof(SYSTEM_PROCESS_INFORMATION64));
                var threads = new ThreadExtendedInformation[ThreadCount];
                for (int i = 0; i < ThreadCount; i++) {
                    threads[i] = new ThreadExtendedInformation(this, thread);
                    thread = (SYSTEM_THREAD_INFORMATION64*)((byte*)thread + sizeof(SYSTEM_THREAD_INFORMATION64));
                }
                Threads = threads;
            }
        }

        unsafe internal ProcessExtendedInformation(SYSTEM_PROCESS_INFORMATION32* info) {
            ProcessId = info->UniqueProcessId.ToInt32();
            ParentProcessId = info->InheritedFromUniqueProcessId.ToInt32();
            ThreadCount = (int)info->NumberOfThreads;
            ImageName = new string(info->ImageName.Buffer);

            BasePriority = info->BasePriority;
            CreateTime = new DateTime(info->CreateTime);
            KernelTime = new TimeSpan(info->KernelTime);
            UserTime = new TimeSpan(info->UserTime);
            HardPageFaults = info->HardFaultCount;
            SessionId = info->SessionId;
            HandleCount = info->HandleCount;
            CycleTime = info->CycleTime;
            WorkingSetSize = info->WorkingSetSize;
            PrivateWorkingSetSize = info->WorkingSetPrivateSize;

            if (ThreadCount > 0) {
                var thread = (SYSTEM_THREAD_INFORMATION32*)((byte*)info + sizeof(SYSTEM_PROCESS_INFORMATION32));
                var threads = new ThreadExtendedInformation[ThreadCount];
                for (int i = 0; i < ThreadCount; i++) {
                    threads[i] = new ThreadExtendedInformation(this, thread);
                    thread = (SYSTEM_THREAD_INFORMATION32*)((byte*)thread + sizeof(SYSTEM_THREAD_INFORMATION32));
                }
                Threads = threads;
            }
        }

        public ThreadExtendedInformation[] Threads { get; }
        public int ProcessId { get; }
        public int ParentProcessId { get; }
        public int ThreadCount { get; }
        public string ImageName { get; }
        public uint HardPageFaults { get; }
        public DateTime CreateTime { get; }
        public TimeSpan KernelTime { get; }
        public TimeSpan UserTime { get; }
        public ulong CycleTime { get; }
        public int SessionId { get; }
        public uint PageFaultCount { get; }
        public int HandleCount { get; }
        public int BasePriority { get; }
        public long WorkingSetSize { get; }
        public long PrivateWorkingSetSize { get; }
        
    }
}
