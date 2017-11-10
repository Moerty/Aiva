using MahApps.Metro.Controls;

namespace Aiva.Bot {

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {

        public MainWindow() {
            InitializeComponent();
            this.DataContext = new ViewModels.MainWindow();
        }
    }
}