namespace Aiva.Core.Client.Tasks.Functions {
    class TimeWatched {
        /// <summary>
        /// Add User when joins channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void AddUser(object sender, TwitchLib.Events.Client.OnUserJoinedArgs e) {
            Database.ActiveUsersHandler.AddUserToList(e.Username);
        }

        public static void AddExistingUsers(object sender, TwitchLib.Events.Client.OnExistingUsersDetectedArgs e) {
            Database.ActiveUsersHandler.AddUserToList(e.Users);
        }

        public static void RemoveUser(object sender, TwitchLib.Events.Client.OnUserLeftArgs e) {
            Database.ActiveUsersHandler.AddTimeWatchedToDatabase(
                e.Username,
                Database.ActiveUsersHandler.GetJoinedTime(e.Username));
        }
    }
}
