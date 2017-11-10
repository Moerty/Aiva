using System;
using TwitchLib;

namespace Aiva.Core {

    public class AivaClient {

        /// <summary>
        /// Static AivaClientMember
        /// Thread safe
        /// </summary>
        private static readonly Lazy<AivaClient> lazyAivaClient = new Lazy<AivaClient>();

        public static AivaClient Instance => lazyAivaClient.Value;

        public TwitchClient AivaTwitchClient;
        public TwitchAPI TwitchApi;
        public string Username;
        public string Channel;
        public string ChannelID;
        public string TwitchID;

        public string ClientID;
        public string OAuthKey;

        public Client.Tasks.Tasks Tasks;

        public AivaClient() {
            // Get config related informations
            Username = Config.Config.Instance.Storage.General.BotName;
            OAuthKey = Config.Config.Instance.Storage.Credentials.TwitchOAuth;
            Channel = Config.Config.Instance.Storage.General.Channel;
            ClientID = Config.Config.Instance.Storage.Credentials.TwitchClientID;

            // setup TwitchApi
            SetupTwitch();

            GetChannelID();
        }

        /// <summary>
        /// Get the channel id from twitch
        /// </summary>
        private async void GetChannelID() {
            var channelDetails = await TwitchApi.Channels.v5.GetChannelAsync();

            ChannelID = channelDetails.Id.ToString();
        }

        /// <summary>
        /// Validation for Twitch ClientID & OAuthKey
        /// </summary>
        private async void DoValidation() {
            if (String.IsNullOrEmpty(TwitchApi.Settings.AccessToken))
                TwitchApi.Settings.AccessToken = OAuthKey;

            var root = await TwitchApi.Root.v5.GetRoot(OAuthKey);

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
            TwitchApi = new TwitchAPI(ClientID, OAuthKey);

            // TwitchClient
            var TwitchCredentials = new TwitchLib.Models.Client.ConnectionCredentials(Username, OAuthKey);
            AivaTwitchClient = new TwitchClient(
                credentials: TwitchCredentials,
                channel: null,
                chatCommandIdentifier: Convert.ToChar(Config.Config.Instance.Storage.General.CommandIdentifier),
                whisperCommandIdentifier: '@',
                logging: false,
                logger: null,
                autoReListenOnExceptions: true);

            Tasks = new Client.Tasks.Tasks();
            AivaTwitchClient = Tasks.SetTasks(AivaTwitchClient);

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