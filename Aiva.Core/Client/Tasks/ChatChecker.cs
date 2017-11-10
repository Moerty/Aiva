using Aiva.Core.Config;
using System;
using System.Linq;
using TwitchLib.Enums;
using TwitchLib.Events.Client;
using TwitchLib.Extensions.Client;

namespace Aiva.Core.Client.Tasks {

    internal class ChatChecker {

        /// <summary>
        /// Caps checker for Viewers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CapsChecker(object sender, OnMessageReceivedArgs e) {
            if (e.ChatMessage.UserType == UserType.Viewer) {
                if (IsAllUpper(e.ChatMessage.Message)) {
                    AivaClient.Instance.AivaTwitchClient.TimeoutUser(e.ChatMessage.Username, new TimeSpan(0, 5, 0), Text.Instance.GetString("CapsTimeoutText"));
                }
            }
        }

        private static bool IsAllUpper(string input) {
            for (int i = 0; i < input.Length; i++) {
                if (Char.IsLetter(input[i]) && !Char.IsUpper(input[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Link checker for Viewers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void LinkChecker(object sender, OnMessageReceivedArgs e) {
            if (e.ChatMessage.UserType == UserType.Viewer) {
                if (e.ChatMessage.Message.Contains("www.") || e.ChatMessage.Message.Contains("http://")) {
                    AivaClient.Instance.AivaTwitchClient.TimeoutUser(e.ChatMessage.Username, new TimeSpan(0, 5, 0), Text.Instance.GetString("LinkTimeoutText"));
                }
            }
        }

        /// <summary>
        /// Check if User have written a BlacklistedWord and timeout him
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void BlacklistWordsChecker(object sender, OnMessageReceivedArgs e) {
            using (var context = new Storage.StorageEntities()) {
                var result = context.BlacklistedWords.Any(word => e.ChatMessage.Message.Contains(word.Word));

                if (result) {
                    AivaClient.Instance.AivaTwitchClient.TimeoutUser(e.ChatMessage.Username, new TimeSpan(0, 5, 0), Text.Instance.GetString("BlacklistedWordTimeoutText"));
                }
            }
        }

        // TODO: SpamCheck
    }
}