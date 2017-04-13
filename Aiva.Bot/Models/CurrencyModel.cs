using PropertyChanged;
using System.Windows.Media;
using Aiva.Core.Storage;

namespace Aiva.Bot.Models {
    [ImplementPropertyChanged]
    class CurrencyModel {
        public string BankheistText { get; set; }
        public SolidColorBrush BankheistTileColor { get; set; }
        public bool AddCurrencyOnOff { get; set; }
        public AsyncObservableCollection<Currency> UserList { get; set; }
    }
}
