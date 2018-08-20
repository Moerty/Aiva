using Aiva.Core.Twitch;
using System;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;

namespace Aiva.Extensions.SpamProtection {
    public class Spam {
        private readonly Core.Database.Handlers.Chat _databaseChatHandler;

        public Spam() {
            _databaseChatHandler = new Core.Database.Handlers.Chat();
        }

        public void SpamCheck(object sender, OnMessageReceivedArgs e) {
            var lastMessage = _databaseChatHandler.GetLastMessageFromUser(e.ChatMessage.UserId);

            if (string.Compare(e.ChatMessage.Message, lastMessage, true) == 0) {
                AivaClient.Instance.TwitchClient.TimeoutUser(
                    viewer: e.ChatMessage.DisplayName,
                    duration: TimeSpan.FromSeconds(Core.Config.Config.Instance.Storage.Chat.SpamChecker.TimeoutTimeInSeconds),
                    dryRun: AivaClient.DryRun);
            }
        }
    }
}