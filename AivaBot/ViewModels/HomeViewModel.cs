using System.Collections.ObjectModel;

namespace AivaBot.ViewModels {
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
                    Content = new Views.Songrequest()
                },
                new Models.HomeModel
                {
                    Header = "Voting",
                    Content = new Views.Voting()
                },
                new Models.HomeModel
                {
                    Header = "Streamwährung",
                    Content = new Views.Currency()
                },
                new Models.HomeModel
                {
                    Header = "Wetten",
                    Content = new Views.Bets(),
                },
                new Models.HomeModel
                {
                    Header = "Rangverwaltung",
                    Content = new Controls.Rank()
                },
                new Models.HomeModel
                {
                    Header = "Charts",
                    Content = new Views.Statistics(),
                }
            };
        }
    }
}
