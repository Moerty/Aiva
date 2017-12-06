using MahApps.Metro.Controls;

namespace Aiva.Gui.Views.Tabs {
    /// <summary>
    /// Interaktionslogik für Timers.xaml
    /// </summary>
    public partial class Timers : MetroContentControl {
        public Timers() {
            InitializeComponent();
            this.DataContext = new ViewModels.Tabs.Timers();
        }
    }
}