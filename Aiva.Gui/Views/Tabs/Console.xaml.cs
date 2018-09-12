using MahApps.Metro.Controls;

namespace Aiva.Gui.Views.Tabs {
    /// <summary>
    /// Interaktionslogik für Console.xaml
    /// </summary>
    public partial class Console : MetroContentControl {
        public Console() {
            InitializeComponent();
            this.DataContext = new ViewModels.Tabs.Chat();
        }
    }
}