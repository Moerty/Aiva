using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Controls;
using PropertyChanged;
using System.Windows.Threading;
using TwitchLib;
using System.Windows.Input;
using System.Windows;

namespace BlackBoxBot.ViewModels {
    [ImplementPropertyChanged]
	class MainViewModel
	{
        public Models.MainModel Model { get; set; }
        private DispatcherTimer OnoffTimer;

		public MainViewModel()
		{
            // Create Model
            Model = new Models.MainModel();

            // Create WindowCommand
            CreateWindowCommands();

            // Stream online check
            SetOnOffTimer();
        }

        private void CreateWindowCommands() {
            var myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source =
                new Uri("/BlackBoxBot;component/Resources/Icons.xaml",
                        UriKind.RelativeOrAbsolute);

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

            Model.Content = new Views.HomeViewModel();
        }

        private void SettingsCommand(object sender, ExecutedRoutedEventArgs e) {
            Model.Content = new Views.SettingsView();
        }

        private void DashboardCommand(object sender, ExecutedRoutedEventArgs e) {
            Model.Content = new Views.DashboardView();
        }

        private void HomeCommand(object sender, ExecutedRoutedEventArgs e) {
            if(Model.Content.GetType() != new Views.HomeViewModel().GetType())
                Model.Content = new Views.HomeViewModel();
        }

        private void ViewerCommand(object sender, ExecutedRoutedEventArgs e) {
            var u = new Views.pUsers();
            u.Show();
        }

        private void SetOnOffTimer()
        {
            OnoffTimer = new DispatcherTimer();
            OnoffTimer.Tick += CheckStreamOnOrOffAsync;
            OnoffTimer.Interval = new TimeSpan(0, 0, 5);
            OnoffTimer.Start();
        }

        /// <summary>
		/// Statusbar Stream On Off Checker
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void CheckStreamOnOrOffAsync(object sender, EventArgs e)
        {
            var result = await TwitchApi.Streams.BroadcasterOnlineAsync(Config.General.Config["General"]["Channel"].ToLower());


            if (result)
            {
                Model.StreamerOnlineText = Config.Language.Instance.GetString("StreamOn");
                Model.IsOnline = true;
            }
            else
            {
                Model.StreamerOnlineText = Config.Language.Instance.GetString("StreamOff");
                Model.IsOnline = false;
            }
        }

        
    }
}
