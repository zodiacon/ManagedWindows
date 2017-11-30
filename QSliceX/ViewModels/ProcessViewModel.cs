using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSliceX.ViewModels {
    class ProcessViewModel : BindableBase {
        public string Name { get; set; }
        public int Id { get; set; }
        private float _cpu;
        public float CPU {
            get => _cpu;
            set => SetProperty(ref _cpu, value);
        }

        TimeSpan _totalTime;
        public TimeSpan TotalTime {
            get => _totalTime;
            set => SetProperty(ref _totalTime, value);
        }
    }
}
