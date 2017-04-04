using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using PropertyChanged;
using System.Threading;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Threading;
using TwitchLib;
using System.Windows.Input;

namespace BlackBoxBot.ViewModels
{
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
            Model.WindowCommandItems = new ObservableCollection<Models.MainModel.WindowCommandModel> {
                new Models.MainModel.WindowCommandModel {
                    Header = "Viewer",
                },
                new Models.MainModel.WindowCommandModel {
                    Header = "Home"
                },
                new Models.MainModel.WindowCommandModel {
                    Header = "Dashboard"
                },
                new Models.MainModel.WindowCommandModel {
                    Header = "Einstellungen",
                }
            };

            var buttonType = new Button().GetType();

            CommandManager.RegisterClassCommandBinding(buttonType, new CommandBinding(Model.WindowCommandItems[0].Command, ViewerCommand));
            CommandManager.RegisterClassCommandBinding(buttonType, new CommandBinding(Model.WindowCommandItems[1].Command, HomeCommand));
            CommandManager.RegisterClassCommandBinding(buttonType, new CommandBinding(Model.WindowCommandItems[2].Command, DashboardCommand));
            CommandManager.RegisterClassCommandBinding(buttonType, new CommandBinding(Model.WindowCommandItems[3].Command, SettingsCommand));
        }

        private void SettingsCommand(object sender, ExecutedRoutedEventArgs e) {
            Model.Content = new Views.SettingsView();
        }

        private void DashboardCommand(object sender, ExecutedRoutedEventArgs e) {
            Model.Content = new Views.DashboardView();
        }

        private void HomeCommand(object sender, ExecutedRoutedEventArgs e) {
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
