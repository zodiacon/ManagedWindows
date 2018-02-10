using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.ManagedWindows.Core {
    public sealed class NativeObjectType {
        static Dictionary<string, NativeObjectType> _types = new Dictionary<string, NativeObjectType>(16);

        internal unsafe static NativeObjectType FromTypeInformation(OBJECT_TYPE_INFORMATION* info) {
            var name = info->TypeName.ToString();
            if (_types.TryGetValue(name, out var type))
                return type;

            type = new NativeObjectType(info, name);
            _types.Add(name, type);
            return type;
        }

        unsafe private NativeObjectType(OBJECT_TYPE_INFORMATION* info, string name = null) {
            NumberOfHandles = info->TotalNumberOfHandles;
            NumberOfObjects = info->TotalNumberOfObjects;
            Name = name ?? info->TypeName.ToString();
            TotalPagedPoolUsage = info->TotalPagedPoolUsage;
            TotalNonPagedPoolUsage = info->TotalNonPagedPoolUsage;
            PoolType = info->PoolType;
            GenericMapping = new GenericMapping(&info->GenericMapping);
            PeakHandleTableUsage = info->HighWaterHandleTableUsage;
            PeakNumberOfHandles = info->HighWaterNumberOfHandles;
            PeakNumberOfObjects = info->HighWaterNumberOfObjects;
            PeakNonPagedPoolUsage = info->HighWaterNonPagedPoolUsage;
            PeakPagedPoolUsage = info->HighWaterPagedPoolUsage;
            TotalNamePoolUsage = info->TotalNamePoolUsage;
            PeakNamePoolUsage = info->HighWaterNamePoolUsage;
            DefaultPagedPoolCharge = info->DefaultPagedPoolCharge;
            DefaultNonPagedPoolCharge = info->DefaultNonPagedPoolCharge;
        }

        public uint NumberOfHandles { get; }
        public uint NumberOfObjects { get; }
        public string Name { get; }
        public uint TotalPagedPoolUsage { get; }
        public uint TotalNonPagedPoolUsage { get; }
        public uint TotalNamePoolUsage { get; }
        public uint TotalHandleTableUsage { get; }
        public uint PeakNumberOfObjects { get; }
        public uint PeakNumberOfHandles { get; }
        public uint PeakPagedPoolUsage { get; }
        public uint PeakNonPagedPoolUsage { get; }
        public uint PeakNamePoolUsage { get; }
        public uint PeakHandleTableUsage { get; }
        public PoolType PoolType { get; }
        public uint DefaultPagedPoolCharge { get; }
        public uint DefaultNonPagedPoolCharge { get; }
        public GenericMapping GenericMapping { get; }
    }
}
