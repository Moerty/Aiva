using Aiva.Core.Config;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.Windows {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Setup {
        public string TwitchClientID { get; set; }
        public ICommand RequestOAuthTokenCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        private readonly Core.Twitch.Authentication _authentication;
        private string _twitchOAuthToken;
        private bool _isTwitchAuthenticated;

        public Setup() {
            _authentication = new Core.Twitch.Authentication();

            RequestOAuthTokenCommand =
                new Internal.RelayCommand(
                    request => RequestOAuthToken(),
                    request => !string.IsNullOrEmpty(TwitchClientID));

            ConfirmCommand =
                new Internal.RelayCommand(
                    confirm => ConfirmSetup(),
                    confirm => _isTwitchAuthenticated);
        }

        private void ConfirmSetup() {
            Config.Instance.Storage.Credentials.TwitchClientID =
                TwitchClientID;

            Config.Instance.Storage.Credentials.TwitchOAuth =
                _twitchOAuthToken;

            GetTwitchUserDetails();

            RestartProgram();
        }

        private async void GetTwitchUserDetails() {
            var twitchApi = new TwitchLib.TwitchAPI(accessToken: _twitchOAuthToken);

            var twitchDetails = await twitchApi.Root.v5.GetRoot().ConfigureAwait(false);

            if (twitchDetails?.Token.Valid == true) {
                Config.Instance.Storage.General.BotName = twitchDetails.Token.Username;
                Config.Instance.Storage.Credentials.TwitchClientID = twitchDetails.Token.ClientId;
                Config.Instance.Storage.General.BotUserID = twitchDetails.Token.UserId;
                Config.Instance.Storage.General.Channel = twitchDetails.Token.Username.ToLower();
            } else {
                throw new Exception("Can't validate Twitch OAuth Token");
            }
        }

        private void RestartProgram() {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private async void RequestOAuthToken() {
            // send request to browser
            _authentication.SendRequestToBrowser(TwitchClientID);

            // get return values
            var result = await _authentication
                .GetAuthenticationValuesAsync()
                .ConfigureAwait(false);

            if (result != null) {
                _isTwitchAuthenticated = true;
                _twitchOAuthToken = result.Token;
            }
        }
    }
}