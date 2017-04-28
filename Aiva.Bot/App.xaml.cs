using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Aiva.Bot {
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application {
        private void StartApp(object sender, EventArgs e) {
            var mainWindow = new MainWindow();
            mainWindow.Closing += MainWindow_Closing;
            mainWindow.Show();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Environment.Exit(0);
        }

        private void ExitApp(object sender, EventArgs e) {
            Environment.Exit(0);
            Core.AivaClient.Instance.Disconnect();
        }
    }
}
