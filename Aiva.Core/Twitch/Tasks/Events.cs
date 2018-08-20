using System;
using System.Collections.Generic;
using TwitchLib;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api.Services;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace Aiva.Core.Twitch.Tasks {
    public class Events {
        public EventHandler<string> ShowMessage;
        public EventHandler<string> ShowNewSub;
        public EventHandler<List<IFollow>> OnNewFollower;

        private FollowerService _followerService;

        public void SetEvents(ref TwitchClient client) {
            client.OnNewSubscriber += NewSub;

#if DEBUG
            client.OnMessageReceived += MessageReceivedTest;
#endif
        }

        public async void SetFollowerService(ITwitchAPI api) {
            // FollowerService
            _followerService = new FollowerService(api);
            _followerService.SetChannelByChannelId(AivaClient.Instance.ChannelId);

            _followerService.OnNewFollowersDetected
                += (sender, e)
                => OnNewFollower?.Invoke(this, e.NewFollowers);

            await _followerService.StartService().ConfigureAwait(false);
        }

        private void NewSub(object sender, OnNewSubscriberArgs e) {
            //// sub normal
            //if (!e.Subscriber.IsTwitchPrime) {
            //    // chat
            //    if (Config.Config.Instance.Storage.Interactions.WriteInChatNormalSub) {
            //        AivaClient.Instance.TwitchClient.SendMessage(
            //            AivaClient.Instance.Channel,
            //            $"Thanks @{e.Subscriber.DisplayName} for your subscription!", AivaClient.DryRun);
            //    }

            //    // overlay
            //    if (Config.Config.Instance.Storage.Overlay.ShowNewSub) {
            //        if (Config.Config.Instance.Storage.Overlay.ShowMessageOnNewSubNormal) {
            //            ShowNewSub?.Invoke(this, $"Thanks {e.Subscriber.DisplayName} for your subscription!");
            //            ShowMessage?.Invoke(this, e.Subscriber.ResubMessage);
            //        } else {
            //            ShowNewSub?.Invoke(this, $"Thanks {e.Subscriber.DisplayName} for your subscription!");
            //        }
            //    }
            //    // sub prime
            //} else {
                // chat
                if (Config.Config.Instance.Storage.Interactions.WriteInChatPrimeSub) {
                    AivaClient.Instance.TwitchClient.SendMessage(
                        AivaClient.Instance.Channel,
                        $"Thanks @{e.Subscriber.DisplayName} for your subscription via prime!", AivaClient.DryRun);
                }

                // overlay
                if (Config.Config.Instance.Storage.Overlay.ShowNewSubPrime) {
                    if (Config.Config.Instance.Storage.Overlay.ShowMessageOnNewSubPrime) {
                        ShowNewSub?.Invoke(this, $"Thanks {e.Subscriber.DisplayName} for your subscription via prime!");
                        ShowMessage?.Invoke(this, e.Subscriber.ResubMessage);
                    } else {
                        ShowNewSub?.Invoke(this, $"Thanks {e.Subscriber.DisplayName} for your subscription via prime!");
                    }
                }
            //}
        }

        public void MessageReceivedTest(object sender, OnMessageReceivedArgs e) {
            ShowMessage?.Invoke(this, e.ChatMessage.DisplayName);
        }
    }
}