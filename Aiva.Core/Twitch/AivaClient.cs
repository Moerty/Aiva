using System;
using TwitchLib;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace Aiva.Core.Twitch {
    public class AivaClient {
        private static readonly Lazy<AivaClient> lazyAivaClient = new Lazy<AivaClient>();
        public static AivaClient Instance => lazyAivaClient.Value;
        public static bool DryRun = false;

        public TwitchClient TwitchClient;
        public TwitchAPI TwitchApi;
        public string BotName;
        public int BotId;
        public string Channel;
        public string ChannelId;
        public int TwitchId;
        public Tasks.Events Events;
        public bool IsPartnered;

        public readonly Tasks.Tasks Tasks;

        public AivaClient() {
            BotName = Config.Config.Instance.Storage.General.BotName;
            Channel = Config.Config.Instance.Storage.General.Channel;
            Tasks = new Tasks.Tasks();
            Events = new Tasks.Events();
            SetupTwitch();
        }

        private void SetupTwitch() {
            // TwitchApi
            TwitchApi = new TwitchAPI();
            TwitchApi.Settings.ClientId = Config.Config.Instance.Storage.Credentials.TwitchClientID;
            TwitchApi.Settings.AccessToken = Config.Config.Instance.Storage.Credentials.TwitchOAuth;

            // TwitchClient
            var TwitchCredentials = new ConnectionCredentials(BotName,
                Config.Config.Instance.Storage.Credentials.TwitchOAuth);

            TwitchClient = new TwitchClient();
            TwitchClient.Initialize(
                credentials: TwitchCredentials,
                channel: Channel,
                chatCommandIdentifier: Convert.ToChar(Config.Config.Instance.Storage.General.CommandIdentifier),
                whisperCommandIdentifier: '@',
                autoReListenOnExceptions: true);

            TwitchClient.OnJoinedChannel += OnJoinedChannel;

            TwitchClient.Connect();

            //GetChannelId();
        }

        public void SetTasks() {
            GetChannelId();
            Tasks.SetTasks(ref TwitchClient);
            Events.SetEvents(ref TwitchClient);
            Events.SetFollowerService(AivaClient.Instance.TwitchApi);
        }

        private async void GetChannelId() {
            var channelDetails = TwitchApi.Channels.v5.GetChannelAsync().Result;
            ChannelId = channelDetails.Id;
            IsPartnered = channelDetails.Partner;

            Tasks.Channel = new Channel();
            Tasks.Channel.Start();
        }

        /// <summary>
        /// Fires when Client join Channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e) {
            TwitchClient.SendMessage(
                channel: Channel,
                message: "Aiva started, hi at all!",
                dryRun: DryRun);
        }
    }
}