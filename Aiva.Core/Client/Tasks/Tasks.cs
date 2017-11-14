using System;
using TwitchLib;
using TwitchLib.Events.Client;

namespace Aiva.Core.Client.Tasks {
    public class Tasks {
        #region Models
        /// <summary>
        /// Error from TwitchLib cause "OnModeratorsReceived" ist null.
        /// Listen to this event from the Client fíx this issue
        /// </summary>
        public event EventHandler<OnModeratorsReceivedArgs> OnModeratorsReceivedEvent;

        private Client.Internal.Currency _currencyTimer;
        private readonly Client.Internal.Commands.Commands _commands;
        private readonly DatabaseHandlers.Users _usersHandler;
        #endregion Models

        #region Constructor
        public Tasks() {
            _commands = new Internal.Commands.Commands();
            _usersHandler = new DatabaseHandlers.Users();
            SetCurrencyTimer();
        }

        private void SetCurrencyTimer() {
            if (Config.Config.Instance.Storage.Currency.AddCurrencyFrquently) {
                _currencyTimer = new Internal.Currency();
            }
        }
        #endregion Constructor

        #region Functions

        /// <summary>
        /// Set AivaClient Tasks
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public TwitchClient SetTasks(TwitchClient client) {
            client = OnModeratorsReceived(client);
            client = OnExistingUsersDetected(client);
            client = OnUserJoined(client);
            client = OnMessageReceived(client);
            client = OnUserLeft(client);
            client = OnNewSubscriber(client);
#pragma warning disable RCS1212 // Remove redundant assignment.
            client = ModCommands(client);
#pragma warning restore RCS1212 // Remove redundant assignment.

            return client;
        }

        public TwitchClient ModCommands(TwitchClient client) {
            client.OnChatCommandReceived += _commands.ModCommands.Currency.CommandReceived;

            return client;
        }

        /// <summary>
        /// Set OnNewSubscriber Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public TwitchClient OnNewSubscriber(TwitchClient client) {
            client.OnNewSubscriber += Internal.Chat.Client_OnNewSubscriber;

            return client;
        }

        /// <summary>
        /// Set OnUserLeft Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public TwitchClient OnUserLeft(TwitchClient client) {
            client.OnUserLeft += _usersHandler.Remove.RemoveUserFromActiveUsers;

            return client;
        }

        /// <summary>
        /// Set OnUserJoined Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public TwitchClient OnUserJoined(TwitchClient client) {
            client.OnUserJoined += _usersHandler.Add.AddUserToDatabase;

            return client;
        }

        /// <summary>
        /// Set OnExistingUsers Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public TwitchClient OnExistingUsersDetected(TwitchClient client) {
            client.OnExistingUsersDetected += _usersHandler.Add.AddUserToDatabase;

            return client;
        }

        /// <summary>
        /// Set OnMessageReceived Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public TwitchClient OnMessageReceived(TwitchClient client) {
            if (Config.Config.Instance.Storage.Chat.BlacklistWordsChecker)
                client.OnMessageReceived += ChatChecker.BlacklistWordsChecker;

            if (Config.Config.Instance.Storage.Chat.CapsChecker)
                client.OnMessageReceived += ChatChecker.CapsChecker;

            if (Config.Config.Instance.Storage.Chat.LinkChecker)
                client.OnMessageReceived += ChatChecker.LinkChecker;

            return client;
        }

        public TwitchClient OnModeratorsReceived(TwitchClient client) {
            client.OnModeratorsReceived += Client_OnModeratorsReceived;
            return client;
        }

        public void Client_OnModeratorsReceived(object sender, TwitchLib.Events.Client.OnModeratorsReceivedArgs e) {
            OnModeratorsReceivedEvent.Invoke(null, e);
        }
        #endregion Functions
    }
}