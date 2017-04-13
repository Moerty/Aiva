namespace Aiva.Extensions.Models.Bets {
    [PropertyChanged.ImplementPropertyChanged]
    public class UserModel {
        public string Name { get; set; }
        public int Value { get; set; }
        public char Team { get; set; }
    }
}