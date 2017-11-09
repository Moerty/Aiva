using System;
using TwitchLib.Events.Client;
using TwitchLib.Extensions.Client;

namespace Aiva.Core.Client.Internal {
    public class Chat {

        /// <summary>
        /// Send Message when new Susbcriber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e) {
            if (!e.Subscriber.IsTwitchPrime) {
                if (Config.Config.Instance.Storage.Interactions.WriteInChatNormalSub) {
                    AivaClient.Instance.AivaTwitchClient.SendMessage(
                        Config.Text.Instance.GetString("InteractionChatMessageNormalSub")
                        .Replace("@USERNAME@", e.Subscriber.DisplayName));
                }
            } else {
                if (Config.Config.Instance.Storage.Interactions.WriteInChatPrimeSub) {
                    AivaClient.Instance.AivaTwitchClient.SendMessage(
                        Config.Text.Instance.GetString("InteractionChatMessagePrimeSub")
                        .Replace("@USERNAME@", e.Subscriber.DisplayName));
                }
            }
        }

        /// <summary>
        /// Unmute a User
        /// </summary>
        /// <param name="username"></param>
        public static void UnmuteUser(string username) {
            AivaClient.Instance.AivaTwitchClient.UnbanUser(username);
        }

        /// <summary>
        /// Mute a Viewer for 5 Minutes
        /// </summary>
        /// <param name="username"></param>
        public static void MuteUser(string username) {
            AivaClient.Instance.AivaTwitchClient.TimeoutUser(username, new TimeSpan(0, 5, 0), "Muted through Streamer!");
        }

        /// <summary>
        /// Send a message to Twitch
        /// </summary>
        /// <param name="message"></param>
        public static void SendMessage(string message) {
            AivaClient.Instance.AivaTwitchClient.SendMessage(message);
        }
    }
}
