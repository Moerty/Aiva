using Aiva.Core.Database;

namespace Aiva.Bot.Views {
    /// <summary>
    /// Interaktionslogik für ucCurrency.xaml
    /// </summary>
    public partial class Currency : MahApps.Metro.Controls.MetroContentControl {

        public Currency() {
            InitializeComponent();
            this.DataContext = new ViewModels.CurrencyViewModel();
        }

        private void MetroContentControl_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            var currencyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("currencyViewSource")));

            currencyViewSource.Source = CurrencyHandler.GetCurrencyList();
        }
    }
}
