using Aiva.Core.Config;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.Tabs {
    public class Streamgames {
        public ICommand BankheistOptionsWindow { get; set; }
        public ICommand OpenWikiInBrowserBankheist { get; set; }

        public ICommand RouletteOptionsWindow { get; set; }
        public ICommand OpenWikiInBrowserRoulette { get; set; }

        private Extensions.Streamgames.Bankheist.Handler _bankheistHandler;
        private Extensions.Streamgames.Roulette.Handler _rouletteHandler;

        public bool IsBankheistActive {
            get {
                return Config.Instance.Storage.StreamGames.Bankheist.General.Active;
            }
            set {
                Config.Instance.Storage.StreamGames.Bankheist.General.Active = value;
            }
        }

        public bool IsRouletteActive {
            get {
                return Config.Instance.Storage.StreamGames.Roulette.General.Active;
            } set {
                if(value) {
                    Config.Instance.Storage.StreamGames.Roulette.General.Active = value;
                    _rouletteHandler.StartRoulette();
                    } else {
                    _rouletteHandler.StopRoulette();
                    Config.Instance.Storage.StreamGames.Roulette.General.Active = value;
                }
            }
        }

        public Streamgames() {
            BankheistOptionsWindow = new Internal.RelayCommand(
                bankheist => ShowBankheistOptions());

            OpenWikiInBrowserBankheist = new Internal.RelayCommand(
                open => Extensions.SpamProtection.Caps.OpenWikiInBrowser());

            _bankheistHandler = new Extensions.Streamgames.Bankheist.Handler();

            _rouletteHandler = new Extensions.Streamgames.Roulette.Handler();

            if(Config.Instance.Storage.StreamGames.Roulette.General.Active) {
                _rouletteHandler.StartRoulette();
            }
        }

        private async void ShowBankheistOptions() {
            var options = new Views.ChildWindows.Streamgames.Bankheist();
            options.ClosingFinished += (sender, EventArgs) => RestartBankheist();
            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(options).ConfigureAwait(false);
        }

        private void RestartBankheist() {
            _bankheistHandler = new Extensions.Streamgames.Bankheist.Handler();
        }

        private async void OpenBlacklistOptions() {
            var optionsWindow = new Views.ChildWindows.SpamProtection.Blacklist();

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(optionsWindow).ConfigureAwait(false);
        }
    }
}