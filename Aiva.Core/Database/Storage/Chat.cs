using System;

namespace Aiva.Core.Database.Storage {
    public class Chat {
        public int ChatId { get; set; }

        public DateTime Timestamp { get; set; }
        public string ChatMessage { get; set; }

        public int TwitchUser { get; set; }
        public virtual Users Users { get; set; }
    }
}