using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {

    public class Console : INotifyPropertyChanged {

        #region Models
        public Models.Console Model { get; set; }
        public Extensions.Chat.Chat ChatHandler { get; set; }

        public ICommand SendMessageCommand { get; set; }
        // ContextMenu Commands
        public ICommand MuteCommand { get; set; }
        public ICommand UnmuteCommand { get; set; }
        public ICommand ModCommand { get; set; }
        public ICommand UnmodCommand { get; set; }
        public ICommand CopyMessageCommand { get; set; }
        public ICommand CopyTwitchUsernameCommand { get; set; }
        public ICommand ShowUserInfo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Models

        #region Constructor
        public Console() {
            Model = new Models.Console();
            ChatHandler = new Extensions.Chat.Chat();
            SetCommands();
        }

        #endregion Constructor

        #region Commands
        /// <summary>
        /// Set the Commands
        /// </summary>
        private void SetCommands() {
            SendMessageCommand = new Internal.RelayCommand(u => SendMessage(), s => Model.CanSendMessage);
            UnmuteCommand = new Internal.RelayCommand(um => UnmuteUser(), um => true);
            MuteCommand = new Internal.RelayCommand(um => MuteUser(), um => true);
            ModCommand = new Internal.RelayCommand(um => ModUser(), um => true);
            UnmodCommand = new Internal.RelayCommand(um => UnmodUser(), um => true);
            ShowUserInfo = new Internal.RelayCommand(u => ShowUserinfo(), u => true);
        }

        private void ShowUserinfo() {
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
        #endregion Commands
    }
}