using System.Collections.Generic;

namespace Aiva.Core.Database.Storage {
    public class Users {
        public int UsersId { get; set; }

        public int TwitchUser { get; set; }

        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Bio { get; set; }
        public System.DateTime? CreatedAt { get; set; }
        public System.DateTime? UpdatedAt { get; set; }
        public string Logo { get; set; }

        public virtual ICollection<Chat> Chat { get; set; }
        public virtual ActiveUsers ActiveUsers { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual TimeWatched TimeWatched { get; set; }
        public virtual ViewerStatistics ViewerStatics { get; set; }
    }
}