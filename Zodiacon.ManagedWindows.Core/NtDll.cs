using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public enum ProcessInformationClass : uint {
        BasicInformation = 0,
        QuotaLimits = 1,
        IoCounters = 2,
        VmCounters = 3,
        Times = 4,
        BasePriority = 5,
        RaisePriority = 6,
        DebugPort = 7,
        ExceptionPort = 8,
        AccessToken = 9,
        LdtInformation = 10,
        LdtSize = 11,
        DefaultHardErrorMode = 12,
        IoPortHandlers = 13,
        PooledUsageAndLimits = 14,
        WorkingSetWatch = 15,
        UserModeIOPL = 16,
        EnableAlignmentFaultFixup = 17,
        PriorityClass = 18,
        Wx86Information = 19,
        HandleCount = 20,
        AffinityMask = 21,
        PriorityBoost = 22,
        DeviceMap = 23,
        SessionInformation = 24,
        ForegroundInformation = 25,
        Wow64Information = 26,
        ImageFileName = 27,
        LUIDDeviceMapsEnabled = 28,
        BreakOnTermination = 29,
        DebugObjectHandle = 30,
        DebugFlags = 31,
        HandleTracing = 32,
        IoPriority = 33,
        ExecuteFlags = 34,
        TlsInformation = 35,
        Cookie = 36,
        ImageInformation = 37,
        CycleTime = 38,
        PagePriority = 39,
        InstrumentationCallback = 40,
        ThreadStackAllocation = 41,
        WorkingSetWatchEx = 42,
        ImageFileNameWin32 = 43,
        ImageFileMapping = 44,
        AffinityUpdateMode = 45,
        MemoryAllocationMode = 46,
        GroupInformation = 47,
        TokenVirtualizationEnabled = 48,
        OwnerInformation = 49,
        WindowInformation = 50,
        HandleInformation = 51,
        MitigationPolicy = 52,
        DynamicFunctionTableInformation = 53,
        HandleCheckingMode = 54,
        KeepAliveCount = 55,
        RevokeFileHandles = 56,
        WorkingSetControl = 57,
        HandleTable = 58,
        CheckStackExtentsMode = 59,
        CommandLineInformation = 60,
        ProtectionInformation = 61,
    }

    public enum ThreadInformationClass : uint {
        BasicInformation,
        Times,
        Priority,
        BasePriority,
        AffinityMask,
        ImpersonationToken,
        DescriptorTableEntry,
        EnableAlignmentFaultFixup,
        EventPair,
        QuerySetWin32StartAddress,
        ZeroTlsCell,
        PerformanceCount,
        AmILastThread,
        IdealProcessor,
        PriorityBoost,
        SetTlsArrayAddress,
        IsIoPending,
        HideFromDebugger,
        BreakOnTermination,
        SwitchLegacyState,
        IsTerminated,
        LastSystemCall,
        IoPriority,
        CycleTime,
        PagePriority,
        ActualBasePriority,
        TebInformation,
        CSwitchMon,
        CSwitchPmu,
        Wow64Context,
        GroupInformation,
        UmsInformation,
        CounterProfiling,
        IdealProcessorEx,
        CpuAccountingInformation,
        SuspendCount,
        HeterogeneousCpuPolicy,
        ContainerId,
        NameInformation,
        Property,
        SelectedCpuSets,
        SystemThreadInformation,
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct CLIENT_ID {
        public IntPtr Process;
        public IntPtr Thread;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct EXCEPTION_REGISTRATION_RECORD {
        EXCEPTION_REGISTRATION_RECORD* Next;
        IntPtr Handler;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct UNICODE_STRING {
        public short Length;
        public short MaximumLengh;
        public char* Buffer;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct LIST_ENTRY {
        public LIST_ENTRY* Next;
        public LIST_ENTRY* Prev;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct NT_TIB {
        EXCEPTION_REGISTRATION_RECORD* ExceptionList;
        public IntPtr StackBase;
        public IntPtr StackLimit;
        public IntPtr SubSystemTib;
        public IntPtr FiberData;
        public IntPtr ArbitraryUserPointer;
        NT_TIB* Self;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct NT_TIB32 {
        uint ExceptionList;
        public uint StackBase;
        public uint StackLimit;
        public uint SubSystemTib;
        public uint FiberData;
        public uint ArbitraryUserPointer;
        uint Self;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct TEB32 {
        public NT_TIB32 Tib;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct TEB64 {
        public NT_TIB Tib;
        public uint EnvironmentPointer;
        public CLIENT_ID Cid;
        public IntPtr ActiveRpcInfo;
        public IntPtr ThreadLocalStoragePointer;
        public IntPtr Peb;
        public uint LastErrorValue;
        public uint CountOfOwnedCriticalSections;
        public IntPtr CsrClientThread;
        public IntPtr Win32ThreadInfo;

        public fixed uint Win32ClientInfo[0x1f];

        public IntPtr WOW32Reserved;
        public uint CurrentLocale;
        public uint FpSoftwareStatusRegister;

        public fixed long SystemReserved1[0x36];
        IntPtr Spare1;
        public uint ExceptionCode;

        fixed uint SpareBytes1[0x28];
        fixed long SystemReserved2[0xa];

        public uint GdiRgn;
        public uint GdiPen;
        public uint GdiBrush;
        public CLIENT_ID RealClientId;
        IntPtr GdiCachedProcessHandle;
        uint GdiClientPID;
        uint GdiClientTID;
        IntPtr GdiThreadLocaleInfo;
        fixed long UserReserved[5];

        public fixed long GlDispatchTable[0x118];
        fixed uint GlReserved1[0x1a];

        long GlReserved2;
        public long GlSectionInfo;
        public long GlSection;
        public long GlTable;
        public long GlCurrentRC;
        public long GlContext;
        public int LastStatusValue;
        public UNICODE_STRING StaticUnicodeString;

        public fixed char StaticUnicodeBuffer[0x105];
        long DeallocationStack;
        fixed long TlsSlots[0x40];
        public LIST_ENTRY TlsLinks;
        public IntPtr Vdm;
        public IntPtr ReservedForNtRpc;

        fixed long DbgSsReserved[2];
        public uint HardErrorDisabled;

        public fixed long Instrumentation[0x10];
        public IntPtr WinSockData;
        public uint GdiBatchCount;
        uint Spare2;
        uint Spare3;
        uint Spare4;
        public IntPtr ReservedForOle;
        public uint WaitingOnLoaderLock;
        public IntPtr StackCommit;
        public IntPtr StackCommitMax;
        public IntPtr StackReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct THREAD_BASIC_INFORMATION {
        public int ExitStatus;
        public void* TebBaseAddress;
        public CLIENT_ID ClientId;
        public UIntPtr AffinityMask;
        public int Priority;
        public int BasePriority;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct PROCESS_BASIC_INFORMATION {
        public int ExitStatus;
        public IntPtr PebBaseAddress;
        public UIntPtr AffinityMask;
        public int BasePriority;
        public IntPtr ProcessId;
        public IntPtr ParentProcessId;
    }

    [Flags]
    public enum ProcessExtendedInformationFlags {
        ProtectedProcess = 1,
        Wow64Process = 2,
        ProcessDeleting = 4,
        CrossSessionCreate = 8,
        Frozen = 16,
        Background = 32,
        StronglyNamed = 64
    }

    [StructLayout(LayoutKind.Sequential)]
    struct PROCESS_EXTENDED_BASIC_INFORMATION {
        IntPtr Size;
        public PROCESS_BASIC_INFORMATION BasicInfo;
        public ProcessExtendedInformationFlags Flags;

        internal void Init() {
            Size = new IntPtr(Marshal.SizeOf<PROCESS_EXTENDED_BASIC_INFORMATION>());
        }
    }

    public enum ProcessProtectedType {
        PsProtectedTypeNone,
        PsProtectedTypeProtectedLight,
        PsProtectedTypeProtected,
        PsProtectedTypeMax
    }

    public enum ProtectedSigner {
        PsProtectedSignerNone,
        PsProtectedSignerAuthenticode,
        PsProtectedSignerCodeGen,
        PsProtectedSignerAntimalware,
        PsProtectedSignerLsa,
        PsProtectedSignerWindows,
        PsProtectedSignerWinTcb,
        PsProtectedSignerMax
    }

    public enum SystemInformationClass {
        BasicInformation = 0,
        ProcessorInformation = 1,
        PerformanceInformation = 2,
        TimeOfDayInformation = 3,
        PathInformation = 4,
        ProcessInformation = 5,
        CallCountInformation = 6,
        DeviceInformation = 7,
        ProcessorPerformanceInformation = 8,
        FlagsInformation = 9,
        CallTimeInformation = 10,
        ModuleInformation = 11,
        LocksInformation = 12,
        StackTraceInformation = 13,
        PagedPoolInformation = 14,
        NonPagedPoolInformation = 15,
        HandleInformation = 16,
        ObjectInformation = 17,
        PageFileInformation = 18,
        VdmInstemulInformation = 19,
        VdmBopInformation = 20,
        FileCacheInformation = 21,
        PoolTagInformation = 22,
        InterruptInformation = 23,
        DpcBehaviorInformation = 24,
        FullMemoryInformation = 25,
        LoadGdiDriverInformation = 26,
        UnloadGdiDriverInformation = 27,
        TimeAdjustmentInformation = 28,
        SummaryMemoryInformation = 29,
        MirrorMemoryInformation = 30,
        PerformanceTraceInformation = 31,
        Obsolete0 = 32,
        ExceptionInformation = 33,
        CrashDumpStateInformation = 34,
        KernelDebuggerInformation = 35,
        ContextSwitchInformation = 36,
        RegistryQuotaInformation = 37,
        ExtendServiceTableInformation = 38,
        PrioritySeperation = 39,
        VerifierAddDriverInformation = 40,
        VerifierRemoveDriverInformation = 41,
        ProcessorIdleInformation = 42,
        LegacyDriverInformation = 43,
        CurrentTimeZoneInformation = 44,
        LookasideInformation = 45,
        TimeSlipNotification = 46,
        SessionCreate = 47,
        SessionDetach = 48,
        SessionInformation = 49,
        RangeStartInformation = 50,
        VerifierInformation = 51,
        VerifierThunkExtend = 52,
        SessionProcessInformation = 53,
        LoadGdiDriverInSystemSpace = 54,
        NumaProcessorMap = 55,
        PrefetcherInformation = 56,
        ExtendedProcessInformation = 57,
        RecommendedSharedDataAlignment = 58,
        ComPlusPackage = 59,
        NumaAvailableMemory = 60,
        ProcessorPowerInformation = 61,
        EmulationBasicInformation = 62,
        EmulationProcessorInformation = 63,
        ExtendedHandleInformation = 64,
        LostDelayedWriteInformation = 65,
        BigPoolInformation = 66,
        SessionPoolTagInformation = 67,
        SessionMappedViewInformation = 68,
        HotpatchInformation = 69,
        ObjectSecurityMode = 70,
        WatchdogTimerHandler = 71,
        WatchdogTimerInformation = 72,
        LogicalProcessorInformation = 73,
        Wow64SharedInformationObsolete = 74,
        RegisterFirmwareTableInformationHandler = 75,
        FirmwareTableInformation = 76,
        ModuleInformationEx = 77,
        VerifierTriageInformation = 78,
        SuperfetchInformation = 79,
        MemoryListInformation = 80,
        FileCacheInformationEx = 81,
        ThreadPriorityClientIdInformation = 82,
        ProcessorIdleCycleTimeInformation = 83,
        VerifierCancellationInformation = 84,
        ProcessorPowerInformationEx = 85,
        RefTraceInformation = 86,
        SpecialPoolInformation = 87,
        ProcessIdInformation = 88,
        ErrorPortInformation = 89,
        BootEnvironmentInformation = 90,
        HypervisorInformation = 91,
        VerifierInformationEx = 92,
        TimeZoneInformation = 93,
        ImageFileExecutionOptionsInformation = 94,
        CoverageInformation = 95,
        PrefetchPatchInformation = 96,
        VerifierFaultsInformation = 97,
        SystemPartitionInformation = 98,
        SystemDiskInformation = 99,
        ProcessorPerformanceDistribution = 100,
        NumaProximityNodeInformation = 101,
        DynamicTimeZoneInformation = 102,
        CodeIntegrityInformation = 103,
        ProcessorMicrocodeUpdateInformation = 104,
        ProcessorBrandString = 105,
        VirtualAddressInformation = 106,
        LogicalProcessorAndGroupInformation = 107,
        ProcessorCycleTimeInformation = 108,
        StoreInformation = 109,
        RegistryAppendString = 110,
        AitSamplingValue = 111,
        VhdBootInformation = 112,
        CpuQuotaInformation = 113,
        NativeBasicInformation = 114,
        ErrorPortTimeouts = 115,
        LowPriorityIoInformation = 116,
        BootEntropyInformation = 117,
        VerifierCountersInformation = 118,
        PagedPoolInformationEx = 119,
        SystemPtesInformationEx = 120,
        NodeDistanceInformation = 121,
        AcpiAuditInformation = 122,
        BasicPerformanceInformation = 123,
        QueryPerformanceCounterInformation = 124,
        SessionBigPoolInformation = 125,
        BootGraphicsInformation = 126,
        ScrubPhysicalMemoryInformation = 127,
        BadPageInformation = 128,
        ProcessorProfileControlArea = 129,
        CombinePhysicalMemoryInformation = 130,
        EntropyInterruptTimingInformation = 131,
        ConsoleInformation = 132,
        PlatformBinaryInformation = 133,
        PolicyInformation = 134,
        HypervisorProcessorCountInformation = 135,
        DeviceDataInformation = 136,
        DeviceDataEnumerationInformation = 137,
        MemoryTopologyInformation = 138,
        MemoryChannelInformation = 139,
        BootLogoInformation = 140,
        ProcessorPerformanceInformationEx = 141,
        Spare0 = 142,
        SecureBootPolicyInformation = 143,
        PageFileInformationEx = 144,
        SecureBootInformation = 145,
        EntropyInterruptTimingRawInformation = 146,
        PortableWorkspaceEfiLauncherInformation = 147,
        FullProcessInformation = 148,
        KernelDebuggerInformationEx = 149,
        BootMetadataInformation = 150,
        SoftRebootInformation = 151,
        ElamCertificateInformation = 152,
        OfflineDumpConfigInformation = 153,
        ProcessorFeaturesInformation = 154,
        RegistryReconciliationInformation = 155,
        EdidInformation = 156,
    }

    [SuppressUnmanagedCodeSecurity]
    public static partial class NtDll {
        const string Library = "ntdll";

        [DllImport(Library, ExactSpelling = true)]
        public static extern int NtQuerySystemInformation(SystemInformationClass infoClass, IntPtr buffer, uint size, out uint actualSize);

        [DllImport(Library, ExactSpelling = true)]
        public static extern int NtQueryInformationProcess(SafeHandle hProcess, ProcessInformationClass infoClass, IntPtr buffer, int size, out uint actualSize);

        [DllImport(Library, ExactSpelling = true)]
        public static unsafe extern int NtQueryInformationProcess(SafeHandle hProcess, ProcessInformationClass infoClass, void* buffer, int size, int* actualSize = null);

        [DllImport(Library, ExactSpelling = true)]
        public static extern int NtQueryInformationThread(SafeHandle hThread, ThreadInformationClass infoClass, IntPtr buffer, uint size, out uint actualSize);
    }
}
