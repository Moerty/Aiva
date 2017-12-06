using System;
using TwitchLib;
using TwitchLib.Events.Client;

namespace Aiva.Core.Twitch {
    public class AivaClient {
        private static readonly Lazy<AivaClient> lazyAivaClient = new Lazy<AivaClient>();
        public static AivaClient Instance => lazyAivaClient.Value;
        public static bool DryRun = false;

        public TwitchClient TwitchClient;
        public TwitchAPI TwitchApi;
        public string BotName;
        public string BotId;
        public string Channel;
        public string ChannelId;
        public string TwitchId;

        public readonly Tasks.Tasks Tasks;

        public AivaClient() {
            BotName = Config.Config.Instance.Storage.General.BotName;
            Channel = Config.Config.Instance.Storage.General.Channel;
            Tasks = new Tasks.Tasks();

            SetupTwitch();
        }

        private void SetupTwitch() {
            // TwitchApi
            TwitchApi = new TwitchAPI(Config.Config.Instance.Storage.Credentials.TwitchClientID,
                Config.Config.Instance.Storage.Credentials.TwitchOAuth);

            // TwitchClient
            var TwitchCredentials = new TwitchLib.Models.Client.ConnectionCredentials(BotName,
                Config.Config.Instance.Storage.Credentials.TwitchOAuth);

            TwitchClient = new TwitchClient(
                credentials: TwitchCredentials,
                channel: null,
                chatCommandIdentifier: Convert.ToChar(Config.Config.Instance.Storage.General.CommandIdentifier),
                whisperCommandIdentifier: '@',
                logging: false,
                logger: null,
                autoReListenOnExceptions: true);

            Tasks.SetTasks(ref TwitchClient);

            TwitchClient.OnConnected += OnConnected;
            TwitchClient.OnJoinedChannel += OnJoinedChannel;

            TwitchClient.Connect();

            GetChannelId();
        }

        private async void GetChannelId() {
            var channelDetails = await TwitchApi.Channels.v5.GetChannelAsync().ConfigureAwait(false);

            ChannelId = channelDetails.Id;
        }

        /// <summary>
        /// Fires when Client join Channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e) {
            TwitchClient.SendMessage(
                message: "Aiva started, hi at all!",
                dryRun: DryRun);
        }

        /// <summary>
        /// Fires when Client connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnected(object sender, OnConnectedArgs e) {
            TwitchClient.JoinChannel(Channel);
        }
    }
}