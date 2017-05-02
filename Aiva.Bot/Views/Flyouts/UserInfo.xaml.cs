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

namespace Aiva.Bot.Views.Flyouts
{
    /// <summary>
    /// Interaktionslogik für UserInfo.xaml
    /// </summary>
    /// 
    [PropertyChanged.ImplementPropertyChanged]
    public partial class UserInfo : MahApps.Metro.Controls.MetroContentControl
    {
        public new object DataContext { get; set; }

        public UserInfo(string name)
        {
            InitializeComponent();
            this.DataContext = new ViewModels.Flyouts.UsersInfoVM(name);
        }
    }
}
