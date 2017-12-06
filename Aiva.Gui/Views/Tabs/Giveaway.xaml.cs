using MahApps.Metro.Controls;

namespace Aiva.Gui.Views.Tabs {
    /// <summary>
    /// Interaktionslogik für Giveaway.xaml
    /// </summary>
    public partial class Giveaway : MetroContentControl {
        public Giveaway() {
            InitializeComponent();
            this.DataContext = new ViewModels.Tabs.Giveaway();
        }
    }
}