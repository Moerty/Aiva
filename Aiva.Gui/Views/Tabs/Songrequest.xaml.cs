using MahApps.Metro.Controls;

namespace Aiva.Gui.Views.Tabs {
    /// <summary>
    /// Interaktionslogik für Songrequest.xaml
    /// </summary>
    public partial class Songrequest : MetroContentControl {
        public Songrequest() {
            InitializeComponent();
            this.DataContext = new ViewModels.Tabs.Songrequest();
        }
    }
}