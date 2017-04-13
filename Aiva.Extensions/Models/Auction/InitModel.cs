namespace Aiva.Extensions.Models.Auction {
    public class InitModel {
        public string Name { get; set; }
        public string Command { get; set; }
        public bool WithTickets { get; set; }
        public int Tickets { get; set; }
        public bool WriteStartInChat { get; set; }
    }
}
