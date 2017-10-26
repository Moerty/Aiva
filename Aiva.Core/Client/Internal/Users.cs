using System;
using System.Collections.Generic;
using System.Linq;
using Aiva.Core.Models;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Enums;

namespace Aiva.Core.Client.Internal {
    public class Users {
        public static event EventHandler<OnNewUserFoundArgs> OnNewUserFound;

        public static void InvokeOnNewUserFound(List<string> UserList) {
            var userList = new List<TwitchLib.Models.API.v5.Users.User>();

            foreach (var user in UserList) {
                if (!Database.Users.IsUserInDatabase(user)) {
                    var twitchUser = TwitchAPI.Users.v5.GetUserByNameAsync(user).Result; //TwitchApi.Users.GetUser(user);

                    if (twitchUser != null && twitchUser.Total > 0) {

                        foreach (var userMatch in twitchUser.Matches) {
                            if (String.Compare(user, userMatch.Name, true) != 0)
                                continue;

                            userList.Add(userMatch);
                        }
                    }
                }
            }

            if (userList.Any())
                OnNewUserFound?.Invoke(null, new OnNewUserFoundArgs { Users = userList });
        }

        /// <summary>
        /// Fires when User joined the Channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async static void OnUserJoined(object sender, OnUserJoinedArgs e) {
            var user = await TwitchAPI.Users.v5.GetUserByNameAsync(e.Username);

            if (user != null && user.Total > 0) {
                foreach (var userMatch in user.Matches) {
                    if (String.Compare(userMatch.Name, e.Username, true) != 0)
                        continue;

                    InvokeUserJoined(new List<TwitchLib.Models.API.v5.Users.User> { userMatch });
                }
            }


        }

        /// <summary>
        /// Fires when existing Users detected in Channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async static void OnExistingUserJoined(object sender, OnExistingUsersDetectedArgs e) {
            //var userList = new List<TwitchLib.Models.API.User.User>();
            var userList = new List<TwitchLib.Models.API.v5.Users.User>();

            foreach (var user in e.Users) {
                var twitchUser = await TwitchAPI.Users.v5.GetUserByNameAsync(user);

                if (twitchUser != null && twitchUser.Total > 0) {
                    foreach (var userMatch in twitchUser.Matches) {
                        if (String.Compare(userMatch.Name, user) != 0)
                            continue;


                        userList.Add(userMatch);
                    }
                }
            }

            if (userList.Any())
                InvokeUserJoined(userList);
        }

        /// <summary>
        /// Invoke the Eventhandler
        /// </summary>
        /// <param name="userList"></param> 
        private static void InvokeUserJoined(List<TwitchLib.Models.API.v5.Users.User> userList) {
            OnNewUserFound?.Invoke(
                null, new OnNewUserFoundArgs {
                    Users = userList
                });
        }
    }
}
