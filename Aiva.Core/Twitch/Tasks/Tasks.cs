using System;
using TwitchLib;
using TwitchLib.Events.Client;

namespace Aiva.Core.Twitch.Tasks {
    public class Tasks {
        /// <summary>
        /// Error from TwitchLib cause "OnModeratorsReceived" ist null.
        /// Listen to this event from the Client fíx this issue
        /// </summary>
        public event EventHandler<OnModeratorsReceivedArgs> OnModeratorsReceivedEvent;

        private Currency _currencyTimer;
        private readonly Commands.Handler _commandsHandler;
        private readonly Database.Handlers.Users _databaseUsersHandler;

        public Tasks() {
            _commandsHandler = new Commands.Handler();
            _databaseUsersHandler = new Database.Handlers.Users();
            SetCurrencyTimer();
        }

        private void SetCurrencyTimer() {
            if (Config.Config.Instance.Storage.Currency.AddCurrencyFrquently) {
                _currencyTimer = new Currency();
            }
        }

        public void SetTasks(ref TwitchClient client) {
            OnModeratorsReceived(ref client);
            OnExistingUsersDetected(ref client);
            OnUserJoined(ref client);
            OnUserLeft(ref client);
            OnNewSubscriber(ref client);
            ModCommands(ref client);
            CurrencyCommands(ref client);
        }

        private void CurrencyCommands(ref TwitchClient client) {
            client.OnChatCommandReceived += _commandsHandler.CurrencyCommands.CommandReceived;
        }

        private void ModCommands(ref TwitchClient client) {
            client.OnChatCommandReceived += _commandsHandler.ModCommands.Currency.CommandReceived;
            client.OnChatCommandReceived += _commandsHandler.ModCommands.CommandReceived;
        }

        private void OnNewSubscriber(ref TwitchClient client) {
            client.OnNewSubscriber += Chat.Client_OnNewSubscriber;
        }

        private void OnUserLeft(ref TwitchClient client) {
            client.OnUserLeft += _databaseUsersHandler.Remove.RemoveUserFromActiveUsers;
        }

        private void OnUserJoined(ref TwitchClient client) {
            client.OnUserJoined += _databaseUsersHandler.Add.AddUserToDatabase;
        }

        private void OnExistingUsersDetected(ref TwitchClient client) {
            client.OnExistingUsersDetected += _databaseUsersHandler.Add.AddUserToDatabase;
        }

        private void OnModeratorsReceived(ref TwitchClient client) {
            client.OnModeratorsReceived += Client_OnModeratorsReceived;
        }

        private void Client_OnModeratorsReceived(object sender, OnModeratorsReceivedArgs e) {
            OnModeratorsReceivedEvent.Invoke(this, e);
        }
    }
}