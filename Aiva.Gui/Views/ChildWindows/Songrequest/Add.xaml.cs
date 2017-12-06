using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Gui.Views.ChildWindows.Songrequest {
    /// <summary>
    /// Interaktionslogik für Add.xaml
    /// </summary>
    public partial class Add : ChildWindow {
        public Add() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.Songrequest.Add();
        }
    }
}