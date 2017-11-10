using System;
using TwitchLib;
using TwitchLib.Events.Client;

namespace Aiva.Core.Client.Tasks {
    public class Tasks {
        /// <summary>
        /// Error from TwitchLib cause "OnModeratorsReceived" ist null.
        /// Listen to this event from the Client fíx this issue
        /// </summary>
        public event EventHandler<OnModeratorsReceivedArgs> OnModeratorsReceivedEvent;

        private Client.Internal.Currency _currencyTimer;
        private Internal.Commands.ModCommands _modCommandsHandler;

        /// <summary>
        /// Set AivaClient Tasks
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public TwitchClient SetTasks(TwitchClient client) {
            _modCommandsHandler = new Internal.Commands.ModCommands();
            client = OnModeratorsReceived(client);
            client = OnExistingUsersDetected(client);
            client = OnUserJoined(client);
            client = OnMessageReceived(client);
            client = OnUserLeft(client);
            client = OnNewSubscriber(client);
            client = ModCommands(client);
            SetCurrencyTimer();

            return client;
        }

        private void SetCurrencyTimer() {
            if (Config.Config.Instance.Storage.Currency.AddCurrencyFrquently) {
                _currencyTimer = new Internal.Currency();
            }
        }

        public TwitchClient ModCommands(TwitchClient client) {
            client.OnChatCommandReceived += _modCommandsHandler.ParseModCommand;

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
            client.OnUserLeft += DatabaseHandlers.Users.Removeuser.RemoveUserFromActiveUsers;

            return client;
        }

        /// <summary>
        /// Set OnUserJoined Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public TwitchClient OnUserJoined(TwitchClient client) {
            client.OnUserJoined += DatabaseHandlers.Users.AddUser.AddUserToDatabase;

            return client;
        }

        /// <summary>
        /// Set OnExistingUsers Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public TwitchClient OnExistingUsersDetected(TwitchClient client) {
            client.OnExistingUsersDetected += DatabaseHandlers.Users.AddUser.AddUserToDatabase;

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
    }
}