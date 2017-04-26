using System;
using TwitchLib;

namespace Aiva.Core {
    public class AivaClient {

        private static AivaClient _Instance;
        public static AivaClient Instance {
            get {
                if (_Instance == null)
                    InitAivaClient();

                return _Instance;
            }
            private set {
                _Instance = value;
            }
        }

        /// <summary>
        /// Init AivaClient
        /// </summary>
        private static void InitAivaClient() {
            Instance = new AivaClient();
        }


        public TwitchClient AivaTwitchClient;
        public string Username;
        public string Channel;

        private string ClientID;
        private string OAuthKey;

        public AivaClient() {
            // Get config related informations
            Username = Config.Config.Instance["General"]["BotName"];
            OAuthKey = Config.Config.Instance["Credentials"]["TwitchOAuth"];
            Channel = Config.Config.Instance["General"]["Channel"].ToLower();
            ClientID = Config.Config.Instance["Credentials"]["TwitchClientID"];

            // setup TwitchApi
            SetupTwitch();
        }

        /// <summary>
        /// Setup TwitchClient
        /// </summary>
        private void SetupTwitch() {
            // TwitchApi
            TwitchApi.SetClientId(ClientID);
            TwitchApi.SetAccessToken(OAuthKey);

            // TwitchClient
            var TwitchCredentials = new TwitchLib.Models.Client.ConnectionCredentials(Username, OAuthKey);
            AivaTwitchClient = new TwitchClient(
                TwitchCredentials,
                null,
                Convert.ToChar(Config.Config.Instance["General"]["CommandIdentifier"]),
                '@',
                true, // Loggin
                true); // ReListen when error occurs

            AivaTwitchClient = Client.Tasks.Tasks.SetClientTasks(AivaTwitchClient);

            AivaTwitchClient.OnConnected += OnConnected;
            AivaTwitchClient.OnJoinedChannel += OnJoinedChannel;

            AivaTwitchClient.Connect();
        }

        /// <summary>
        /// Fires when Client join Channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnJoinedChannel(object sender, TwitchLib.Events.Client.OnJoinedChannelArgs e) {
            AivaClient.Instance.AivaTwitchClient.SendMessage(Config.Text.Instance.GetString("AivaStartedText"));
        }

        /// <summary>
        /// Fires when Client connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnected(object sender, TwitchLib.Events.Client.OnConnectedArgs e) {
            AivaTwitchClient.JoinChannel(Channel);
        }
    }
}
