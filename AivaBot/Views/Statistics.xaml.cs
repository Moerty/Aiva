namespace AivaBot.Views {
    /// <summary>
    /// Interaktionslogik für ucStatistics.xaml
    /// </summary>
    public partial class Statistics : MahApps.Metro.Controls.MetroContentControl {
        public Statistics() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChartsViewModel();
        }
    }
}
