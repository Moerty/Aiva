using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class Console {

        public Models.Console Model { get; set; }
        public Extensions.Chat.Chat ChatHandler { get; set; }

        public ICommand SendMessageCommand { get; set; } = new RoutedCommand();

        // ContextMenu Commands
        public ICommand MuteCommand { get; set; } = new RoutedCommand();
        public ICommand UnmuteCommand { get; set; } = new RoutedCommand();
        public ICommand ModCommand { get; set; } = new RoutedCommand();
        public ICommand UnmodCommand { get; set; } = new RoutedCommand();
        public ICommand CopyMessageCommand { get; set; } = new RoutedCommand();
        public ICommand CopyTwitchUsernameCommand { get; set; } = new RoutedCommand();
        public ICommand ShowUserInfo { get; set; } = new RoutedCommand();

        public Console() {
            Model = new Models.Console();
            ChatHandler = new Extensions.Chat.Chat();

            SendMessageCommand = new Internal.RelayCommand(u => SendMessage(), s => Model.CanSendMessage);
            UnmuteCommand = new Internal.RelayCommand(um => UnmuteUser(), um => true);
            MuteCommand = new Internal.RelayCommand(um => MuteUser(), um => true);
            ModCommand = new Internal.RelayCommand(um => ModUser(), um => true);
            UnmodCommand = new Internal.RelayCommand(um => UnmodUser(), um => true);
            ShowUserInfo = new Internal.RelayCommand(u => ShowUserinfo(), u => true);
        }

        private void ShowUserinfo()
        {
            MainWindow.Instance.SelectedTab.Flyouts[0].DataContext = new ViewModels.Flyouts.UsersInfoVM(ChatHandler.SelectedViewer.Name);
            MainWindow.Instance.SelectedTab.Flyouts[0].IsOpen = true;
        }

        /// <summary>
        /// Unmod User
        /// </summary>
        private void UnmodUser() => Core.Client.Internal.Stream.UnmodUser(ChatHandler.SelectedViewer.Name);

        /// <summary>
        /// Mod User
        /// </summary>
        private void ModUser() => Core.Client.Internal.Stream.ModUser(ChatHandler.SelectedViewer.Name);

        /// <summary>
        /// Mute Viewer
        /// </summary>
        private void MuteUser() => Core.Client.Internal.Chat.MuteUser(ChatHandler.SelectedViewer.Name);

        /// <summary>
        /// Unmute Viewer
        /// </summary>
        private void UnmuteUser() => Core.Client.Internal.Chat.UnmuteUser(ChatHandler.SelectedViewer.Name);

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
