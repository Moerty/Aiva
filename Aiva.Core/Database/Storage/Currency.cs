namespace Aiva.Core.Database.Storage {
    public class Currency {
        public int CurrencyId { get; set; }

        public long Value { get; set; }

        public int TwitchUser { get; set; }
        public virtual Users Users { get; set; }
    }
}