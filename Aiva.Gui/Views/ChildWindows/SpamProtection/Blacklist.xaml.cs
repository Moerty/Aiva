using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Gui.Views.ChildWindows.SpamProtection {
    /// <summary>
    /// Interaktionslogik für Blacklist.xaml
    /// </summary>
    public partial class Blacklist : ChildWindow {
        public Blacklist() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.SpamProtection.Blacklist();
        }
    }
}