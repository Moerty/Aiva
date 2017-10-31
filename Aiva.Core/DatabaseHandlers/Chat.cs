using System;
using TwitchLib.Events.Client;

namespace Aiva.Core.DatabaseHandlers {
    public class Chat {

        /// <summary>
        /// Add raw Message to Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddReceivedMessageToDatabase(object sender, OnMessageReceivedArgs e) {
            using (var context = new Storage.StorageEntities()) {
                context.Chat.Add(new Storage.Chat {
                    TwitchID = e.ChatMessage.UserId,
                    ChatMessage = e.ChatMessage.Message,
                    Timestamp = DateTime.Now,
                });

                context.SaveChanges();
            }
        }
    }
}
