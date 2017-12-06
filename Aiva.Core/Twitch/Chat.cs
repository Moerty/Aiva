using TwitchLib.Events.Client;

namespace Aiva.Core.Twitch {
    public static class Chat {
        /// <summary>
        /// Send Message when new Susbcriber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e) {
            if (!e.Subscriber.IsTwitchPrime) {
                if (Config.Config.Instance.Storage.Interactions.WriteInChatNormalSub) {
                    AivaClient.Instance.TwitchClient.SendMessage(
                        $"Thanks @{e.Subscriber.DisplayName} for your Subscription!");
                }
            } else {
                if (Config.Config.Instance.Storage.Interactions.WriteInChatPrimeSub) {
                    AivaClient.Instance.TwitchClient.SendMessage(
                        $"Thanks @{e.Subscriber.DisplayName} for your Subscription via Prime!");
                }
            }
        }
    }
}