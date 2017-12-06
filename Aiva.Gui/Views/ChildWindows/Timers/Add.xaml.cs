using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Gui.Views.ChildWindows.Timers {
    /// <summary>
    /// Interaktionslogik für Add.xaml
    /// </summary>
    public partial class Add : ChildWindow {
        public Add() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.Timers.Add();
        }

        public Add(string name, string text, int interval, int id) {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.Timers.Add {
                Name = name,
                Text = text,
                Interval = interval,
                DatabaseID = id,
                IsEditing = true
            };
        }
    }
}