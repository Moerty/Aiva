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

        public FirstStartViewModel() {
            CommandManager.RegisterClassCommandBinding(new MahApps.Metro.Controls.MetroContentControl().GetType(), new CommandBinding(StartRequestCommand, StartRequest));
        }

        private async void StartRequest(object sender, ExecutedRoutedEventArgs e) {
            // Send Request
            Task.Run(() =>Client.TwitchAuthentication.Instance.SendRequestToBrowser());

            var result = await Client.TwitchAuthentication.Instance.GetAuthenticationValuesAsync();

            if(result != null) {
                Model.OAuthToken = result.Token;
                Model.Scopes = result.Scopes;
            }
        }
    }
}
