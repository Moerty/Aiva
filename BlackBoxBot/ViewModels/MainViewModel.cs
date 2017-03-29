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

namespace BlackBoxBot.ViewModels
{
    [ImplementPropertyChanged]
	class MainViewModel
	{
		public ObservableCollection<Models.MainModel> tabItems { get; set; }
        public Models.MainModel model { get; set; }

        private DispatcherTimer OnoffTimer;

		public MainViewModel()
		{
            // Create Model
            model = new Models.MainModel();

            // Tabs
            CreateTabs();

            // Stream online check
            SetOnOffTimer();
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
                model.StreamerOnlineText = Config.Language.Instance.GetString("StreamOn");
                model.IsOnline = true;
            }
            else
            {
                model.StreamerOnlineText = Config.Language.Instance.GetString("StreamOff");
                model.IsOnline = false;
            }
        }

        private void CreateTabs()
        {
            tabItems = new ObservableCollection<Models.MainModel>
            {
                new Models.MainModel
                {
                    Header = nameof(Giveaway),
                    Content = new Views.Giveaway(),
                },
                new Models.MainModel
                {
                    Header = "Losung",
                    Content = new Views.Auction(),
                },
                new Models.MainModel
                {
                    Header = nameof(Songrequest),
                    Content = new Views.ucSongrequest()
                },
                new Models.MainModel
                {
                    Header = "Voting",
                    Content = new Views.ucVoting()
                },
                new Models.MainModel
                {
                    Header = "Streamwährung",
                    Content = new Views.ucCurrency()
                },
                new Models.MainModel
                {
                    Header = "Wetten",
                    Content = new Views.Bets(),
                },
                new Models.MainModel
                {
                    Header = "Rangverwaltung",
                    Content = new Controls.ucRank()
                },
                /*new Models.MainModel
                {
                    Header = "Toplisten",
                    Content = new Controls.ucToplist()
                },*/
                new Models.MainModel
                {
                    Header = "Charts",
                    Content = new Views.ucStatistics(),
                }
            };
        }
    }
}
