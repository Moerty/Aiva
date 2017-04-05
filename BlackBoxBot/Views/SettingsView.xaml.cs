namespace BlackBoxBot.Views {
    /// <summary>
    /// Interaktionslogik für ucSettings.xaml
    /// </summary>
    public partial class SettingsView : MahApps.Metro.Controls.MetroContentControl {
        public SettingsView() {
            this.DataContext = new ViewModels.SettingsViewModel();
            InitializeComponent();
            cbLanguage.Items.Add("german");
        }
    }
}