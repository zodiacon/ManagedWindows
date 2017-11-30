using Zodiacon.ManagedWindows.Processes;
using Prism.Commands;
using Prism.Mvvm;
using QSliceX.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace QSliceX.ViewModels {
    class MainViewModel : BindableBase {
        DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Input);
        Dictionary<int, ProcessViewModel> _processbyCpuTime;
        Settings _settings;

        public Settings Settings => _settings;

        int _tick;
        float _totalCpu;

        public IEnumerable<ProcessViewModel> Processes {
            get {
                var current = GetCurrentCpuTimes();
                var interval = Environment.TickCount - _tick;

                float total = 0;
                foreach (var p in current) {
                    if (_processbyCpuTime.TryGetValue(p.Key, out var process)) {
                        p.Value.CPU = (float)(p.Value.TotalTime.TotalMilliseconds - process.TotalTime.TotalMilliseconds) * 100 / interval / Environment.ProcessorCount;
                        total += p.Value.CPU;
                    }
                }

                if (_settings.ShowIdleProcess) {
                    current.Add(0, new ProcessViewModel {
                        CPU = 100 - total,
                        Name = "[Idle]"
                    });
                }

                TotalCPU = total;
                _processbyCpuTime = current;
                _tick = Environment.TickCount;
                var result = current.Values.Where(p => p.CPU >= Settings.MinimumCPU).OrderByDescending(p => p.CPU).Take(15);
                return result;
            }
        }

        public MainViewModel() {
            Process.EnterDebugMode();

            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            LoadSettings();

            _processbyCpuTime = GetCurrentCpuTimes();
            _tick = Environment.TickCount;

            _timer.Interval = TimeSpan.FromMilliseconds(Settings.Interval);
            _timer.Tick += (_, __) => {
                RaisePropertyChanged(nameof(Processes));
            };

            _timer.Start();
        }

        private void LoadSettings() {
            try {
                using (var stm = File.OpenRead(GetSettingsFile())) {
                    _settings = Helpers.Load<Settings>(stm);
                }
            }
            catch (Exception) {
            }

            if (_settings == null)
                _settings = new Settings();
        }

        string GetSettingsFile() {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\QSlice.settings.xml";
        }

        public void Save() {
            var filename = GetSettingsFile();
            if (File.Exists(filename))
                File.Delete(filename);
            using (var stm = File.OpenWrite(filename)) {
                Helpers.Save(stm, _settings);
            }
        }

        private Dictionary<int, ProcessViewModel> GetCurrentCpuTimes() {
            return (from p in NativeProcess.EnumProcesses()
                    let id = p.Id
                    where id != 0
                    let process = NativeProcess.Open(ProcessAccessMask.QueryLimitedInformation, p.Id)
                    select new {
                        id,
                        process.TotalTime,
                        p.Name
                    }).
                ToDictionary(pr => pr.id, pr => new ProcessViewModel {
                    Name = pr.Name,
                    TotalTime = pr.TotalTime,
                    Id = pr.id
                });
        }

        public float TotalCPU {
            get => _totalCpu;
            set => SetProperty(ref _totalCpu, value);
        }

        private double _startAngle = 0;

        public double StartAngle {
            get => _startAngle;
            set {
                if (value > 360)
                    value -= 360;
                SetProperty(ref _startAngle, value, () => {
                    RaisePropertyChanged(nameof(EndAngle));
                });
            }
        }

        public double EndAngle => _startAngle + 360;

        public ICommand CloseCommand => new DelegateCommand(() => Application.Current.Shutdown());

        public int Interval {
            get => Settings.Interval;
            set {
                Settings.Interval = value;
                _timer.Interval = TimeSpan.FromMilliseconds(value);
            }
        }

        public ICommand ResetToDefaultsCommand => new DelegateCommand(() => {
            _settings = new Settings();
            RaisePropertyChanged(nameof(Settings));
        });
    }
}
