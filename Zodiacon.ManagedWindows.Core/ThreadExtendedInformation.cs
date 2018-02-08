using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class ThreadExtendedInformation {
        readonly SYSTEM_THREAD_INFORMATION64 _info64;
        readonly SYSTEM_THREAD_INFORMATION32 _info32;
        readonly bool _is64;

        unsafe internal ThreadExtendedInformation(ProcessExtendedInformation process, SYSTEM_THREAD_INFORMATION64* info) {
            Process = process;
            _info64 = *info;
            _is64 = true;

            ThreadId = info->ClinetId.Thread.ToInt32();
        }

        unsafe internal ThreadExtendedInformation(ProcessExtendedInformation process, SYSTEM_THREAD_INFORMATION32* info) {
            _info32 = *info;

            Process = process;
            ThreadId = info->ClientId.Thread.ToInt32();
        }

        public ProcessExtendedInformation Process { get; }
        public int ThreadId { get; }
        public int BasePriority => _is64 ? _info64.BasePriority : _info32.BasePriority;
        public TimeSpan KernelTime => new TimeSpan(_is64 ? _info64.KernelTime : _info32.KernelTime);
        public TimeSpan UserTime => new TimeSpan(_is64 ? _info64.UserTime : _info32.UserTime);
        public DateTime CreateTime => new DateTime(_is64 ? _info64.CreateTime : _info32.CreateTime);
        public uint WaitTime => _is64 ? _info64.WaitTime : _info32.WaitTime;
        public IntPtr StartAddress => _is64 ? _info64.StartAddress : _info32.StartAddress;
        public int Priority => _is64 ? _info64.Priority : _info32.Priority;
        public uint ContextSwitches => _is64 ? _info64.ContextSwitches : _info32.ContextSwitches;
        public ThreadState State => _is64 ? _info64.ThreadState : _info32.ThreadState;
        public WaitReason WaitReason => _is64 ? _info64.WaitReason : _info32.WaitReason;
    }
}
