using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aiva.Bot.Views
{
    /// <summary>
    /// Interaktionslogik für Users.xaml
    /// </summary>
    public partial class Users : MahApps.Metro.Controls.MetroWindow
    {
        public Users()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.Users();
        }
    }
}
