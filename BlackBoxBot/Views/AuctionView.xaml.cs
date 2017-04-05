namespace BlackBoxBot.Views {
    /// <summary>
    /// Interaktionslogik für Auction.xaml
    /// </summary>
    public partial class Auction : MahApps.Metro.Controls.MetroContentControl {
        public Auction() {
            InitializeComponent();
            this.DataContext = new ViewModels.AuctionViewModels(this);
        }
    }
}
