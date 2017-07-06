using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Setup {
        public string OAuthKey { get; set; }
        public string Channel { get; set; }
        public bool IsYoutubeAuthenticated { get; set; }

        public ICommand RequestTwitchOAuthKeyCommand { get; set; }
        public ICommand RequestGoogleAuthCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        public ObservableCollection<string> TwitchScopes { get; set; }

        private Core.Client.TwitchAuthentication TwitchAuthenticator;

        public Setup() {
            SetupCommands();
            TwitchAuthenticator = new Core.Client.TwitchAuthentication();
        }

        private void SetupCommands() {
            RequestTwitchOAuthKeyCommand = new Internal.RelayCommand(t => RequestTwitchOAuthKey(), p => true);
            RequestGoogleAuthCommand = new Internal.RelayCommand(g => RequestGoogleAuth(), g => true);
            ConfirmCommand = new Internal.RelayCommand(c => Confirm(), c => !String.IsNullOrEmpty(OAuthKey) &&
                                                                            !String.IsNullOrEmpty(Channel) &&
                                                                            TwitchScopes != null &&
                                                                            IsYoutubeAuthenticated);
        }

        private void Confirm() {
            var config = new IniData(new FileIniDataParser().ReadFile("ConfigFiles\\general.default"));
        }

        private void RequestGoogleAuth() {
            IsYoutubeAuthenticated = Extensions.Songrequest.YouTubeAuthenticator.Authenticate();
        }

        private async void RequestTwitchOAuthKey() {
            Task.Run(() => TwitchAuthenticator.SendRequestToBrowser());

            var result = await TwitchAuthenticator.GetAuthenticationValuesAsync();

            if (result != null) {
                OAuthKey = result.Token;

                TwitchScopes = new ObservableCollection<string>();
                foreach (var scope in result.Scopes.Split(' ')) {
                    TwitchScopes.Add(scope);
                }
            }
        }
    }
}
