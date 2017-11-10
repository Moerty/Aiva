namespace Aiva.Bot.Views {
    /// <summary>
    /// Interaktionslogik für Users.xaml
    /// </summary>
    public partial class Users : MahApps.Metro.Controls.MetroWindow {
        public Users() {
            InitializeComponent();
            this.DataContext = new ViewModels.Users();
        }
    }
}