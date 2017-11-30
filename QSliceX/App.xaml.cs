using QSliceX.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace QSliceX {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        MainViewModel _vm;
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            _vm = new MainViewModel();
            var win = new MainWindow { DataContext = _vm };
            win.Show();
        }

        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);

            _vm.Save();
        }
    }
}
