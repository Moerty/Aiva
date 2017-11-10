using MahApps.Metro.Controls;

namespace Aiva.Bot.Views {

    /// <summary>
    /// Interaktionslogik für Timers.xaml
    /// </summary>
    public partial class Timers : MetroContentControl {

        public Timers() {
            InitializeComponent();
            this.DataContext = new ViewModels.Timers();
        }
    }
}