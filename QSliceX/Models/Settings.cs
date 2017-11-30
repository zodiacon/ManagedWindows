using Prism.Mvvm;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSliceX.Models {
    public class Settings : BindableBase {
        string _palette = "Metro";
        public string Palette {
            get => _palette;
            set => SetProperty(ref _palette, value);
        }

        int _size = 700;
        public int Size {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        private int _fontSize = 11;

        public int FontSize {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }

        bool _showIdleProcess = true;
        public bool ShowIdleProcess {
            get => _showIdleProcess;
            set => SetProperty(ref _showIdleProcess, value);
        }

        public bool SmartLabels { get; set; } = true;

        int _interval = 1000;
        public int Interval {
            get => _interval;
            set => SetProperty(ref _interval, value);
        }

        private double _width = .75;

        public double Width {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        double _opacity = 1;
        public double Opacity {
            get => _opacity;
            set => SetProperty(ref _opacity, value);
        }

        float _minimumCPU = .5f;
        public float MinimumCPU {
            get => _minimumCPU;
            set => SetProperty(ref _minimumCPU, value);
        }
    }
}
