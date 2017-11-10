using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Bot.Views.ChildWindows.Songrequest {
    /// <summary>
    /// Interaktionslogik für AddTimer.xaml
    /// </summary>
    public partial class AddSong : ChildWindow {
        public AddSong() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.Songrequest.AddSong();
        }
    }
}