using TwitchLib.Events.Client;

namespace Aiva.Core.Twitch.Tasks.Commands.ModCommands {
    public class Handler {
        #region Models
        public Currency Currency;
        public EditCommands EditCommands;
        private readonly Database.Handlers.Commands _commandsDatabaseHandler;
        #endregion Models

        #region Contructor
        public Handler() {
            Currency = new Currency();
            _commandsDatabaseHandler = new Database.Handlers.Commands();
            EditCommands = new EditCommands();
        }
        #endregion Contructor

        #region Functions
        /// <summary>
        /// Receiver if a permitted user want to add / edit / remove a command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CommandReceived(object sender, OnChatCommandReceivedArgs e) {
            if (e.Command.ChatMessage.UserType != TwitchLib.Enums.UserType.Viewer
                && e.Command.ChatMessage.UserType != TwitchLib.Enums.UserType.Staff) {
                if (string.Compare(e.Command.CommandText, "command", false) == 0) {
                    if (e.Command.ArgumentsAsList?.Count >= 0) {
                        switch (e.Command.ArgumentsAsList[0]) {
                            case "add":
                                if (e.Command.ArgumentsAsList.Count >= 2) {
                                    _commandsDatabaseHandler
                                    .AddCommand(
                                        creater: e.Command.ChatMessage.DisplayName,
                                        commandName: e.Command.ArgumentsAsList[1],
                                        text: e.Command.ArgumentsAsList[2]);
                                }

                                break;
                            case "edit":
                                if (e.Command.ArgumentsAsList.Count >= 2)
                                    _commandsDatabaseHandler.EditCommand(commandName: e.Command.ArgumentsAsList[1], text: e.Command.ArgumentsAsList[2]);
                                break;
                            case "remove":
                                _commandsDatabaseHandler.RemoveCommand(e.Command.ArgumentsAsList[1]);
                                break;
                        }
                    }
                }
            }
        }
        #endregion Functions
    }
}