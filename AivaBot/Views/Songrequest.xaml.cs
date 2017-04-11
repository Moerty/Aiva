namespace AivaBot.Views {
    /// <summary>
    /// Interaktionslogik für ucSongrequest.xaml
    /// </summary>
    public partial class Songrequest : MahApps.Metro.Controls.MetroContentControl {
        public Songrequest() {
            InitializeComponent();
            this.DataContext = new ViewModels.SongrequestViewModel(this);
        }
    }
}