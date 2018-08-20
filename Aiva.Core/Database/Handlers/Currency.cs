using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aiva.Core.Database.Handlers {
    public class Currency {
        #region Models
        public AddCurrency Add;
        public RemoveCurrency Remove;
        public TransferCurrency Transfer;
        #endregion Models

        #region Constructor
        public Currency() {
            Add = new AddCurrency();
            Remove = new RemoveCurrency();
            Transfer = new TransferCurrency();
        }
        #endregion Constructor

        #region Add
        public class AddCurrency {
            /// <summary>
            /// Add list of Users Currency
            /// </summary>
            /// <param name="UserList">todo: describe UserList parameter on AddCurrencyToUserList</param>
            public async void Add(List<Tuple<string, int, int>> UserList) {
                using (var context = new Storage.DatabaseContext()) {
                    foreach (var user in UserList) {
                        var userDb = context.Users.SingleOrDefault(u => u.UsersId == user.Item2);

                        if (userDb != null) {
                            userDb.Currency.Value += user.Item3;
                        }
                    }

                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }

            public bool Add(int twitchID, int value) {
                using (var context = new Storage.DatabaseContext()) {
                    var user = context.Users
                        .Include(c => c.Currency)
                        .SingleOrDefault(u => u.UsersId == twitchID);

                    if (user != null) {
                        user.Currency.Value += value;

                        context.SaveChanges();

                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Add currency to active viewers
            /// </summary>
            public void AddCurrencyActiveViewer() {
                using (var context = new Storage.DatabaseContext()) {
                    var activeUsers = context.ActiveUsers.ToList();

                    foreach (var user in activeUsers) {
                        user.Users.Currency.Value += Convert.ToInt64(Config.Config.Instance.Storage.Currency.CurrencyToAddFrequently);
                    }

                    context.SaveChanges();
                }
            }
        }
        #endregion Add

        #region Remove

        public class RemoveCurrency {
            /// <summary>
            /// Remove Currency from User
            /// </summary>
            /// <param name="twitchID"></param>
            /// <param name="value"></param>
            public bool Remove(int twitchID, int value) {
                using (var context = new Storage.DatabaseContext()) {
                    var user = context.Users
                        .Include(c => c.Currency)
                        .SingleOrDefault(u => u.UsersId == twitchID);

                    if (user != null) {
                        user.Currency.Value -= value;

                        context.SaveChanges();

                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Remove list of Users Currency
            /// </summary>
            /// <param name="UserList"></param>
            public async void Remove(List<Tuple<string, int, int>> UserList) {
                using (var context = new Storage.DatabaseContext()) {
                    foreach (var user in UserList) {
                        var userDb = context.Users.SingleOrDefault(u => u.UsersId == user.Item2);

                        if (userDb != null) {
                            userDb.Currency.Value -= user.Item3;
                        }
                    }

                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }
        #endregion Remove

        #region Transfer

        public class TransferCurrency {
            /// <summary>
            /// Transfer currency from a user to a user
            /// </summary>
            /// <param name="userid1"></param>
            /// <param name="userid2"></param>
            /// <param name="value"></param>
            internal bool Transfer(int userid1, int userid2, int value) {
                using (var context = new Storage.DatabaseContext()) {
                    var user1 = context.Users.SingleOrDefault(u => u.UsersId == userid1);
                    var user2 = context.Users.SingleOrDefault(u => u.UsersId == userid2);

                    if (user1 != null && user2 != null) {
                        if (user1.Currency.Value >= value) {
                            user1.Currency.Value -= value;
                            user2.Currency.Value += value;

                            context.SaveChanges();

                            return true;
                        }
                    }
                }

                return false;
            }
        }

        #endregion Transfer

        #region Functions

        /// <summary>
        /// Get Currency from a user
        /// </summary>
        /// <param name="twitchID">todo: describe twitchID parameter on GetCurrencyFromUser</param>
        /// <returns></returns>
        public long? GetCurrency(int twitchID) {
            using (var context = new Storage.DatabaseContext()) {
                var user = context.Users
                    .Include(u => u.Currency)
                    .SingleOrDefault(u => u.UsersId == twitchID);

                if (user != null) {
                    return user.Currency.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if a user has enough currency
        /// </summary>
        /// <param name="twitchID"></param>
        /// <param name="currencyToCheck"></param>
        /// <returns></returns>
        public bool HasUserEnoughCurrency(int twitchID, int currencyToCheck) {
            using (var context = new Storage.DatabaseContext()) {
                var user = context.Users.SingleOrDefault(u => twitchID == u.UsersId);

                if (user != null) {
                    if (user.Currency.Value >= currencyToCheck) {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Add User to Table Currency
        /// </summary>
        /// <param name="twitchID"></param>
        public async void AddUserToCurrencyTable(int twitchID) {
            using (var context = new Storage.DatabaseContext()) {
                var currencyEntry = context.Currency.SingleOrDefault(c => c.UsersId == twitchID);

                if (currencyEntry == null) {
                    context.Currency.Add(
                        new Storage.Currency {
                            UsersId = twitchID,
                            Value = 0
                        });

                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }

        #endregion Functions
    }
}