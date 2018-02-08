using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class ProcessExtendedInformation {
        readonly SYSTEM_PROCESS_INFORMATION64 _info64;
        readonly SYSTEM_PROCESS_INFORMATION32 _info32;
        readonly bool _is64;

        unsafe internal ProcessExtendedInformation(SYSTEM_PROCESS_INFORMATION64* info, bool includeThreads) {
            _info64 = *info;
            _is64 = true;

            ProcessId = info->UniqueProcessId.ToInt32();
            ParentProcessId = info->InheritedFromUniqueProcessId.ToInt32();
            ThreadCount = info->NumberOfThreads;
            ImageName = ProcessId == 0 ? "Idle" : new string(info->ImageName.Buffer);

            if (includeThreads && ThreadCount > 0) {
                var thread = (SYSTEM_THREAD_INFORMATION64*)((byte*)info + sizeof(SYSTEM_PROCESS_INFORMATION64));
                var threads = new ThreadExtendedInformation[ThreadCount];
                for (int i = 0; i < ThreadCount; i++) {
                    threads[i] = new ThreadExtendedInformation(this, thread);
                    thread = (SYSTEM_THREAD_INFORMATION64*)((byte*)thread + sizeof(SYSTEM_THREAD_INFORMATION64));
                }
                Threads = threads;
            }
        }

        unsafe internal ProcessExtendedInformation(SYSTEM_PROCESS_INFORMATION32* info, bool includeThreads) {
            _info32 = *info;

            ProcessId = info->UniqueProcessId.ToInt32();
            ParentProcessId = info->InheritedFromUniqueProcessId.ToInt32();
            ThreadCount = (int)info->NumberOfThreads;
            ImageName = new string(info->ImageName.Buffer);

            if (includeThreads && ThreadCount > 0) {
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
        public uint HardPageFaults => _is64 ? _info64.HardFaultCount : _info32.HardFaultCount;
        public DateTime CreateTime => new DateTime(_is64 ? _info64.CreateTime : _info32.CreateTime);
        public TimeSpan KernelTime => new TimeSpan(_is64? _info64.KernelTime : _info32.KernelTime);
        public TimeSpan UserTime => new TimeSpan(_is64 ? _info64.UserTime : _info32.UserTime);
        public ulong CycleTime => _is64 ? _info64.CycleTime : _info32.CycleTime;
        public int SessionId => _is64 ? _info64.SessionId : _info32.SessionId;
        public uint PageFaultCount => _is64 ? _info64.PageFaultCount : _info32.PageFaultCount;
        public int HandleCount => _is64 ? _info64.HandleCount : _info32.HandleCount;
        public int BasePriority => _is64 ? _info64.BasePriority : _info32.BasePriority;
        public long WorkingSetSize => _is64 ? _info64.WorkingSetSize : _info32.WorkingSetSize;
        public long PrivateWorkingSetSize => _is64 ? _info64.WorkingSetPrivateSize : _info32.WorkingSetPrivateSize;
        public long PagefileUsage => _is64 ? _info64.PagefileUsage : _info32.PagefileUsage;
        public long PeakPagefileUsage => _is64 ? _info64.PeakPagefileUsage : _info32.PeakPagefileUsage;
        public long PrivatePageCount => _is64 ? _info64.PrivatePageCount : _info32.PrivatePageCount;
        public long ReadOperationCount => _is64 ? _info64.ReadOperationCount : _info32.ReadOperationCount;
        public long WriteOperationCount => _is64 ? _info64.WriteOperationCount : _info32.WriteOperationCount;
        public long OtherOperationCount => _is64 ? _info64.OtherOperationCount : _info32.OtherOperationCount;
        public long ReadTransferCount => _is64 ? _info64.ReadTransferCount : _info32.ReadTransferCount;
        public long WriteTransferCount => _is64 ? _info64.WriteTransferCount : _info32.WriteTransferCount;
        public long OtherTransferCount => _is64 ? _info64.OtherTransferCount : _info32.OtherTransferCount;
        public long PeakVirtualSize => _is64 ? _info64.PeakVirtualSize : _info32.PeakVirtualSize;
        public long VirtualSize =>_is64 ? _info64.VirtualSize : _info32.VirtualSize;
        public long QuotaPeakPagedPoolUsage => _is64 ? _info64.QuotaPeakPagedPoolUsage : _info32.QuotaPeakPagedPoolUsage;
        public long QuotaPagedPoolUsage => _is64 ? _info64.QuotaPagedPoolUsage : _info32.QuotaPagedPoolUsage;
        public long QuotaPeakNonPagedPoolUsage => _is64 ? _info64.QuotaPeakNonPagedPoolUsage : _info32.QuotaPeakNonPagedPoolUsage;
        public long QuotaNonPagedPoolUsage => _is64 ? _info64.QuotaNonPagedPoolUsage : _info32.QuotaNonPagedPoolUsage;
        public UIntPtr UniqueProcessKey => _is64 ? _info64.UniqueProcessKey : _info32.UniqueProcessKey;
    }
}
