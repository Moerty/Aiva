using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

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
                += (sender, e) => SetMessageAndStartAnimation(e);

            Core.Twitch.AivaClient.Instance.Events.ShowNewSub
                += (sender, e) => SetNameAndStartAnimation(e);
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

        public void SetMessageAndStartAnimation(string text) {
            Application.Current.Dispatcher.Invoke(() => EventMessageBox.Text = text);
            StartAnimationMessage();
        }

        public void SetNameAndStartAnimation(string text) {
            Application.Current.Dispatcher.Invoke(() => EventName.Text = text);
            StartAnimationName();
        }
    }
}
