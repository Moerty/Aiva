using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Extensions.Client;
using TwitchLib.Models.Client;

namespace Aiva.Core.Twitch {
    public class Channel {
        public EventHandler<int> OnNewTotalFollower;
        public EventHandler<int> OnNewTotalViews;
        public EventHandler<string> OnNewStreamGame;
        public EventHandler<string> OnNewStreamTitle;
        public EventHandler<int> OnNewTotalViewers;
        public EventHandler<bool> SubOnlyMode;
        public EventHandler<bool> FollowersOnlyMode;
        public EventHandler<bool> SlowMode;

        public int TotalFollowers { get; private set; }
        public int TotalViews { get; private set; }
        public string StreamGame { get; private set; }
        public string StreamTitle { get; private set; }
        public bool IsSubOnlyModeActive { get; private set; }
        public bool IsFollowersOnlyModeActive { get; private set; }
        public TimeSpan FollowersOnlyModeDuration { get; private set; }
        public bool IsSlowModeActive { get; private set; }

        private readonly Database.Handlers.Statistics _statisticsDatabaseHandler;

        public Channel() {
            _statisticsDatabaseHandler = new Database.Handlers.Statistics();
        }

        private void GetChannelState() {
            // run that in background, its not nessesary 
            // that we get the data instant
            Task.Factory.StartNew(() => {
                // fix for waiting that twitchclient is connect,
                // cause we call a method that throws an exception 
                // if twitchclient is not connected
                while(!AivaClient.Instance.TwitchClient.IsConnected) {
                    Thread.Sleep(1000);
                }
                var state = AivaClient.Instance.TwitchClient.GetJoinedChannel(
                AivaClient.Instance.Channel);

                if (state?.ChannelState != null) {
                    SetChannelStateValues(state.ChannelState);
                };
            });
        }

        private void SetChannelStateValues(ChannelState channelState) {
            IsSubOnlyModeActive = channelState.SubOnly == true;
            IsFollowersOnlyModeActive = channelState.FollowersOnly != TimeSpan.FromMinutes(0);
            if(IsFollowersOnlyModeActive) {
                FollowersOnlyModeDuration = channelState.FollowersOnly;
            }
            IsSlowModeActive = channelState.SlowMode == true;
        }

        public void Start() {
            Task.Factory.StartNew(ChannelUpdate);
            Task.Factory.StartNew(StartViewerStaticRecording);
            GetChannelState();
            AivaClient.Instance.TwitchClient.OnChannelStateChanged += ChannelStateChanged;
        }

        private void ChannelStateChanged(object sender, OnChannelStateChangedArgs e) {
            SetChannelStateValues(e.ChannelState);
        }

        private async void StartViewerStaticRecording() {
            while (true) {
                var stream = await AivaClient.Instance.TwitchApi.Streams.v5.GetStreamByUserAsync
                    (AivaClient.Instance.ChannelId).ConfigureAwait(false);

                if (stream?.Stream != null) { // check if offline
                    _statisticsDatabaseHandler.AddViewerCountToDatabase(stream.Stream.Viewers);
                    OnNewTotalViewers?.Invoke(this, stream.Stream.Viewers);
                }

                Thread.Sleep(300000); // wait 5 min to update this
            }
        }

        private async void ChannelUpdate() {
            while (true) {
                var channel = await AivaClient.Instance.TwitchApi.Channels.v5.GetChannelAsync().ConfigureAwait(false);

                if(channel != null) {
                    TotalFollowers = channel.Followers;
                    OnNewTotalFollower?.Invoke(this, TotalFollowers);

                    TotalViews = channel.Views;
                    OnNewTotalViews?.Invoke(this, TotalViews);

                    StreamGame = channel.Game;
                    OnNewStreamGame?.Invoke(this, StreamGame);

                    StreamTitle = channel.Status;
                    OnNewStreamTitle?.Invoke(this, StreamTitle);
                }

                Thread.Sleep(TimeSpan.FromMinutes(5));
            }
        }

        public void UpdateStreamTitle(string title) {
            AivaClient.Instance.TwitchApi.Channels.v5.UpdateChannelAsync(
                    channelId: AivaClient.Instance.ChannelId,
                    status: title);
        }

        public void UpdateStreamGame(string game) {
            AivaClient.Instance.TwitchApi.Channels.v5.UpdateChannelAsync(
                channelId: AivaClient.Instance.ChannelId,
                game: game);
        }

        public void UpdateStreamTitleAndGame(string title, string game) {
            UpdateStreamTitle(title);
            UpdateStreamGame(game);
        }

        public void SetSlowMode(bool on_off, int seconds = 0) {
            if (on_off) {
                AivaClient.Instance.TwitchClient.SlowModeOn
                    (TimeSpan.FromSeconds(seconds));
            } else {
                AivaClient.Instance.TwitchClient.SlowModeOff();
            }
        }

        public void SetSubMode(bool on_off) {
            if (on_off) {
                AivaClient.Instance.TwitchClient.SubscribersOnlyOn();
            } else {
                AivaClient.Instance.TwitchClient.SubscribersOnlyOff();
            }
        }

        public void SetFollowersMode(bool on_off) {
            if (!on_off) {
                AivaClient.Instance.TwitchClient.FollowersOnlyOff();
            }
            // add FollowersOnlyModeOn when fixed from Twitchlib
        }
    }
}
