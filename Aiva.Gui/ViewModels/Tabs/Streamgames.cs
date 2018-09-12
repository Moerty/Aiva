using Aiva.Core.Config;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.Tabs {
    public class Streamgames {
        public ICommand BankheistOptionsWindow { get; set; }
        public ICommand OpenWikiInBrowserBankheist { get; set; }

        private Extensions.Streamgames.Bankheist.Handler _bankheistHandler;

        public bool IsBankheistActive {
            get {
                return Config.Instance.Storage.StreamGames.Bankheist.General.Active;
            }
            set {
                Config.Instance.Storage.StreamGames.Bankheist.General.Active = value;
            }
        }

        public Streamgames() {
            BankheistOptionsWindow = new Internal.RelayCommand(
                bankheist => ShowBankheistOptions());

            OpenWikiInBrowserBankheist = new Internal.RelayCommand(
                open => Extensions.SpamProtection.Caps.OpenWikiInBrowser());

            _bankheistHandler = new Extensions.Streamgames.Bankheist.Handler();

            //var roulette = new Extensions.Streamgames.Roulette.Handler();
            //roulette.StartRoulette();
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