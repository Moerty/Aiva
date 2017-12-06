using Aiva.Core.Config;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.Tabs {
    public class SpamProtection {
        public ICommand CapsProtectionOptionsWindow { get; set; }
        public ICommand OpenWikiInBrowserCapsProtection { get; set; }

        public ICommand LinksProtectionOptionsWindow { get; set; }
        public ICommand OpenWikiInBrowserLinksProtection { get; set; }

        public ICommand BlacklistProtectionOptionsWindow { get; set; }
        public ICommand OpenWikiInBrowserBlacklistProtection { get; set; }

        public bool IsCapsActive {
            get {
                return Config.Instance.Storage.Chat.CapsChecker.Active;
            }
            set {
                Config.Instance.Storage.Chat.CapsChecker.Active = value;
            }
        }

        public bool IsLinksActive {
            get {
                return Config.Instance.Storage.Chat.LinkChecker.Active;
            }
            set {
                Config.Instance.Storage.Chat.LinkChecker.Active = value;
            }
        }

        public bool IsBlacklistActive {
            get {
                return Config.Instance.Storage.Chat.BlacklistWordsChecker.Active;
            }
            set {
                Config.Instance.Storage.Chat.BlacklistWordsChecker.Active = value;
            }
        }

        public SpamProtection() {
            CapsProtectionOptionsWindow = new Internal.RelayCommand(
                caps => ShowCapsOptions());

            OpenWikiInBrowserCapsProtection = new Internal.RelayCommand(
                open => Extensions.SpamProtection.Caps.OpenWikiInBrowser());

            LinksProtectionOptionsWindow = new Internal.RelayCommand(
                links => ShowLinksOptions());

            OpenWikiInBrowserLinksProtection = new Internal.RelayCommand(
                open => Extensions.SpamProtection.Links.OpenWikiInBrowser());

            BlacklistProtectionOptionsWindow = new Internal.RelayCommand(
                blacklist => OpenBlacklistOptions());

            OpenWikiInBrowserBlacklistProtection = new Internal.RelayCommand(
                open => Extensions.SpamProtection.Blacklist.OpenWikiInBrowser());
        }

        private async void OpenBlacklistOptions() {
            var optionsWindow = new Views.ChildWindows.SpamProtection.Blacklist();

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(optionsWindow).ConfigureAwait(false);
        }

        private async void ShowLinksOptions() {
            var optionWindow = new Views.ChildWindows.SpamProtection.Links();

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(optionWindow).ConfigureAwait(false);
        }

        private async void ShowCapsOptions() {
            var optionsWindow = new Views.ChildWindows.SpamProtection.Caps();

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(optionsWindow).ConfigureAwait(false);
        }
    }
}