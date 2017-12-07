using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Gui.Views.ChildWindows.Streamgames {
    /// <summary>
    /// Interaktionslogik für Bankheist.xaml
    /// </summary>
    public partial class Bankheist : ChildWindow {
        public Bankheist() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.Streamgames.Bankheist();
        }
    }
}