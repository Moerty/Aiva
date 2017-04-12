namespace AivaBot.Auction.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class AddModel {
        public string Username { get; set; }
        public int Tickets { get; set; }
    }
}
