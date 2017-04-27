using System;
using TwitchLib.Events.Client;

namespace Aiva.Core.Client.Internal {
    public class Chat {

        /// <summary>
        /// Send Message when new Susbcriber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e) {
            if (!e.Subscriber.IsTwitchPrime) {
                if (Convert.ToBoolean(Config.Config.Instance["Interactions"]["WriteInChatNormalSub"])) {
                    AivaClient.Instance.AivaTwitchClient.SendMessage(
                        Config.Text.Instance.GetString("InteractionChatMessageNormalSub")
                        .Replace("@USERNAME@", e.Subscriber.DisplayName));
                }
            } else {
                if (Convert.ToBoolean(Config.Config.Instance["Interactions"]["WriteInChatPrimeSub"])) {
                    AivaClient.Instance.AivaTwitchClient.SendMessage(
                        Config.Text.Instance.GetString("InteractionChatMessagePrimeSub")
                        .Replace("@USERNAME@", e.Subscriber.DisplayName));
                }
            }
        }
    }
}
