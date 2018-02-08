using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    [StructLayout(LayoutKind.Sequential)]
    struct GENERIC_MAPPING {
        public uint GenericRead;
        public uint GenericWrite;
        public uint GenericExecute;
        public uint GenericAll;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct SYSTEM_OBJECTTYPE_INFORMATION {
        public uint NextEntryOffset;
        public uint NumberOfObjects;
        public uint NumberOfHandles;
        public uint TypeIndex;
        public uint InvalidAttributes;
        public GENERIC_MAPPING GenericMapping;
        public uint ValidAccessMask;
        public uint PoolType;
        public byte SecurityRequired;
        public byte WaitableObject;
        public UNICODE_STRING TypeName;
    }

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
        MemoryExhaustion = 62,
        FaultInformation = 63,
        TelemetryIdInformation = 64,
        CommitReleaseInformation = 65,
        DefaultCpuSetsInformation = 66,
        AllowedCpuSetsInformation = 67,
        Reserved1Information = 66,
        Reserved2Information = 67,
        SubsystemProcess = 68,
        JobMemoryInformation = 69,
        InPrivate = 70,
        RaiseUMExceptionOnInvalidHandleClose = 71,
        IumChallengeResponse = 72,
        ChildProcessInformation = 73,
        HighGraphicsPriorityInformation = 74,
        SubsystemInformation = 75,
        EnergyValues = 76,
        ActivityThrottleState = 77,
        ActivityThrottlePolicy = 78,
        Win32kSyscallFilterInformation = 79,
        DisableSystemAllowedCpuSets = 80,
        WakeInformation = 81,
        EnergyTrackingState = 82,
        ManageWritesToExecutableMemory = 83,
        CaptureTrustletLiveDump = 84,
        TelemetryCoverage = 85,
        EnclaveInformation = 86,
        EnableReadWriteVmLogging = 87,
        UptimeInformation = 88,
        ImageSection = 89,
        MaxProcessInfoClass
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
        CriticalProcessErrorLogInformation = 142,
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
        ManufacturingInformation = 157,
        EnergyEstimationConfigInformation = 158,
        HypervisorDetailInformation = 159,
        ProcessorCycleStatsInformation = 160,
        VmGenerationCountInformation = 161,
        TrustedPlatformModuleInformation = 162,
        KernelDebuggerFlags = 163,
        CodeIntegrityPolicyInformation = 164,
        IsolatedUserModeInformation = 165,
        HardwareSecurityTestInterfaceResultsInformation = 166,
        SingleModuleInformation = 167,
        AllowedCpuSetsInformation = 168,
        DmaProtectionInformation = 169,
        InterruptCpuSetsInformation = 170,
        SecureBootPolicyFullInformation = 171,
        CodeIntegrityPolicyFullInformation = 172,
        AffinitizedInterruptProcessorInformation = 173,
        RootSiloInformation = 174,
        CpuSetInformation = 175,
        CpuSetTagInformation = 176,
        Win32WerStartCallout = 177,
        SecureKernelProfileInformation = 178,
        CodeIntegrityPlatformManifestInformation = 179,
        InterruptSteeringInformation = 180,
        SupportedProcessorArchitectures = 181,
        MemoryUsageInformation = 182,
        CodeIntegrityCertificateInformation = 183,
        PhysicalMemoryInformation = 184,
        ControlFlowTransition = 185,
        KernelDebuggingAllowed = 186,
        ActivityModerationExeState = 187,
        ActivityModerationUserSettings = 188,
        CodeIntegrityPoliciesFullInformation = 189,
        CodeIntegrityUnlockInformation = 190,
        IntegrityQuotaInformation = 191,
        FlushInformation = 192,
        ProcessorIdleMaskInformation = 193,
        SecureDumpEncryptionInformation = 194,
        WriteConstraintInformation = 195,
        KernelVaShadowInformation = 196,
        HypervisorSharedPagedInformation = 197,
        FirmwareBootPerformanceInformation = 198,
        CodeIntegrityVerificationInformation = 199,
        FirmwarePartitionInformation = 200,
        SystemSpeculationControlInformation = 201,
        MaxSystemInfoClass
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

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    unsafe struct UNICODE_STRING {
        public short Length;
        public short MaximumLengh;
        public char* Buffer;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct UNICODE_STRING32 {
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
    struct OBJECT_TYPE_INFORMATION {
        public UNICODE_STRING TypeName;
        public uint TotalNumberOfObjects;
        public uint TotalNumberOfHandles;
        public uint TotalPagedPoolUsage;
        public uint TotalNonPagedPoolUsage;
        public uint TotalNamePoolUsage;
        public uint TotalHandleTableUsage;
        public uint HighWaterNumberOfObjects;
        public uint HighWaterNumberOfHandles;
        public uint HighWaterPagedPoolUsage;
        public uint HighWaterNonPagedPoolUsage;
        public uint HighWaterNamePoolUsage;
        public uint HighWaterHandleTableUsage;
        public uint InvalidAttributes;
        public GENERIC_MAPPING GenericMapping;
        public uint ValidAccessMask;
        public byte SecurityRequired;
        public byte MaintainHandleCount;
        public uint PoolType;
        public uint DefaultPagedPoolCharge;
        public uint DefaultNonPagedPoolCharge;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct OBJECT_TYPES_INFORMATION {
        public uint NumberOfTypes;
        //public OBJECT_TYPE_INFORMATION TypeInformation;
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

    [StructLayout(LayoutKind.Explicit)]
    struct SYSTEM_PROCESS_INFORMATION64 {
        [FieldOffset(0)] public uint NextEntryOffset;
        [FieldOffset(4)] public int NumberOfThreads;
        [FieldOffset(8)] public long WorkingSetPrivateSize;
        [FieldOffset(0x10)] public uint HardFaultCount;
        [FieldOffset(0x14)] public uint NumberOfThreadsHighWatermark;
        [FieldOffset(0x18)] public ulong CycleTime;
        [FieldOffset(0x20)] public long CreateTime;
        [FieldOffset(0x28)] public long UserTime;
        [FieldOffset(0x30)] public long KernelTime;
        [FieldOffset(0x38)] public UNICODE_STRING ImageName;
        [FieldOffset(0x48)] public int BasePriority;
        [FieldOffset(0x50)] public IntPtr UniqueProcessId;
        [FieldOffset(0x58)] public IntPtr InheritedFromUniqueProcessId;
        [FieldOffset(0x60)] public int HandleCount;
        [FieldOffset(0x64)] public int SessionId;
        [FieldOffset(0x68)] public UIntPtr UniqueProcessKey;
        [FieldOffset(0x70)] public long PeakVirtualSize;
        [FieldOffset(0x78)] public long VirtualSize;
        [FieldOffset(0x80)] public uint PageFaultCount;
        [FieldOffset(0x88)] public long PeakWorkingSetSize;
        [FieldOffset(0x90)] public long WorkingSetSize;
        [FieldOffset(0x98)] public long QuotaPeakPagedPoolUsage;
        [FieldOffset(0xa0)] public long QuotaPagedPoolUsage;
        [FieldOffset(0xa8)] public long QuotaPeakNonPagedPoolUsage;
        [FieldOffset(0xb0)] public long QuotaNonPagedPoolUsage;
        [FieldOffset(0xb8)] public long PagefileUsage;
        [FieldOffset(0xc0)] public long PeakPagefileUsage;
        [FieldOffset(0xc8)] public long PrivatePageCount;
        [FieldOffset(0xd0)] public long ReadOperationCount;
        [FieldOffset(0xd8)] public long WriteOperationCount;
        [FieldOffset(0xe0)] public long OtherOperationCount;
        [FieldOffset(0xe8)] public long ReadTransferCount;
        [FieldOffset(0xf0)] public long WriteTransferCount;
        [FieldOffset(0xf8)] public long OtherTransferCount;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct SYSTEM_PROCESS_INFORMATION32 {
        [FieldOffset(0)] public uint NextEntryOffset;
        [FieldOffset(4)] public int NumberOfThreads;
        [FieldOffset(8)] public long WorkingSetPrivateSize;
        [FieldOffset(0x10)] public uint HardFaultCount;
        [FieldOffset(0x14)] public uint NumberOfThreadsHighWatermark;
        [FieldOffset(0x18)] public ulong CycleTime;
        [FieldOffset(0x20)] public long CreateTime;
        [FieldOffset(0x28)] public long UserTime;
        [FieldOffset(0x30)] public long KernelTime;
        [FieldOffset(0x38)] public UNICODE_STRING32 ImageName;
        [FieldOffset(0x40)] public int BasePriority;
        [FieldOffset(0x44)] public IntPtr UniqueProcessId;
        [FieldOffset(0x48)] public IntPtr InheritedFromUniqueProcessId;
        [FieldOffset(0x4c)] public int HandleCount;
        [FieldOffset(0x50)] public int SessionId;
        [FieldOffset(0x54)] public UIntPtr UniqueProcessKey;
        [FieldOffset(0x58)] public long PeakVirtualSize;
        [FieldOffset(0x5c)] public long VirtualSize;
        [FieldOffset(0x60)] public uint PageFaultCount;
        [FieldOffset(0x64)] public uint PeakWorkingSetSize;
        [FieldOffset(0x68)] public uint WorkingSetSize;
        [FieldOffset(0x6c)] public uint QuotaPeakPagedPoolUsage;
        [FieldOffset(0x70)] public uint QuotaPagedPoolUsage;
        [FieldOffset(0x74)] public uint QuotaPeakNonPagedPoolUsage;
        [FieldOffset(0x78)] public uint QuotaNonPagedPoolUsage;
        [FieldOffset(0x7c)] public uint PagefileUsage;
        [FieldOffset(0x80)] public uint PeakPagefileUsage;
        [FieldOffset(0x84)] public uint PrivatePageCount;
        [FieldOffset(0x88)] public long ReadOperationCount;
        [FieldOffset(0x90)] public long WriteOperationCount;
        [FieldOffset(0x98)] public long OtherOperationCount;
        [FieldOffset(0xa0)] public long ReadTransferCount;
        [FieldOffset(0xa8)] public long WriteTransferCount;
        [FieldOffset(0xb0)] public long OtherTransferCount;
    }


    public enum ThreadState {
        Initialized = 0,
        Ready = 1,
        Running = 2,
        Standby = 3,
        Terminated = 4,
        Waiting = 5,
        Transition = 6,
        DeferredReady = 7,
        GateWaitObsolete = 8,
        WaitingForProcessInSwap = 9
    }

    public enum WaitReason {
        Executive = 0,
        FreePage = 1,
        PageIn = 2,
        PoolAllocation = 3,
        DelayExecution = 4,
        Suspended = 5,
        UserRequest = 6,
        WrExecutive = 7,
        WrFreePage = 8,
        WrPageIn = 9,
        WrPoolAllocation = 10,
        WrDelayExecution = 11,
        WrSuspended = 12,
        WrUserRequest = 13,
        WrSpare0 = 14,
        WrQueue = 15,
        WrLpcReceive = 16,
        WrLpcReply = 17,
        WrVirtualMemory = 18,
        WrPageOut = 19,
        WrRendezvous = 20,
        WrKeyedEvent = 21,
        WrTerminated = 22,
        WrProcessInSwap = 23,
        WrCpuRateControl = 24,
        WrCalloutStack = 25,
        WrKernel = 26,
        WrResource = 27,
        WrPushLock = 28,
        WrMutex = 29,
        WrQuantumEnd = 30,
        WrDispatchInt = 31,
        WrPreempted = 32,
        WrYieldExecution = 33,
        WrFastMutex = 34,
        WrGuardedMutex = 35,
        WrRundown = 36,
        WrAlertByThreadId = 37,
        WrDeferredPreempt = 38,
        WrPhysicalFault = 39,
        MaximumWaitReason
    }

    [StructLayout(LayoutKind.Sequential)]
    struct SYSTEM_THREAD_INFORMATION64 {
        public long KernelTime;
        public long UserTime;
        public long CreateTime;
        public uint WaitTime;
        uint dummy1;
        public IntPtr StartAddress;
        public CLIENT_ID ClinetId;
        public int Priority;
        public int BasePriority;
        public uint ContextSwitches;
        public ThreadState ThreadState;
        public WaitReason WaitReason;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct SYSTEM_THREAD_INFORMATION32 {
        public long KernelTime;
        public long UserTime;
        public long CreateTime;
        public uint WaitTime;
        public IntPtr StartAddress;
        public CLIENT_ID ClientId;
        public int Priority;
        public int BasePriority;
        public uint ContextSwitches;
        public ThreadState ThreadState;
        public WaitReason WaitReason;
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

    [StructLayout(LayoutKind.Sequential)]
    struct SYSTEM_HANDLE_INFORMATION_EX {
        public IntPtr HandleCount;
        IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct SYSTEM_HANDLE_INFORMATION {
        public uint HandleCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SYSTEM_HANDLE_TABLE_ENTRY_INFO {
        public ushort UniqueProcessId;
        public ushort CreatorBackTraceIndex;
        public byte ObjectTypeIndex;
        public byte HandleAttributes;
        public ushort HandleValue;
        public IntPtr Object;
        public uint GrantedAccess;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX {
        public IntPtr Object;
        public IntPtr UniqueProcessId;
        public IntPtr HandleValue;
        public uint GrantedAccess;
        public ushort CreatorBackTraceIndex;
        public ushort ObjectTypeIndex;
        public uint HandleAttributes;
        uint Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct RTL_PROCESS_MODULE_INFORMATION {
        IntPtr Section; 
        public IntPtr MappedBase;
        public IntPtr ImageBase;
        public uint ImageSize;
        public uint Flags;
        public ushort LoadOrderIndex;
        public ushort InitOrderIndex;
        public ushort LoadCount;
        public ushort OffsetToFileName;
        public fixed byte FullPathName[256];
    }

    struct RTL_PROCESS_MODULE_INFORMATION_EX {
        public ushort NextOffset;
        public RTL_PROCESS_MODULE_INFORMATION BaseInfo;
        public uint ImageChecksum;
        public uint TimeDateStamp;
        public IntPtr DefaultBase;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct SYSTEM_PERFORMANCE_INFORMATION {
        public long IdleProcessTime;
        public long IoReadTransferCount;
        public long IoWriteTransferCount;
        public long IoOtherTransferCount;
        public uint IoReadOperationCount;
        public uint IoWriteOperationCount;
        public uint IoOtherOperationCount;
        public uint AvailablePages;
        public uint CommittedPages;
        public uint CommitLimit;
        public uint PeakCommitment;
        public uint PageFaultCount;
        public uint CopyOnWriteCount;
        public uint TransitionCount;
        public uint CacheTransitionCount;
        public uint DemandZeroCount;
        public uint PageReadCount;
        public uint PageReadIoCount;
        public uint CacheReadCount;
        public uint CacheIoCount;
        public uint DirtyPagesWriteCount;
        public uint DirtyWriteIoCount;
        public uint MappedPagesWriteCount;
        public uint MappedWriteIoCount;
        public uint PagedPoolPages;
        public uint NonPagedPoolPages;
        public uint PagedPoolAllocs;
        public uint PagedPoolFrees;
        public uint NonPagedPoolAllocs;
        public uint NonPagedPoolFrees;
        public uint FreeSystemPtes;
        public uint ResidentSystemCodePage;
        public uint TotalSystemDriverPages;
        public uint TotalSystemCodePages;
        public uint NonPagedPoolLookasideHits;
        public uint PagedPoolLookasideHits;
        public uint AvailablePagedPoolPages;
        public uint ResidentSystemCachePage;
        public uint ResidentPagedPoolPage;
        public uint ResidentSystemDriverPage;
        public uint CcFastReadNoWait;
        public uint CcFastReadWait;
        public uint CcFastReadResourceMiss;
        public uint CcFastReadNotPossible;
        public uint CcFastMdlReadNoWait;
        public uint CcFastMdlReadWait;
        public uint CcFastMdlReadResourceMiss;
        public uint CcFastMdlReadNotPossible;
        public uint CcMapDataNoWait;
        public uint CcMapDataWait;
        public uint CcMapDataNoWaitMiss;
        public uint CcMapDataWaitMiss;
        public uint CcPinMappedDataCount;
        public uint CcPinReadNoWait;
        public uint CcPinReadWait;
        public uint CcPinReadNoWaitMiss;
        public uint CcPinReadWaitMiss;
        public uint CcCopyReadNoWait;
        public uint CcCopyReadWait;
        public uint CcCopyReadNoWaitMiss;
        public uint CcCopyReadWaitMiss;
        public uint CcMdlReadNoWait;
        public uint CcMdlReadWait;
        public uint CcMdlReadNoWaitMiss;
        public uint CcMdlReadWaitMiss;
        public uint CcReadAheadIos;
        public uint CcLazyWriteIos;
        public uint CcLazyWritePages;
        public uint CcDataFlushes;
        public uint CcDataPages;
        public uint ContextSwitches;
        public uint FirstLevelTbFills;
        public uint SecondLevelTbFills;
        public uint SystemCalls;
        public ulong CcTotalDirtyPages;
        public ulong CcDirtyPageThreshold;
        public long ResidentAvailablePages;
        public ulong SharedCommittedPages;
    }

    [SuppressUnmanagedCodeSecurity]
    public static partial class NtDll {
        const string Library = "ntdll";

        public const int StatusInfoLengthMismatch = unchecked((int)0xc0000004);

        [DllImport(Library, ExactSpelling = true)]
        public unsafe static extern int NtQuerySystemInformation(SystemInformationClass infoClass, IntPtr buffer, int size, int* actualSize = null);

        [DllImport(Library, ExactSpelling = true)]
        public static unsafe extern int NtQuerySystemInformationEx(SystemInformationClass infoClass, void* inputBuffer, int inputBufferLen, void* buffer, uint size, int* actualSize = null);

        [DllImport(Library, ExactSpelling = true)]
        public static extern int NtQueryInformationProcess(SafeHandle hProcess, ProcessInformationClass infoClass, IntPtr buffer, int size, out uint actualSize);

        [DllImport(Library, ExactSpelling = true)]
        public static unsafe extern int NtQueryInformationProcess(SafeHandle hProcess, ProcessInformationClass infoClass, void* buffer, int size, int* actualSize = null);

        [DllImport(Library, ExactSpelling = true)]
        public static extern int NtQueryInformationThread(SafeHandle hThread, ThreadInformationClass infoClass, IntPtr buffer, uint size, out uint actualSize);
    }
}
