using MahApps.Metro.Controls;

namespace Aiva.Gui.Views.Tabs {
    /// <summary>
    /// Interaktionslogik für Voting.xaml
    /// </summary>
    public partial class Voting : MetroContentControl {
        public Voting() {
            InitializeComponent();
            this.DataContext = new ViewModels.Tabs.Voting();
        }
    }
}