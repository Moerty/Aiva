using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib;
using TwitchLib.Events.Client;

namespace Aiva.Core.Twitch.Tasks
{
    public class Events
    {
        public EventHandler<string> ShowMessage;
        public EventHandler<string> ShowNewSub;

        public void SetEvents(ref TwitchClient client) {
            client.OnNewSubscriber += NewSub;

#if DEBUG
            client.OnMessageReceived += MessageReceivedTest;
#endif
        }

        private void NewSub(object sender, OnNewSubscriberArgs e) {
            // sub normal
            if (!e.Subscriber.IsTwitchPrime) {
                // chat
                if (Config.Config.Instance.Storage.Interactions.WriteInChatNormalSub) {
                    AivaClient.Instance.TwitchClient.SendMessage(
                        $"Thanks @{e.Subscriber.DisplayName} for your subscription!", AivaClient.DryRun);
                }

                // overlay
                if(Config.Config.Instance.Storage.Overlay.ShowNewSub) {
                    if(Config.Config.Instance.Storage.Overlay.ShowMessageOnNewSubNormal) {
                        ShowNewSub?.Invoke(this, $"Thanks {e.Subscriber.DisplayName} for your subscription!");
                        ShowMessage?.Invoke(this, e.Subscriber.ResubMessage);
                    } else {
                        ShowNewSub?.Invoke(this, $"Thanks {e.Subscriber.DisplayName} for your subscription!");
                    }
                }
            // sub prime
            } else {
                // chat
                if (Config.Config.Instance.Storage.Interactions.WriteInChatPrimeSub) {
                    AivaClient.Instance.TwitchClient.SendMessage(
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
            }
        }

        public void MessageReceivedTest(object sender, OnMessageReceivedArgs e) {
            ShowMessage?.Invoke(this, e.ChatMessage.DisplayName);
        }
    }
}
