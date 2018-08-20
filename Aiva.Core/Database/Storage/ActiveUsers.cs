using System;

namespace Aiva.Core.Database.Storage {
    public class ActiveUsers {
        public int TwitchUserId { get; set; }

        public DateTime JoinedTime { get; set; }

        public int UsersId { get; set; }
        public virtual Users Users { get; set; }
    }
}