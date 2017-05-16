namespace Aiva.Bot.Views {
    /// <summary>
    /// Interaktionslogik für Commands.xaml
    /// </summary>
    public partial class Timers : MahApps.Metro.Controls.MetroContentControl {
        public Timers() {
            InitializeComponent();
            this.DataContext = new ViewModels.Commands();
        }
    }
}
