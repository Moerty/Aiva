using System;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels.Flyouts {
    [PropertyChanged.ImplementPropertyChanged]
    public class HonorSongrequester {
        private long TwitchID;
        private string Username;
        public int CurrencyToAdd { get; set; }
        public bool WriteInChat { get; set; }

        public ICommand HonorRequesterCommand { get; set; }

        public HonorSongrequester(string username, long? twitchID = null) {
            if (twitchID.HasValue) {
                this.TwitchID = twitchID.Value;
                Username = !String.IsNullOrEmpty(username) ? username : String.Empty;
            } else {
                Username = username;
            }
            HonorRequesterCommand = new Internal.RelayCommand(h => HonorRequester(), h => twitchID.HasValue);
        }

        private void HonorRequester() {
            if (TwitchID != 0) {
                Core.Database.Currency.Add.AddCurrencyToUser(TwitchID, CurrencyToAdd);
            }

            if (WriteInChat)
                SendMessageInChat();
        }

        private void SendMessageInChat() {
            Core.Client.Internal.Chat.SendMessage(
                $"@{Username} was added {CurrencyToAdd} currency! Nice Song, dude!");
        }
    }
}
