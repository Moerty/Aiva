namespace AivaBot.Views.SettingsTabs {
    /// <summary>
    /// Interaktionslogik für Chat.xaml
    /// </summary>
    public partial class Chat : MahApps.Metro.Controls.MetroContentControl {
        public Chat() {
            InitializeComponent();
            this.DataContext = new ViewModels.SettingsViewModel.ChatTabViewModel();
        }
    }
}
