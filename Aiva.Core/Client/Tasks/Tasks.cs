using System;
using System.Timers;
using TwitchLib;
using System.Linq;
using System.Collections.Generic;
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

            SetTimers();

            return client;
        }

        private void SetCurrencyTimer() {
            if (Convert.ToBoolean(Config.Config.Instance["Currency"]["AddCurrencyFrequently"])) {
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
            //client.OnUserJoined += Database.Users.AddUser.AddUserToDatabase;
            client.OnUserJoined += Internal.Users.OnUserJoined;
            Internal.Users.OnNewUserFound += DatabaseHandlers.Users.AddUser.AddUserToDatabase;

            return client;
        }

        /// <summary>
        /// Set OnExistingUsers Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public TwitchClient OnExistingUsersDetected(TwitchClient client) {
            //client.OnExistingUsersDetected += Database.Users.AddUser.AddUserToDatabase;
            client.OnExistingUsersDetected += Internal.Users.OnExistingUserJoined;
            Internal.Users.OnNewUserFound += DatabaseHandlers.Users.AddUser.AddUserToDatabase;

            return client;
        }

        /// <summary>
        /// Set OnMessageReceived Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public TwitchClient OnMessageReceived(TwitchClient client) {

            if (Convert.ToBoolean(Config.Config.Instance["Chat"]["BlacklistedWordsChecker"]))
                client.OnMessageReceived += ChatChecker.BlacklistWordsChecker;

            if (Convert.ToBoolean(Config.Config.Instance["Chat"]["CapsChecker"]))
                client.OnMessageReceived += ChatChecker.CapsChecker;

            if (Convert.ToBoolean(Config.Config.Instance["Chat"]["LinkChecker"]))
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


        #region Timers
        private static void SetTimers() {

            // ChatUsersCheckerTimer for undocumented Endpoint
            ChatUsersCheckerTimer = new Timer {
                Interval = new TimeSpan(0, 1, 0).TotalMilliseconds,
                AutoReset = true
            };
            ChatUsersCheckerTimer.Elapsed += TriggerChatUsersCheckerTimer;
            ChatUsersCheckerTimer.Start();
        }

        /// <summary>
        /// Triggers the Timer to check the undocumented Endpoint for Chatters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async static void TriggerChatUsersCheckerTimer(object sender, ElapsedEventArgs e) {
            var users = await TwitchAPI.Undocumented.GetChattersAsync(Core.AivaClient.Instance.Channel);


            if (users != null && users.Any()) {
                var UserList = new List<string>();


                foreach (var user in users) {
                    UserList.Add(user.Username);
                }

                Internal.Users.InvokeOnNewUserFound(UserList);
            }
        }

        static Timer ChatUsersCheckerTimer;

        #endregion Timers
    }
}
