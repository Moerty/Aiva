namespace Aiva.Bot.Views {
    /// <summary>
    /// Interaktionslogik für cGiveaway.xaml
    /// </summary>
    public partial class Giveaway : MahApps.Metro.Controls.MetroContentControl {

        public Giveaway() {
            InitializeComponent();
            this.DataContext = new ViewModels.GiveawayViewModel();
        }
    }
}
