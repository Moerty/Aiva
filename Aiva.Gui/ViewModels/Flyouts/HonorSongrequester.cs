using Aiva.Core.Twitch;
using PropertyChanged;
using System;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.Flyouts {
    [AddINotifyPropertyChangedInterface]
    public class HonorSongrequester {
        public int CurrencyToAdd { get; set; }
        public bool WriteInChat { get; set; }
        public ICommand HonorCommand { get; set; }

        public event EventHandler OnClose;

        private readonly int _userId;
        private readonly string _userName;
        private readonly Core.Database.Handlers.Currency.AddCurrency _addCurrencyDatabaseHandler;

        public HonorSongrequester(int userId, string username) {
            _userId = userId;
            _userName = username;
            _addCurrencyDatabaseHandler = new Core.Database.Handlers.Currency.AddCurrency();
            CurrencyToAdd = 100;
            WriteInChat = true;

            HonorCommand = new Internal.RelayCommand(
                honor => HonorRequester(),
                honor => CurrencyToAdd != 0);
        }

        private void HonorRequester() {
            _addCurrencyDatabaseHandler.Add(_userId, CurrencyToAdd);
            WriteInfoInChat();
            OnClose?.Invoke(this, EventArgs.Empty);
        }

        private void WriteInfoInChat() {
            if (WriteInChat) {
                AivaClient.Instance.TwitchClient.SendMessage(
                    message: $"@{_userName} was added {CurrencyToAdd} currency! Nice Song, dude!",
                    dryRun: AivaClient.DryRun);
            }
        }
    }
}