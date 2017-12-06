using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Gui.Views.ChildWindows.Giveaway {
    /// <summary>
    /// Interaktionslogik für Start.xaml
    /// </summary>
    public partial class Start : ChildWindow {
        public Start() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.Giveaway.Start();
        }
    }
}