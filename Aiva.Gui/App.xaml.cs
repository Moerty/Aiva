using System;
using System.IO;
using System.Windows;

namespace Aiva.Gui {
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application {
        private void StartApp(object sender, EventArgs e) {
            if (File.Exists("Config\\config.json")) {
                var bootstrapper = new Core.Bootstrapper();
                bootstrapper.Start();

                var mainWindow = new MainWindow();
                mainWindow.Show();
            } else {
                var setup = new Views.Windows.Setup();
                setup.Show();
            }
        }

        private void ExitApp(object sender, EventArgs e) {
            // when setup is closed without saving the config file,
            // aivaclient cant save the config, cause the file doesnt exists
            if (File.Exists("Config\\config.json")) {
                Core.Twitch.AivaClient.Instance.TwitchClient.Disconnect();
                Core.Config.Config.Instance.SaveConfig();
            }
            CefSharp.Cef.Shutdown();
            Environment.Exit(0);
        }
    }
}