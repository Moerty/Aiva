using System;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels.Flyouts {
    [PropertyChanged.ImplementPropertyChanged]
    public class HonorSongrequester {
        private long TwitchID;
        public int CurrencyToAdd { get; set; }
        public bool WriteInChat { get; set; }

        public ICommand HonorRequesterCommand { get; set; }

        public HonorSongrequester(string username, long? twitchID = null) {
            if (twitchID.HasValue) {
                this.TwitchID = twitchID.Value;

                HonorRequesterCommand = new Internal.RelayCommand(h => HonorRequester(), h => twitchID.HasValue);
            }
        }

        private void HonorRequester() {
            Core.Database.Currency.Add.AddCurrencyToUser(TwitchID, CurrencyToAdd);

            if (WriteInChat)
                SendMessageInChat();
        }

        private void SendMessageInChat() {
            Core.Client.Internal.Chat.SendMessage(
                $"@");
        }
    }
}
