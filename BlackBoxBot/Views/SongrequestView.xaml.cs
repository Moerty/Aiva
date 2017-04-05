namespace BlackBoxBot.Views {
    /// <summary>
    /// Interaktionslogik für ucSongrequest.xaml
    /// </summary>
    public partial class ucSongrequest : MahApps.Metro.Controls.MetroContentControl {
        public ucSongrequest() {
            InitializeComponent();
            this.DataContext = new ViewModels.SongrequestViewModel(this);
        }
    }
}