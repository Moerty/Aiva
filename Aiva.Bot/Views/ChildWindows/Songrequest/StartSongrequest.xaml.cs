using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Bot.Views.ChildWindows.Songrequest {

    /// <summary>
    /// Interaktionslogik für AddTimer.xaml
    /// </summary>
    public partial class StartSongrequest : ChildWindow {

        public StartSongrequest() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.Songrequest.StartSongrequest();
        }
    }
}