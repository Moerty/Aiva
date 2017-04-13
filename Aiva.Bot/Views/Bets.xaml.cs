namespace Aiva.Bot.Views {
    /// <summary>
    /// Interaktionslogik für Bets.xaml
    /// </summary>
    public partial class Bets : MahApps.Metro.Controls.MetroContentControl {
        public Bets() {
            InitializeComponent();
            this.DataContext = new ViewModels.BetsViewModel();
        }
    }
}
