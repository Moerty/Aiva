namespace Aiva.Bot.Views.SettingsTabs {
    /// <summary>
    /// Interaktionslogik für General.xaml
    /// </summary>
    public partial class General : MahApps.Metro.Controls.MetroContentControl {
        public General() {
            InitializeComponent();
            this.DataContext = new ViewModels.SettingsViewModel.GeneralTabViewModel();
        }
    }
}
