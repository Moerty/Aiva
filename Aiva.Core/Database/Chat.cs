using System;
using Aiva.Core.Storage;

namespace Aiva.Core.Database {
    public class ChatHandler {
        public static async void MessageReceivedAsync(object sender, TwitchLib.Events.Client.OnMessageReceivedArgs e) {
            using (var context = new StorageEntities()) {
                var Message = new Chat {
                    Name = e.ChatMessage.Username,
                    Message = e.ChatMessage.Message,
                    Timestamp = DateTime.Now.ToString(),
                };

                context.Chat.Add(Message);
                await context.SaveChangesAsync();
            }
        }
    }
}
