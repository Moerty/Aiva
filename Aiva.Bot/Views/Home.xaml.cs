namespace Aiva.Bot.Views {
    /// <summary>
    /// Interaktionslogik für HomeViewModel.xaml
    /// </summary>
    public partial class Home : MahApps.Metro.Controls.MetroContentControl {
        public Home() {
            InitializeComponent();
            this.DataContext = new ViewModels.HomeViewModel();
        }
    }
}
