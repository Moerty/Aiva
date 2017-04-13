using TwitchLib;
using Aiva.Core.Config;
using System;
using TwitchLib.Models.Client;

namespace Aiva.Core.Client {
    class AivaClient {

        private static AivaClient _Client;
        public static AivaClient Client {
            get {
                if (_Client == null)
                    _Client = new AivaClient();

                return _Client;
            }
        }


        public TwitchClient AivaTwitchClient;
        string Username;
        string ClientID;
        string OAuthKey;
        string Channel;

        public AivaClient() {

            // Get config related informations
            Username = Config.General.Config[nameof(General)]["BotName"];
            OAuthKey = Config.General.Config["Credentials"]["TwitchOAuth"];
            Channel = Config.General.Config[nameof(General)]["Channel"].ToLower();
            ClientID = Config.General.Config["Credentials"]["TwitchClientID"];

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
                Convert.ToChar(Config.General.Config[nameof(General)]["CommandIdentifier"]),
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