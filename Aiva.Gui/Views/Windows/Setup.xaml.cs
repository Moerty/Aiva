using MahApps.Metro.Controls;

namespace Aiva.Gui.Views.Windows {
    /// <summary>
    /// Interaktionslogik für Setup.xaml
    /// </summary>
    public partial class Setup : MetroWindow {
        public Setup() {
            InitializeComponent();
            this.DataContext = new ViewModels.Windows.Setup();
        }
    }
}