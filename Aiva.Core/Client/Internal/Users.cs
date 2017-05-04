using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiva.Core.Models;
using TwitchLib;
using TwitchLib.Events.Client;

namespace Aiva.Core.Client.Internal {
    public class Users {
        public static event EventHandler<OnNewUserFoundArgs> OnNewUserFound;

        public static void InvokeOnNewUserFound(List<string> Usernames) {
            var userList = new List<TwitchLib.Models.API.User.User>();

            foreach (var user in Usernames) {
                if (!Database.Users.IsUserInDatabase(user)) {
                    var twitchUser = TwitchApi.Users.GetUser(user);

                    if (twitchUser != null) {
                        userList.Add(twitchUser);
                    }
                }
            }

            if(userList.Any())
                OnNewUserFound?.Invoke(null, new OnNewUserFoundArgs { User = userList });
        }

        /// <summary>
        /// Fires when User joined the Channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnUserJoined(object sender, OnUserJoinedArgs e) {
            var user = TwitchApi.Users.GetUser(e.Username);

            InvokeUserJoined(new List<TwitchLib.Models.API.User.User> { user });
        }

        /// <summary>
        /// Fires when existing Users detected in Channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnExistingUserJoined(object sender, OnExistingUsersDetectedArgs e) {
            var userList = new List<TwitchLib.Models.API.User.User>();

            foreach(var user in e.Users) {
                var twitchUser = TwitchApi.Users.GetUser(user);

                if (twitchUser != null)
                    userList.Add(twitchUser);
            }

            if (userList.Any())
                InvokeUserJoined(userList);
        }

        /// <summary>
        /// Invoke the Eventhandler
        /// </summary>
        /// <param name="userList"></param>
        private static void InvokeUserJoined(List<TwitchLib.Models.API.User.User> userList) {
            OnNewUserFound?.Invoke(
                null, new OnNewUserFoundArgs {
                    User = userList
                });
        }
    }
}
