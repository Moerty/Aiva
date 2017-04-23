namespace Aiva.Bot.Views {
    /// <summary>
    /// Interaktionslogik für ucDashboard.xaml
    /// </summary>

    public partial class Dashboard : MahApps.Metro.Controls.MetroContentControl {

        public Dashboard() {
            InitializeComponent();
            this.DataContext = new ViewModels.DashboardViewModel();
        }
    }
}
