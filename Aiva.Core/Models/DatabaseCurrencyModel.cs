namespace Aiva.Core.Models {
    public static class DatabaseCurrencyModel {
        public class ListCurrencyUpdate {
            public string Name { get; set; }
            public string TwitchID { get; set; }
            public int Value { get; set; }
        }
    }
}