namespace Aiva.Core.Database.Storage {
    public class TimeWatched {
        public int TimeWatchedId { get; set; }

        public long Time { get; set; }

        public int TwitchUser { get; set; }
        public virtual Users Users { get; set; }
    }
}