namespace Aiva.Bot.Views.SettingsTabs {
    /// <summary>
    /// Interaktionslogik für Interactions.xaml
    /// </summary>
    public partial class Interactions : MahApps.Metro.Controls.MetroContentControl {
        public Interactions() {
            InitializeComponent();
            this.DataContext = new ViewModels.SettingsViewModel.InteractionsViewModel();
        }
    }
}
