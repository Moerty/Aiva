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
        public long TwitchID;

        private string ClientID;
        private string OAuthKey;

        public AivaClient() {
            // Get config related informations
            Username = Config.Config.Instance["General"]["BotName"];      //Switch to OAuth Validation
            OAuthKey = Config.Config.Instance["Credentials"]["TwitchOAuth"];
            Channel = Config.Config.Instance["General"]["Channel"].ToLower();
            ClientID = Config.Config.Instance["Credentials"]["TwitchClientID"];

            // Valid Twitch Credentials
            //DoValidation();

            // setup TwitchApi
            SetupTwitch();
        }

        /// <summary>
        /// Validation for Twitch ClientID & OAuthKey
        /// </summary>
        private void DoValidation() {
            //if (!TwitchApi.ValidClientId(ClientID, true))
            //    throw new Exception("Twitch ClientID is not valid!");
            // Bug!!! Reported!

            var oAuthResult = TwitchApi.ValidationAPIRequest(OAuthKey);

            if(oAuthResult != null) {
                if(oAuthResult.Token.Valid) {
                    TwitchID = Convert.ToInt64(oAuthResult.Token.UserId);
                    Username = oAuthResult.Token.Username;
                }
                else {
                    throw new Exception("OAuth Token is not valid!");
                }
            }
            else {
                throw new Exception("Cant validate Twitch OAuth Key");
            }
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

            TwitchID = Convert.ToInt64(TwitchApi.ValidationAPIRequest().Token.UserId);

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
