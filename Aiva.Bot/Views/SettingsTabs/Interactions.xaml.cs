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

namespace Aiva.Bot.Views.SettingsTabs
{
    /// <summary>
    /// Interaktionslogik für Interactions.xaml
    /// </summary>
    public partial class Interactions : MahApps.Metro.Controls.MetroContentControl
    {
        public Interactions()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.SettingsViewModel.InteractionsViewModel();
        }
    }
}
