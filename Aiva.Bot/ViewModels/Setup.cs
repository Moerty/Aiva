using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Setup {
        public string OAuthKey { get; set; }
        public string Channel { get; set; }
        public string ClientID { get; set; }
        public string BotName { get; set; }
        public bool IsYoutubeAuthenticated { get; set; }
        public bool IsTwitchAuthenticated { get; set; }


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
            RequestTwitchOAuthKeyCommand = new Internal.RelayCommand(t => RequestTwitchOAuthKey(), p => !String.IsNullOrEmpty(ClientID));
            RequestGoogleAuthCommand = new Internal.RelayCommand(g => RequestGoogleAuth());
            ConfirmCommand = new Internal.RelayCommand(c => Confirm(), c => !String.IsNullOrEmpty(OAuthKey) &&
                                                                            !String.IsNullOrEmpty(Channel) &&
                                                                            TwitchScopes != null &&
                                                                            IsYoutubeAuthenticated &&
                                                                            IsTwitchAuthenticated &&
                                                                            !String.IsNullOrEmpty(BotName));
        }

        private void Confirm() {
            Core.Config.Config.CreateDefaultConfig();
            Core.Config.Config.Instance["Credentials"]["TwitchOAuth"] = OAuthKey;
            Core.Config.Config.Instance["Credentials"]["TwitchClientID"] = ClientID;
            Core.Config.Config.Instance["General"]["Channel"] = Channel;
            Core.Config.Config.Instance["General"]["BotName"] = BotName;

            Core.Config.Config.WriteConfig();

            Core.DatabaseHandlers.Creator.CreateDatabaseIfNotExist();

            RestartProgram();
        }

        private void RestartProgram() {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void RequestGoogleAuth() {
            var youtubeService = Core.Client.YoutubeConnector.CreateYouTubeService();
            IsYoutubeAuthenticated = youtubeService != null;
        }

        private async void RequestTwitchOAuthKey() {
#pragma warning disable CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
            Task.Run(() => TwitchAuthenticator.SendRequestToBrowser(ClientID));
#pragma warning restore CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist

            var result = await TwitchAuthenticator.GetAuthenticationValuesAsync();

            if (result != null) {
                OAuthKey = result.Token;

                TwitchScopes = new ObservableCollection<string>();
                foreach (var scope in result.Scopes.Split(' ')) {
                    TwitchScopes.Add(scope);
                }

                IsTwitchAuthenticated = true;
            }
        }
    }
}
