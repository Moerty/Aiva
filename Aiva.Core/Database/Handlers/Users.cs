using Aiva.Core.Database.Storage;
using Aiva.Core.Twitch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Events.Client;

namespace Aiva.Core.Database.Handlers {
    public class Users {
        #region Models
        public AddUser Add;
        public Removeuser Remove;
        #endregion Models

        #region Constructor
        public Users() {
            Add = new AddUser();
            Remove = new Removeuser();
        }
        #endregion Constructor

        #region Add
        /// <summary>
        /// Add User Class
        /// </summary>
        public class AddUser {
            /// <summary>
            /// Add or Update User in Database
            /// </summary>
            /// <param name="User">todo: describe User parameter on AddUser</param>
            public void AddUserToDatabase(TwitchLib.Models.API.v5.Users.User User) {
                if (User != null) {
                    using (var context = new Storage.DatabaseContext()) {
                        var databaseUser = context.Users
                            .Include(au => au.ActiveUsers)
                            .SingleOrDefault(u => u.TwitchUser == Convert.ToInt32(User.Id));

                        // Update User
                        if (databaseUser != null) {
                            databaseUser.Bio = User.Bio;
                            databaseUser.CreatedAt = User.CreatedAt;
                            databaseUser.DisplayName = User.DisplayName;
                            databaseUser.Logo = User.Logo;
                            databaseUser.Name = User.Name;
                            databaseUser.Type = User.Type;
                            databaseUser.UpdatedAt = User.UpdatedAt;

                            // Active users
                            if (databaseUser.ActiveUsers == null) {
                                databaseUser.ActiveUsers = new Storage.ActiveUsers {
                                    TwitchUser = databaseUser.TwitchUser,
                                    JoinedTime = DateTime.Now,
                                    Users = databaseUser,
                                };
                            }
                        }
                        // Create User
                        else {
                            var newUser = new Storage.Users {
                                TwitchUser = Convert.ToInt32(User.Id),
                                Bio = User.Bio,
                                CreatedAt = User.CreatedAt,
                                DisplayName = User.DisplayName,
                                Logo = User.Logo,
                                Name = User.Name,
                                Type = User.Type,
                                UpdatedAt = User.UpdatedAt,
                                Currency = new Storage.Currency {
                                    TwitchUser = Convert.ToInt32(User.Id),
                                    Value = 0,
                                },
                                ActiveUsers = new Storage.ActiveUsers {
                                    TwitchUser = Convert.ToInt32(User.Id),
                                    JoinedTime = DateTime.Now,
                                },
                                TimeWatched = new Storage.TimeWatched {
                                    TwitchUser = Convert.ToInt32(User.Id),
                                    Time = 0,
                                }
                            };

                            context.Users.Add(newUser);
                        }

                        context.SaveChanges();
                    }
                }
            }

            /// <summary>
            /// Fires when new Users joined and add them
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            internal void AddUserToDatabase(object sender, OnExistingUsersDetectedArgs e) {
                e.Users.ForEach(user => {
                    var twitchUser = AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(user).Result;
                    AddUserToDatabase(twitchUser.Matches[0]);
                });
            }

            internal void AddUserToDatabase(object sender, OnUserJoinedArgs e) {
                var twitchUser = AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(e.Username).Result;

                if (twitchUser != null) {
                    AddUserToDatabase(twitchUser.Matches[0]);
                }
            }
        }
        #endregion Add

        #region Remove
        /// <summary>
        /// Remove User class
        /// </summary>
        public class Removeuser {
            /// <summary>
            /// Remove ActiveUser Entry
            /// and add TimeWatched values
            /// </summary>
            /// <param name="sender">todo: describe sender parameter on RemoveUserFromActiveUsers</param>
            /// <param name="e">todo: describe e parameter on RemoveUserFromActiveUsers</param>
            public async void RemoveUserFromActiveUsers(object sender, OnUserLeftArgs e) {
                using (var context = new Storage.DatabaseContext()) {
                    //var twitchID = TwitchApi.Users.GetUser(e.Username);
                    var twitchID = await AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(e.Username).ConfigureAwait(false);

                    if (twitchID?.Total > 0) {
                        foreach (var userMatch in twitchID.Matches) {
                            if (String.Compare(userMatch.Name, e.Username, true) != 0)
                                continue;

                            var user = context.Users.SingleOrDefault(
                                u => u.TwitchUser == Convert.ToInt32(userMatch.Id));

                            if (user != null) {
                                var duration = DateTime.Now.Subtract(user.ActiveUsers.JoinedTime);

                                user.TimeWatched.Time += duration.Ticks;

                                context.ActiveUsers.Remove(user.ActiveUsers);

                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
        #endregion Remove

        #region Functions
        /// <summary>
        /// Check if the user is in thedDatabase
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUserInDatabaseUsername(string username) {
            using (var context = new Storage.DatabaseContext()) {
                return context.Users.Any(u => String.Compare(u.Name, username, true) == 0);
            }
        }

        /// <summary>
        /// Check if the user is in the database
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool IsUserInDatabaseUserId(int userid) {
            using (var context = new Storage.DatabaseContext()) {
                return context.Users.Any(u => u.TwitchUser == userid);
            }
        }

        /// <summary>
        /// Returns all Users in Database
        /// </summary>
        /// <returns></returns>
        public List<Storage.Users> GetUsers() {
            using (var context = new DatabaseContext()) {
                return context.Users.ToList();
            }
        }

        #endregion Functions
    }
}