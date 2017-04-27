using System;
using TwitchLib.Events.Client;

namespace Aiva.Core.Database {
    public class Chat {

        /// <summary>
        /// Add raw Message to Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void AddReceivedMessageToDatabase(object sender, OnMessageReceivedArgs e) {
            using (var context = new Storage.StorageEntities()) {
                if (Int64.TryParse(e.ChatMessage.UserId, out long twitchID)) {
                    context.Chat.Add(new Storage.Chat {
                        TwitchID = twitchID,
                        ChatMessage = e.ChatMessage.Message,
                        Timestamp = DateTime.Now,
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
