using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Gui.Views.ChildWindows.Songrequest {
    /// <summary>
    /// Interaktionslogik für Start.xaml
    /// </summary>
    public partial class Edit : ChildWindow {
        public Edit() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.Songrequest.Edit();
        }
    }
}