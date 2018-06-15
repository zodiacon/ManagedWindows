namespace Zodiacon.ManagedWindows.Core {
    public sealed class KernelObjectType {
        public int Index { get; }
        public string Name { get; }
        public GenericMapping GenericMapping { get; }
        public uint InvalidAttributes { get; }
        public uint ValidAccessMask { get; }
        public uint NumberOfObjects { get; }
        public uint NumberOfHandles { get; }
        public uint PeakNumberOfObjects { get; }
        public uint PeakNumberOfHandles { get; }

        internal unsafe KernelObjectType(OBJECT_TYPE_INFORMATION* info) {
            Index = info->TypeIndex;
            Name = new string(info->TypeName.Buffer, 0, info->TypeName.Length / 2);
            GenericMapping = new GenericMapping(&info->GenericMapping);
            InvalidAttributes = info->InvalidAttributes;
            NumberOfHandles = info->TotalNumberOfHandles;
            NumberOfObjects = info->TotalNumberOfObjects;
            ValidAccessMask = info->ValidAccessMask;
            PeakNumberOfHandles = info->HighWaterNumberOfHandles;
            PeakNumberOfObjects = info->HighWaterNumberOfObjects;
        }
    }
}