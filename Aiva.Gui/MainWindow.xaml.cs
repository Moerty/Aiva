using MahApps.Metro.Controls;

namespace Aiva.Gui {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {
        public MainWindow() {
            InitializeComponent();
            this.DataContext = new ViewModels.Windows.MainWindow();
        }

        private void HamburgerMenuControl_OnItemClick(object sender, ItemClickEventArgs e)
        {
            this.HamburgerMenuControl.Content = e.ClickedItem;
            this.HamburgerMenuControl.IsPaneOpen = false;
        }
    }
}