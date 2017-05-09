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
        public string TwitchID;

        public string ClientID;
        public string OAuthKey;

        public AivaClient() {
            // Get config related informations
            Username = Config.Config.Instance["General"]["BotName"];      //Switch to OAuth Validation
            OAuthKey = Config.Config.Instance["Credentials"]["TwitchOAuth"];
            Channel = Config.Config.Instance["General"]["Channel"].ToLower();
            ClientID = Config.Config.Instance["Credentials"]["TwitchClientID"];

            // setup TwitchApi
            SetupTwitch();
        }

        /// <summary>
        /// Validation for Twitch ClientID & OAuthKey
        /// </summary>
        private void DoValidation() {
            if (String.IsNullOrEmpty(TwitchAPI.Settings.AccessToken))
                TwitchAPI.Settings.AccessToken = OAuthKey;

            var root = TwitchAPI.Root.v5.GetRoot(OAuthKey).Result;


            if (root.Token.Valid) {
                ClientID = root.Token.ClientId;
                TwitchID = root.Token.UserId;
                Username = root.Token.Username;
            } else {
                throw new Exception("Cant validate Twitch OAuth Key");
            }
        }

        /// <summary>
        /// Setup TwitchClient
        /// </summary>
        private void SetupTwitch() {
            // TwitchApi
            TwitchAPI.Settings.AccessToken = OAuthKey;
            TwitchAPI.Settings.ClientId = ClientID;

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

            DoValidation();
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
        /// Disconnect from Twitch
        /// </summary>
        public void Disconnect() {
            AivaClient.Instance.AivaTwitchClient.Disconnect();
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
