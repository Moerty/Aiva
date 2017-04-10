namespace BlackBoxBot.Views {
    /// <summary>
    /// Interaktionslogik für ucVoting.xaml
    /// </summary>
    public partial class Voting : MahApps.Metro.Controls.MetroContentControl {
        public Voting() {
            InitializeComponent();
            this.DataContext = new ViewModels.VotingViewModel();
        }
    }
}
