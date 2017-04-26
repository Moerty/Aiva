using System;
using TwitchLib;

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
            client.OnUserJoined += Database.Users.AddUser.AddUserToDatabase;

            return client;
        }

        /// <summary>
        /// Set OnExistingUsers Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static TwitchClient OnExistingUsersDetected(TwitchClient client) {
            client.OnUserJoined += Database.Users.AddUser.AddUserToDatabase;

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
    }
}
