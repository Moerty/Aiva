namespace Aiva.Bot.Views {
    /// <summary>
    /// Interaktionslogik für Commands.xaml
    /// </summary>
    public partial class Commands : MahApps.Metro.Controls.MetroContentControl {
        public Commands() {
            InitializeComponent();
            this.DataContext = new ViewModels.Commands();
        }
    }
}
