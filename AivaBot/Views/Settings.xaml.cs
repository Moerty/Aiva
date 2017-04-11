namespace AivaBot.Views {
    /// <summary>
    /// Interaktionslogik für ucSettings.xaml
    /// </summary>
    public partial class Settings : MahApps.Metro.Controls.MetroContentControl {
        public Settings() {
            this.DataContext = new ViewModels.SettingsViewModel();
            InitializeComponent();
            cbLanguage.Items.Add("german");
        }
    }
}