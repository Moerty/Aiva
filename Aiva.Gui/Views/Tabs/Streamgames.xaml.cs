using MahApps.Metro.Controls;

namespace Aiva.Gui.Views.Tabs {
    /// <summary>
    /// Interaktionslogik für SpamProtection.xaml
    /// </summary>
    public partial class Streamgames : MetroContentControl {
        public Streamgames() {
            InitializeComponent();
            this.DataContext = new ViewModels.Tabs.Streamgames();
        }
    }
}