using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackBoxBot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class FirstStartViewModel {
        public Models.FirstStartModel Model { get; set; } = new Models.FirstStartModel();
        public ICommand StartRequestCommand { get; set; } = new RoutedCommand();
        public ICommand StartRequestYouTubeCommand { get; set; } = new RoutedCommand();

        public FirstStartViewModel() {
            var type = new MahApps.Metro.Controls.MetroContentControl().GetType();
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(StartRequestCommand, StartRequest));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(StartRequestYouTubeCommand, StartRequestYoutube));
        }

        private void StartRequestYoutube(object sender, ExecutedRoutedEventArgs e) {
            Model.GoogleAuth = Songrequest.GoogleCheck.Authenticate();
        }

        private void SendRequest() {
            Task.Run(() => Client.TwitchAuthentication.Instance.SendRequestToBrowser());
        }

        private async void StartRequest(object sender, ExecutedRoutedEventArgs e) {
            // Send Request
            SendRequest();

            var result = await Client.TwitchAuthentication.Instance.GetAuthenticationValuesAsync();

            if(result != null) {
                Model.OAuthToken = result.Token;

                if (Model.Scopes == null) Model.Scopes = new System.Collections.ObjectModel.ObservableCollection<string>();
                foreach(var scope in result.Scopes.Split(' ')) {
                    Model.Scopes.Add(scope);
                }

                Model.TwitchAuth = true;
            }
        }
    }
}
