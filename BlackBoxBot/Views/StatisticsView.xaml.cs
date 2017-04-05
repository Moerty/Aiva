namespace BlackBoxBot.Views {
    /// <summary>
    /// Interaktionslogik für ucStatistics.xaml
    /// </summary>
    public partial class ucStatistics : MahApps.Metro.Controls.MetroContentControl {
        public ucStatistics() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChartsViewModel();
        }
    }
}
