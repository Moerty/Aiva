namespace Aiva.Bot.Views {
    /// <summary>
    /// Interaktionslogik für Console.xaml
    /// </summary>
    public partial class Console : MahApps.Metro.Controls.MetroContentControl {
        public Console() {
            InitializeComponent();
            this.DataContext = new ViewModels.Console();
        }
    }
}