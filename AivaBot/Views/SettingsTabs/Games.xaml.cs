namespace AivaBot.Views.SettingsTabs {
    /// <summary>
    /// Interaktionslogik für Games.xaml
    /// </summary>
    public partial class Games : MahApps.Metro.Controls.MetroContentControl {
        public Games() {
            InitializeComponent();
            this.DataContext = new ViewModels.SettingsViewModel.GamesTabViewModel();
        }
    }
}
