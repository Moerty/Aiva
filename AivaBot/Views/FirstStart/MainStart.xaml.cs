namespace AivaBot.Views.FirstStart {
    /// <summary>
    /// Interaktionslogik für MainStart.xaml
    /// </summary>
    public partial class MainStart : MahApps.Metro.Controls.MetroWindow {
        public MainStart() {
            InitializeComponent();
            this.DataContext = new ViewModels.FirstStartViewModel();
        }
    }
}
