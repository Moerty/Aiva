using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class ConsoleVM {

        public Models.Console Model { get; set; }
        public Extensions.Chat.Chat ChatHandler { get; set; }

        public ICommand SendMessageCommand { get; set; } = new RoutedCommand();

        public ConsoleVM() {
            Model = new Models.Console();
            ChatHandler = new Extensions.Chat.Chat();


            var type = new MahApps.Metro.Controls.MetroContentControl().GetType();
            SendMessageCommand = new Internal.RelayCommand(u => SendMessage(), s => Model.CanSendMessage);
        }

        /// <summary>
        /// Send the Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendMessage(object sender = null, ExecutedRoutedEventArgs e = null) {
            Extensions.Chat.Chat.Instance.SendMessage(Model.MessageToSend);

            Model.MessageToSend = String.Empty;
        }
    }
}
