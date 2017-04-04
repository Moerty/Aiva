using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackBoxBot.ViewModels {
    class HomeViewModel {
        public ObservableCollection<Models.HomeModel> TabItems { get; set; }

        public HomeViewModel() {
            CreateTabs();
        }

        private void CreateTabs() {
            TabItems = new ObservableCollection<Models.HomeModel>
            {
                new Models.HomeModel
                {
                    Header = nameof(Giveaway),
                    Content = new Views.Giveaway(),
                },
                new Models.HomeModel
                {
                    Header = "Losung",
                    Content = new Views.Auction(),
                },
                new Models.HomeModel
                {
                    Header = nameof(Songrequest),
                    Content = new Views.ucSongrequest()
                },
                new Models.HomeModel
                {
                    Header = "Voting",
                    Content = new Views.ucVoting()
                },
                new Models.HomeModel
                {
                    Header = "Streamwährung",
                    Content = new Views.ucCurrency()
                },
                new Models.HomeModel
                {
                    Header = "Wetten",
                    Content = new Views.Bets(),
                },
                new Models.HomeModel
                {
                    Header = "Rangverwaltung",
                    Content = new Controls.ucRank()
                },
                /*new Models.MainModel
                {
                    Header = "Toplisten",
                    Content = new Controls.ucToplist()
                },*/
                new Models.HomeModel
                {
                    Header = "Charts",
                    Content = new Views.ucStatistics(),
                }
            };
        }
    }
}
