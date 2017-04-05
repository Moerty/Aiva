namespace BlackBoxBot.Views {
    /// <summary>
    /// Interaktionslogik für ucVoting.xaml
    /// </summary>
    public partial class ucVoting : MahApps.Metro.Controls.MetroContentControl {
        public ucVoting() {
            InitializeComponent();
            this.DataContext = new ViewModels.VotingViewModel();
        }
    }
}
