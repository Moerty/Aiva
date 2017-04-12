namespace AivaBot.Models {

    [PropertyChanged.ImplementPropertyChanged]
    public class AuctionModel {
        public Text Text { get; set; }
    }

    [PropertyChanged.ImplementPropertyChanged]
    public class Text {
        public string NameWatermark { get; set; }
        public string CommandWatermark { get; set; }
        public string TicketExpanderName { get; set; }
        public string ButtonStartName { get; set; }
        public string ButtonStopName { get; set; }
        public string AuctionExpanderSettingsText { get; set; }
        public string AuctionWriteInChatText { get; set; }
        public string AuctionHeaderUsernameText { get; set; }
        public string AuctionHeaderTicketsText { get; set; }
    }
}