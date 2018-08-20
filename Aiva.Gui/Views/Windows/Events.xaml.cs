using Aiva.Core.Twitch;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using TwitchLib.Api.Interfaces;

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

            AivaClient.Instance.Events.ShowMessage
                += SetMessageAndStartAnimation;

            AivaClient.Instance.Events.ShowNewSub
                += SetNameAndStartAnimation;

            AivaClient.Instance.Events.OnNewFollower
                += OnNewFollower;
        }

        private void OnNewFollower(object sender, List<IFollow> e) {
            foreach(var follow in e) {
                SetNameAndStartAnimation(this, $"New follow: {follow.User.DisplayName}");
                StartAnimationName();
                Thread.Sleep(1000); // wait 1 sec to display new name
            }
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