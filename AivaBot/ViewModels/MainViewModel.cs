using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using PropertyChanged;
using System.Windows.Threading;
using TwitchLib;
using System.Windows.Input;
using System.Windows;

namespace AivaBot.ViewModels {
    [ImplementPropertyChanged]
    class MainViewModel {
        public Models.MainModel Model { get; set; }

        // Models
        private Views.Settings SettingsView;
        private Views.Dashboard DashboardView;
        private Views.Home HomeView;

        private DispatcherTimer OnoffTimer;

        public MainViewModel() {
            // Create Model
            Model = new Models.MainModel();

            // Create Models
            SettingsView = new Views.Settings();
            DashboardView = new Views.Dashboard();
            HomeView = new Views.Home();

            // Create WindowCommand
            CreateWindowCommands();

            // Stream online check
            SetOnOffTimer();
        }

        private void CreateWindowCommands() {
            var myResourceDictionary = new ResourceDictionary() {
                Source =
                new Uri("/AivaBot;component/Resources/Icons.xaml",
                        UriKind.RelativeOrAbsolute)
            };
            Model.WindowCommandItems = new ObservableCollection<Models.MainModel.WindowCommandModel> {
                new Models.MainModel.WindowCommandModel {
                    Header = "Viewer",
                    Icon = myResourceDictionary["users"] as Canvas
                },
                new Models.MainModel.WindowCommandModel {
                    Header = "Home",
                    Icon = myResourceDictionary["appbar_home"] as Canvas
                },
                new Models.MainModel.WindowCommandModel {
                    Header = "Dashboard",
                    Icon = myResourceDictionary["theater"] as Canvas
                },
                new Models.MainModel.WindowCommandModel {
                    Header = "Einstellungen",
                    Icon = myResourceDictionary["settings"] as Canvas
                }
            };

            var buttonType = new Button().GetType();

            CommandManager.RegisterClassCommandBinding(buttonType, new CommandBinding(Model.WindowCommandItems[0].Command, ViewerCommand));
            CommandManager.RegisterClassCommandBinding(buttonType, new CommandBinding(Model.WindowCommandItems[1].Command, HomeCommand));
            CommandManager.RegisterClassCommandBinding(buttonType, new CommandBinding(Model.WindowCommandItems[2].Command, DashboardCommand));
            CommandManager.RegisterClassCommandBinding(buttonType, new CommandBinding(Model.WindowCommandItems[3].Command, SettingsCommand));

            Model.Content = HomeView;
        }

        private void SettingsCommand(object sender, ExecutedRoutedEventArgs e) {
            Model.Content = SettingsView;
        }

        private void DashboardCommand(object sender, ExecutedRoutedEventArgs e) {
            if (Model.Content == SettingsView) {
                Config.Bankheist.WriteConfig();
                Config.Currency.WriteConfig();
                Config.General.WriteConfig();
                Config.ModCommands.WriteConfig();
                Config.Songrequest.WriteConfig();
            }

            Model.Content = DashboardView;
        }

        private void HomeCommand(object sender, ExecutedRoutedEventArgs e) {
            if (Model.Content == SettingsView) {
                Config.Bankheist.WriteConfig();
                Config.Currency.WriteConfig();
                Config.General.WriteConfig();
                Config.ModCommands.WriteConfig();
                Config.Songrequest.WriteConfig();
            }

            Model.Content = HomeView;
        }

        private void ViewerCommand(object sender, ExecutedRoutedEventArgs e) {
            var users = new Views.Users();
            users.Show();
        }

        private void SetOnOffTimer() {
            OnoffTimer = new DispatcherTimer();
            OnoffTimer.Tick += CheckStreamOnOrOffAsync;
            OnoffTimer.Interval = new TimeSpan(0, 1, 0);
            OnoffTimer.Start();
        }

        /// <summary>
		/// Statusbar Stream On Off Checker
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void CheckStreamOnOrOffAsync(object sender, EventArgs e) {
            var result = await TwitchApi.Streams.BroadcasterOnlineAsync(Config.General.Config["General"]["Channel"].ToLower());
            if (result) {
                Model.StreamerOnlineText = Config.Language.Instance.GetString("StreamOn");
                Model.IsOnline = true;
            }
            else {
                Model.StreamerOnlineText = Config.Language.Instance.GetString("StreamOff");
                Model.IsOnline = false;
            }
        }
    }
}
