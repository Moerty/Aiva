using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Bot.Views.ChildWindows {
    /// <summary>
    /// Interaktionslogik für AddTimer.xaml
    /// </summary>
    public partial class StartGiveaway : ChildWindow {
        public StartGiveaway() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.StartGiveaway();
        }
    }
}