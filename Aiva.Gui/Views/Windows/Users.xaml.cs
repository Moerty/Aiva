using MahApps.Metro.Controls;

namespace Aiva.Gui.Views.Windows {
    /// <summary>
    /// Interaktionslogik für Users.xaml
    /// </summary>
    public partial class Users : MetroWindow {
        public Users() {
            InitializeComponent();
            this.DataContext = new ViewModels.Windows.Users();
        }
    }
}