using MahApps.Metro.Controls;

namespace Aiva.Gui.Views.Tabs {
    /// <summary>
    /// Interaktionslogik für Dashboard.xaml
    /// </summary>
    public partial class Dashboard : MetroContentControl {
        public Dashboard() {
            InitializeComponent();
            this.DataContext = new ViewModels.Tabs.Dashboard();
        }
    }
}