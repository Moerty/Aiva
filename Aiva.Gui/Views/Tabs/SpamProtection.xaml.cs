using MahApps.Metro.Controls;

namespace Aiva.Gui.Views.Tabs {
    /// <summary>
    /// Interaktionslogik für SpamProtection.xaml
    /// </summary>
    public partial class SpamProtection : MetroContentControl {
        public SpamProtection() {
            InitializeComponent();
            this.DataContext = new ViewModels.Tabs.SpamProtection();
        }
    }
}