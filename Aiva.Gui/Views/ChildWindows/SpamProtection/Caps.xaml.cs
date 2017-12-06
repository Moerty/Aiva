using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Gui.Views.ChildWindows.SpamProtection {
    /// <summary>
    /// Interaktionslogik für Caps.xaml
    /// </summary>
    public partial class Caps : ChildWindow {
        public Caps() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.SpamProtection.Caps();
        }
    }
}