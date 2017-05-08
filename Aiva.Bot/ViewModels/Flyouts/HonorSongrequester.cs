using System;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels.Flyouts {
    [PropertyChanged.ImplementPropertyChanged]
    public class HonorSongrequester {
        private string TwitchID;
        private readonly string Username;
        public int CurrencyToAdd { get; set; }
        public bool WriteInChat { get; set; }

        public ICommand HonorRequesterCommand { get; set; }

        public HonorSongrequester(string username, string twitchID = null) {
            if (!String.IsNullOrEmpty(twitchID)) {
                this.TwitchID = twitchID;
                Username = !String.IsNullOrEmpty(username) ? username : String.Empty;
            } else {
                Username = username;
            }
            HonorRequesterCommand = new Internal.RelayCommand(h => HonorRequester(), h => !String.IsNullOrEmpty(twitchID));
        }

        /// <summary>
        /// Do Honor
        /// </summary>
        private void HonorRequester() {
            if (!String.IsNullOrEmpty(TwitchID)) {
                Core.Database.Currency.Add.AddCurrencyToUser(TwitchID, CurrencyToAdd);
            }

            if (WriteInChat)
                SendMessageInChat();
        }

        /// <summary>
        /// Send Message in TwitchChat
        /// </summary>
        private void SendMessageInChat() {
            Core.Client.Internal.Chat.SendMessage(
                $"@{Username} was added {CurrencyToAdd} currency! Nice Song, dude!");
        }
    }
}
