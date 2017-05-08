using System;
using System.Collections.Generic;
using System.Linq;

namespace Aiva.Core.Database {
    public class Currency {

        public class Add {
            /// <summary>
            /// Add Currency frequently class
            /// </summary>
            public static class AddCurrencyFrequently {
                public static System.Timers.Timer Timer { get; private set; }
                /// <summary>
                /// Set AddCurrencyFrequently Timer
                /// </summary>
                public static void SetTimer() {
                    if (Convert.ToBoolean(Config.Config.Instance[nameof(Currency)][nameof(AddCurrencyFrequently)])) {
                        if (TimeSpan.TryParse(Config.Config.Instance[nameof(Currency)]["TimerAddCurrencyFrequently"], out TimeSpan Interval)) {
                            Timer = new System.Timers.Timer();
                            Timer.Elapsed += CurrencyTimerTick;
                            Timer.Interval = Interval.TotalMilliseconds;
                            Timer.AutoReset = true;
                            Timer.Start();
                        }
                    }
                }

                /// <summary>
                /// Timer tick
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private static void CurrencyTimerTick(object sender, System.Timers.ElapsedEventArgs e) {
                    using (var context = new Storage.StorageEntities()) {
                        var activeUsers = context.ActiveUsers.ToList();

                        foreach (var user in activeUsers) {
                            user.Users.Currency.Value += Convert.ToInt64(Config.Config.Instance[nameof(Currency)]["CurrencyToAddFrequently"]);
                        }

                        context.SaveChanges();
                    }
                }
            }

            /// <summary>
            /// Add list of Users Currency
            /// </summary>
            /// <param name="UserList">todo: describe UserList parameter on AddCurrencyToUserList</param>
            public async static void AddCurrencyToUserList(List<Models.DatabaseCurrencyModel.ListCurrencyUpdate> UserList) {
                using (var context = new Storage.StorageEntities()) {
                    foreach (var user in UserList) {
                        var userDb = context.Users.SingleOrDefault(u => String.Compare(u.Id, user.TwitchID) == 0);

                        if (userDb != null) {
                            userDb.Currency.Value += user.Value;
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }

            public static void AddCurrencyToUser(string twitchID, int value) {
                using (var context = new Storage.StorageEntities()) {
                    var user = context.Users.SingleOrDefault(u => String.Compare(u.Id, twitchID) == 0);

                    if (user != null) {
                        user.Currency.Value += value;

                        context.SaveChanges();
                    }
                }
            }
        }

        public class Remove {

            /// <summary>
            /// Remove Currency from User
            /// </summary>
            /// <param name="twitchID"></param>
            /// <param name="value"></param>
            public static void RemoveCurrencyFromUser(string twitchID, int value) {
                using (var context = new Storage.StorageEntities()) {
                    var user = context.Users.SingleOrDefault(u => String.Compare(u.Id, twitchID) == 0);

                    if (user != null) {
                        user.Currency.Value -= value;

                        context.SaveChanges();
                    }
                }
            }

            /// <summary>
            /// Remove list of Users Currency
            /// </summary>
            /// <param name="UserList"></param>
            public async static void RemoveCurrencyToUserList(List<Models.DatabaseCurrencyModel.ListCurrencyUpdate> UserList) {
                using (var context = new Storage.StorageEntities()) {
                    foreach (var user in UserList) {
                        var userDb = context.Users.SingleOrDefault(u => String.Compare(u.Id, user.TwitchID) == 0);

                        if (userDb != null) {
                            userDb.Currency.Value -= user.Value;
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }
        }

        public class Transfer {

        }

        /// <summary>
        /// Get Currency from a user
        /// </summary>
        /// <param name="twitchID">todo: describe twitchID parameter on GetCurrencyFromUser</param>
        /// <returns></returns>
        public static long? GetCurrencyFromUser(string twitchID) {
            using (var context = new Core.Storage.StorageEntities()) {
                var user = context.Users.SingleOrDefault(u => String.Compare(u.Id, twitchID) == 0);

                if (user != null) {
                    return user.Currency.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Add User to Table Currency
        /// </summary>
        /// <param name="twitchID"></param>
        public async static void AddUserToCurrencyTable(string twitchID) {
            using (var context = new Storage.StorageEntities()) {
                var currencyEntry = context.Currency.SingleOrDefault(c => String.Compare(c.ID, twitchID) == 0);

                if (currencyEntry == null) {
                    context.Currency.Add(
                        new Storage.Currency {
                            ID = twitchID,
                            Value = 0
                        });

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
