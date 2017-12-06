using System;

namespace Aiva.Core.Database.Storage {
    public class ActiveUsers {
        public int ActiveUsersId { get; set; }

        public DateTime JoinedTime { get; set; }

        public int TwitchUser { get; set; }
        public virtual Users Users { get; set; }
    }
}