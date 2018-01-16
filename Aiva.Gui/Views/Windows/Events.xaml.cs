using System.Windows;

namespace Aiva.Gui.Views.Windows {
    /// <summary>
    /// Interaktionslogik für Events.xaml
    /// </summary>
    public partial class Events : Window {
        public Events() {
            InitializeComponent();

#if DEBUG
            EventMessageBox.Text = "Test Event Text";
#endif

            Core.Twitch.AivaClient.Instance.Events.ShowMessage
                += SetMessageAndStartAnimation;

            Core.Twitch.AivaClient.Instance.Events.ShowNewSub
                += SetNameAndStartAnimation;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            MessageAnimation.Begin();
        }

        public void StartAnimationName() {
            Application.Current.Dispatcher.Invoke(() => NameAnimation.Begin());
        }

        public void StartAnimationMessage() {
            Application.Current.Dispatcher.Invoke(() => MessageAnimation.Begin());
        }

        public void SetMessageAndStartAnimation(object sender, string text) {
            Application.Current.Dispatcher.Invoke(() => EventMessageBox.Text = text);
            StartAnimationMessage();
        }

        public void SetNameAndStartAnimation(object sender, string text) {
            Application.Current.Dispatcher.Invoke(() => EventName.Text = text);
            StartAnimationName();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Core.Twitch.AivaClient.Instance.Events.ShowMessage
                -= SetMessageAndStartAnimation;

            Core.Twitch.AivaClient.Instance.Events.ShowNewSub
                -= SetNameAndStartAnimation;
        }
    }
}