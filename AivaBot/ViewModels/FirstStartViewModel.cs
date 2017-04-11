using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows;

namespace AivaBot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class FirstStartViewModel {
        public Models.FirstStartModel Model { get; set; } = new Models.FirstStartModel();
        public ICommand StartRequestCommand { get; set; } = new RoutedCommand();
        public ICommand StartRequestYouTubeCommand { get; set; } = new RoutedCommand();
        public ICommand StartBotCommand { get; set; } = new RoutedCommand();

        public FirstStartViewModel() {
            var type = new MahApps.Metro.Controls.MetroContentControl().GetType();
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(StartRequestCommand, StartRequest));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(StartRequestYouTubeCommand, StartRequestYoutube));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(StartBotCommand, StartBot));
        }

        private void StartBot(object sender, ExecutedRoutedEventArgs e) {
            if (String.IsNullOrEmpty(Model.Channel)) {
                return;
            }

            var ValidationResult = GetTwitchDetails(Model.OAuthToken);
            if (ValidationResult == null
                || !ValidationResult.Token.Valid) return;

            IniData config = new IniData(new FileIniDataParser().ReadFile("Configs\\general.default"));

            config["Credentials"]["TwitchOAuth"] = Model.OAuthToken;
            config["Credentials"]["TwitchClientID"] = "10n39mbfftkcy2kg1jkzmm62yszdcg";

            config["General"]["Channel"] = Model.Channel.ToLower();
            config["General"]["BotName"] = ValidationResult.Token.Username;

            if (!File.Exists("Configs\\general.ini")) {
                File.Create("Configs\\general.ini").Dispose();
            }
            if (!File.Exists("Configs\\Games\\bankheist.ini")) {
                File.Create("Configs\\Games\\bankheist.ini").Dispose();
            }
            if (!File.Exists("Configs\\currency.ini")) {
                File.Create("Configs\\currency.ini").Dispose();
            }
            if (!File.Exists("Configs\\modcommands.ini")) {
                File.Create("Configs\\modcommands.ini").Dispose();
            }
            if (!File.Exists("Configs\\songrequest.ini")) {
                File.Create("Configs\\songrequest.ini").Dispose();
            }

            // Create Configs
            Config.General.WriteConfig(config);
            Config.Bankheist.WriteInitialConfig();
            Config.Currency.WriteInitialConfig();
            Config.ModCommands.WriteInitialConfig();
            Config.Songrequest.WriteInitialConfig();

            Application.Current.MainWindow.Close();
        }

        /// <summary>
        /// Geht Validation Details
        /// </summary>
        /// <param name="oauth"></param>
        /// <returns></returns>
        private TwitchLib.Models.API.Other.Validate.ValidationResponse GetTwitchDetails(string oauth) {
            var result = TwitchLib.TwitchApi.ValidationAPIRequest(oauth);

            return result;
        }

        private void StartRequestYoutube(object sender, ExecutedRoutedEventArgs e) {
            Model.GoogleAuth = Songrequest.GoogleCheck.Authenticate();
        }

        /// <summary>
        /// Send OAuth Request to the Browser
        /// </summary>
        private void SendRequest() {
            Task.Run(() => Client.TwitchAuthentication.Instance.SendRequestToBrowser());
        }

        /// <summary>
        /// Start the Request to get the OAuth authentication details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StartRequest(object sender, ExecutedRoutedEventArgs e) {
            // Send Request
            SendRequest();

            var result = await Client.TwitchAuthentication.Instance.GetAuthenticationValuesAsync();

            if (result != null) {
                Model.OAuthToken = result.Token;

                if (Model.Scopes == null) Model.Scopes = new System.Collections.ObjectModel.ObservableCollection<string>();
                foreach (var scope in result.Scopes.Split(' ')) {
                    Model.Scopes.Add(scope);
                }

                Model.TwitchAuth = true;
            }
        }
    }
}
