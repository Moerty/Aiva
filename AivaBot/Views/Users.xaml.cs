using System.Linq;
using Database;

namespace AivaBot.Views {
    /// <summary>
    /// Interaktionslogik für pUsers.xaml
    /// </summary>
    public partial class Users : MahApps.Metro.Controls.MetroWindow {
        public Users() {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e) {

            System.Windows.Data.CollectionViewSource usersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("usersViewSource")));
            // Laden Sie Daten durch Festlegen der CollectionViewSource.Source-Eigenschaft:
            // usersViewSource.Source = [generische Datenquelle]

            using (var context = new DatabaseEntities()) {
                usersViewSource.Source = context.Users.ToList();
            }
        }
    }
}
