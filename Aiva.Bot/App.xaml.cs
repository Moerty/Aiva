using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Aiva.Bot {
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application {
        private void StartApp(object sender, EventArgs e) {

            if (File.Exists("ConfigFiles\\config.ini")) {
                var mainWindow = new MainWindow();
                mainWindow.Show();
            } else {
                var setup = new Views.Setup();
                setup.Show();
            }
        }

        private void ExitApp(object sender, EventArgs e) {
            //Application.Current.Dispatcher.Thread.Abort();
            //Dispatcher.CurrentDispatcher.Thread.Abort();

            // when setup is closed without saving the config file, 
            // aivaclient cant save the config, cause the file doesnt exists
            if (File.Exists("ConfigFiles\\config.ini")) {
                Core.AivaClient.Instance.Disconnect();
            }
            CefSharp.Cef.Shutdown();
            Environment.Exit(0);
        }
    }
}
