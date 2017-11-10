using System;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels.Flyouts {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class HonorSongrequester {
        public int CurrencyToAdd { get; set; }
        public bool WriteInChat { get; set; }

        public ICommand HonorRequesterCommand { get; set; }

        private string _twitchID;
        private readonly string _username;
        private Core.DatabaseHandlers.Currency.AddCurrency _addCurrencyDatabaseHandler;

        public HonorSongrequester(string username, string twitchID = null) {
            _addCurrencyDatabaseHandler = new Core.DatabaseHandlers.Currency.AddCurrency();

            if (!String.IsNullOrEmpty(twitchID)) {
                this._twitchID = twitchID;
                _username = !String.IsNullOrEmpty(username) ? username : String.Empty;
            } else {
                _username = username;
            }
            HonorRequesterCommand = new Internal.RelayCommand(h => HonorRequester(), h => !String.IsNullOrEmpty(twitchID));
        }

        /// <summary>
        /// Do Honor
        /// </summary>
        private void HonorRequester() {
            if (!String.IsNullOrEmpty(_twitchID)) {
                _addCurrencyDatabaseHandler.Add(_twitchID, CurrencyToAdd);
            }

            if (WriteInChat)
                SendMessageInChat();
        }

        /// <summary>
        /// Send Message in TwitchChat
        /// </summary>
        private void SendMessageInChat() {
            Core.Client.Internal.Chat.SendMessage(
                $"@{_username} was added {CurrencyToAdd} currency! Nice Song, dude!");
        }
    }
}