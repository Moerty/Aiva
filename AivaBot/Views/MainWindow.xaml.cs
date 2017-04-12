namespace AivaBot.Views {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow {
        public MainWindow() {
            InitializeComponent();
            this.DataContext = new ViewModels.MainViewModel();
        }
    }
}
