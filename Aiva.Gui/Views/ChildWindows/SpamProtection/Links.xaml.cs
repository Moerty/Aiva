using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Gui.Views.ChildWindows.SpamProtection {
    /// <summary>
    /// Interaktionslogik für Links.xaml
    /// </summary>
    public partial class Links : ChildWindow {
        public Links() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.SpamProtection.Links();
        }
    }
}