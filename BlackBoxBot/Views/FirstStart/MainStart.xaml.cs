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
using System.Windows.Shapes;

namespace BlackBoxBot.Views.FirstStart {
    /// <summary>
    /// Interaktionslogik für MainStart.xaml
    /// </summary>
    public partial class MainStart : MahApps.Metro.Controls.MetroWindow {
        public MainStart() {
            InitializeComponent();
            this.DataContext = new ViewModels.FirstStartViewModel();
        }
    }
}
