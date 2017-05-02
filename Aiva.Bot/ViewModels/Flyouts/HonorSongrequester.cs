using System;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels.Flyouts {
    [PropertyChanged.ImplementPropertyChanged]
    public class HonorSongrequester {
        private long TwitchID;
        public int CurrencyToAdd { get; set; }
        public bool WriteInChat { get; set; }

        public ICommand HonorRequesterCommand { get; set; }

        public HonorSongrequester(long twitchID = null) {
            if (twitchID != null) {
                this.TwitchID = twitchID;

                HonorRequesterCommand = new Internal.RelayCommand(h => HonorRequester(), h => Username != null);
            }
        }

        private void HonorRequester() {
            Core.Database.Currency.Add.
        }
    }
}
