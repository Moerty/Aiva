using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Gui.Views.ChildWindows.Voting {
    /// <summary>
    /// Interaktionslogik für Start.xaml
    /// </summary>
    public partial class Start : ChildWindow {
        public Start() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.Voting.Start();
        }
    }
}