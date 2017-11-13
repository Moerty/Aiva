using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Bot.Views.ChildWindows {
    /// <summary>
    /// Interaktionslogik für AddTimer.xaml
    /// </summary>
    public partial class AddTimer : ChildWindow {
        public AddTimer() {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.AddTimer();
        }

        /// <summary>
        /// Another constructor when editing a timer
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="interval"></param>
        /// <param name="id"></param>
        public AddTimer(string name, string text, int interval, long id) {
            InitializeComponent();
            this.DataContext = new ViewModels.ChildWindows.AddTimer {
                Name = name,
                Text = text,
                Interval = interval,
                IsEditing = true,
                DatabaseID = id
            };
        }
    }
}