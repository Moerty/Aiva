using TwitchLib;
using System;
using TwitchLib.Models.Client;

namespace Aiva.Core.Client {
    public class AivaClient {

        private static AivaClient _Client;
        public static AivaClient Client {
            get {
                if (_Client == null)
                    _Client = new AivaClient();

                return _Client;
            }
        }


        public TwitchClient AivaTwitchClient;
        public string Username;
        string ClientID;
        string OAuthKey;
        public string Channel;

        public AivaClient() {

            // Get config related informations
            Username = Config.GeneralConfig.Config["General"]["BotName"];
            OAuthKey = Config.GeneralConfig.Config["Credentials"]["TwitchOAuth"];
            Channel = Config.GeneralConfig.Config["General"]["Channel"].ToLower();
            ClientID = Config.GeneralConfig.Config["Credentials"]["TwitchClientID"];

            // Setup TwitchClient
            SetUpTwitchClient();

            // Connect
            AivaTwitchClient.Connect();
        }

        /// <summary>
        /// Setup TwitchClient
        /// Credentials
        /// Tasks
        /// </summary>
        private void SetUpTwitchClient() {

            // TwitchApi
            TwitchApi.SetClientId(ClientID);
            TwitchApi.SetAccessToken(OAuthKey);

            // TwitchClient
            var TwitchCredentials = new ConnectionCredentials(Username, OAuthKey);
            AivaTwitchClient = new TwitchClient(
                TwitchCredentials,
                Channel,
                Convert.ToChar(Config.GeneralConfig.Config["General"]["CommandIdentifier"]),
                '@',
                true, // Loggin
                true); // ReListen when error occurs


            // Set Tasks
            AivaTwitchClient = Tasks.Tasks.GetEvents(AivaTwitchClient);
            AivaTwitchClient.OnConnected += AivaTwitchClient_OnConnected;
        }

        /// <summary>
        /// Join channel when Client connects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AivaTwitchClient_OnConnected(object sender, TwitchLib.Events.Client.OnConnectedArgs e) {
            AivaTwitchClient.JoinChannel(Channel);
        }
    }
}