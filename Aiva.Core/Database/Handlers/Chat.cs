using System;
using System.Linq;
using TwitchLib.Events.Client;

namespace Aiva.Core.Database.Handlers {
    public class Chat {
        /// <summary>
        /// Add raw Message to Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddReceivedMessageToDatabase(object sender, OnMessageReceivedArgs e) {
            using (var context = new Storage.DatabaseContext()) {
                context.Chat.Add(new Storage.Chat {
                    TwitchUser = Convert.ToInt32(e.ChatMessage.UserId),
                    ChatMessage = e.ChatMessage.Message,
                    Timestamp = DateTime.Now,
                });

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Add message to database
        /// </summary>
        /// <param name="twitchID"></param>
        /// <param name="message"></param>
        /// <param name="timeStamp"></param>
        public void AddMessageToDatabase(int twitchID, string message, DateTime timeStamp) {
            using (var context = new Storage.DatabaseContext()) {
                context.Chat.Add(
                    new Storage.Chat {
                        TwitchUser = twitchID,
                        ChatMessage = message,
                        Timestamp = timeStamp
                    });

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get the last message from user if found
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetLastMessageFromUser(string userId) {
            using (var context = new Storage.DatabaseContext()) {
                var lastMessage = context.Chat.LastOrDefault(
                    l => Convert.ToInt32(userId) == l.TwitchUser);

                return
                    lastMessage != null
                    ? lastMessage.ChatMessage
                    : string.Empty;
            }
        }
    }
}