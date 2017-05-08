using System;
using System.Timers;
using TwitchLib;
using System.Linq;
using System.Collections.Generic;

namespace Aiva.Core.Client.Tasks {
    public class Tasks {

        /// <summary>
        /// Set AivaClient Tasks
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        public static TwitchClient SetClientTasks(TwitchClient Client) {

            Client = OnExistingUsersDetected(Client);
            Client = OnUserJoined(Client);
            Client = OnMessageReceived(Client);
            Client = OnUserLeft(Client);
            Client = OnNewSubscriber(Client);
            Client = ModCommands(Client);

            SetTimers();

            return Client;
        }

        private static TwitchClient ModCommands(TwitchClient client) {
            client.OnChatCommandReceived += Internal.Commands.ModCommands.ParseModCommand;

            return client;
        }

        /// <summary>
        /// Set OnNewSubscriber Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static TwitchClient OnNewSubscriber(TwitchClient client) {
            client.OnNewSubscriber += Internal.Chat.Client_OnNewSubscriber;

            return client;
        }

        /// <summary>
        /// Set OnUserLeft Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static TwitchClient OnUserLeft(TwitchClient client) {
            client.OnUserLeft += Database.Users.Removeuser.RemoveUserFromActiveUsers;

            return client;
        }

        /// <summary>
        /// Set OnUserJoined Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static TwitchClient OnUserJoined(TwitchClient client) {
            //client.OnUserJoined += Database.Users.AddUser.AddUserToDatabase;
            client.OnUserJoined += Internal.Users.OnUserJoined;
            Internal.Users.OnNewUserFound += Database.Users.AddUser.AddUserToDatabase;

            return client;
        }

        /// <summary>
        /// Set OnExistingUsers Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static TwitchClient OnExistingUsersDetected(TwitchClient client) {
            //client.OnExistingUsersDetected += Database.Users.AddUser.AddUserToDatabase;
            client.OnExistingUsersDetected += Internal.Users.OnExistingUserJoined;
            Internal.Users.OnNewUserFound += Database.Users.AddUser.AddUserToDatabase;

            return client;
        }

        /// <summary>
        /// Set OnMessageReceived Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static TwitchClient OnMessageReceived(TwitchClient client) {

            if (Convert.ToBoolean(Config.Config.Instance["Chat"]["BlacklistedWordsChecker"]))
                client.OnMessageReceived += ChatChecker.BlacklistWordsChecker;

            if (Convert.ToBoolean(Config.Config.Instance["Chat"]["CapsChecker"]))
                client.OnMessageReceived += ChatChecker.CapsChecker;

            if (Convert.ToBoolean(Config.Config.Instance["Chat"]["LinkChecker"]))
                client.OnMessageReceived += ChatChecker.LinkChecker;

            return client;
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
            var users = await TwitchAPI.Undocumented.GetChatters(Core.AivaClient.Instance.Channel);

            if (users != null && users.ChatterCount > 0) {
                var userList = new List<string>();

                // Admins
                if (users.Chatters.Admins.Any()) {
                    foreach (var chatter in users.Chatters.Admins) {
                        userList.Add(chatter);
                    }
                }

                // Global Mods
                if (users.Chatters.GlobalMods.Any()) {
                    foreach (var chatter in users.Chatters.GlobalMods) {
                        userList.Add(chatter);
                    }
                }

                // Mod
                if (users.Chatters.Moderators.Any()) {
                    foreach (var chatter in users.Chatters.Moderators) {
                        userList.Add(chatter);
                    }
                }

                // Staff
                if (users.Chatters.Staff.Any()) {
                    foreach (var chatter in users.Chatters.Staff) {
                        userList.Add(chatter);
                    }
                }

                // Viewers
                if (users.Chatters.Viewers.Any()) {
                    foreach (var chatter in users.Chatters.Viewers) {
                        userList.Add(chatter);
                    }
                }

                Internal.Users.InvokeOnNewUserFound(userList);
            }
        }

        static Timer ChatUsersCheckerTimer;

        #endregion Timers
    }
}
