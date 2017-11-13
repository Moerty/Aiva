using System;
using System.Linq;
using TwitchLib.Events.Client;

namespace Aiva.Core.DatabaseHandlers {
    public static class Users {
        /// <summary>
        /// Add User Class
        /// </summary>
        public static class AddUser {
            /// <summary>
            /// Add or Update User in Database
            /// </summary>
            /// <param name="User">todo: describe User parameter on AddUser</param>
            private static void AddUserToDatabase(TwitchLib.Models.API.v5.Users.User User) {
                if (User != null) {
                    using (var context = new Storage.StorageEntities()) {
                        var databaseUser = context.Users.SingleOrDefault(u => String.Compare(u.Id, User.Id) == 0);

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
                                    ID = databaseUser.Id,
                                    JoinedTime = DateTime.Now,
                                    Users = databaseUser,
                                };
                            }
                        }
                        // Create User
                        else {
                            var newUser = new Storage.Users {
                                Id = User.Id,
                                Bio = User.Bio,
                                CreatedAt = User.CreatedAt,
                                DisplayName = User.DisplayName,
                                Logo = User.Logo,
                                Name = User.Name,
                                Type = User.Type,
                                UpdatedAt = User.UpdatedAt,
                                Currency = new Storage.Currency {
                                    Value = 0,
                                },
                                ActiveUsers = new Storage.ActiveUsers {
                                    JoinedTime = DateTime.Now,
                                },
                                TimeWatched = new Storage.TimeWatched {
                                    TimeWatched1 = 0,
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
            internal static void AddUserToDatabase(object sender, OnExistingUsersDetectedArgs e) {
                e.Users.ForEach(user => {
                    var twitchUser = AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(user).Result;
                    AddUserToDatabase(twitchUser.Matches[0]);
                });
            }

            internal static void AddUserToDatabase(object sender, OnUserJoinedArgs e) {
                var twitchUser = AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(e.Username).Result;

                if (twitchUser != null) {
                    AddUserToDatabase(twitchUser.Matches[0]);
                }
            }
        }

        /// <summary>
        /// Remove User class
        /// </summary>
        public static class Removeuser {
            /// <summary>
            /// Remove ActiveUser Entry
            /// and add TimeWatched values
            /// </summary>
            /// <param name="sender">todo: describe sender parameter on RemoveUserFromActiveUsers</param>
            /// <param name="e">todo: describe e parameter on RemoveUserFromActiveUsers</param>
            public async static void RemoveUserFromActiveUsers(object sender, OnUserLeftArgs e) {
                using (var context = new Storage.StorageEntities()) {
                    //var twitchID = TwitchApi.Users.GetUser(e.Username);
                    var twitchID = await AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(e.Username).ConfigureAwait(false);

                    if (twitchID?.Total > 0) {
                        foreach (var userMatch in twitchID.Matches) {
                            if (String.Compare(userMatch.Name, e.Username, true) != 0)
                                continue;

                            var user = context.Users.SingleOrDefault(u => String.Compare(u.Id, userMatch.Id) == 0);

                            if (user != null) {
                                var duration = DateTime.Now.Subtract(user.ActiveUsers.JoinedTime.Value);

                                user.TimeWatched.TimeWatched1 += duration.Ticks;

                                context.ActiveUsers.Remove(user.ActiveUsers);

                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if the user is in the Database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool IsUserInDatabase(string username) {
            using (var context = new Storage.StorageEntities()) {
                return context.Users.Any(u => String.Compare(u.Name, username, true) == 0);
            }
        }
    }
}