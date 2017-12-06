using MahApps.Metro.SimpleChildWindow;
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

namespace Aiva.Gui.Views.ChildWindows.Streamgames {
    /// <summary>
    /// Interaktionslogik für Bankheist.xaml
    /// </summary>
    public partial class Bankheist : ChildWindow {
        public Bankheist() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.Streamgames.Bankheist();
        }
    }
}
