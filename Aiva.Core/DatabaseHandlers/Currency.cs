using System;
using System.Collections.Generic;
using System.Linq;

namespace Aiva.Core.DatabaseHandlers {
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
            public async void Add(List<Models.DatabaseCurrencyModel.ListCurrencyUpdate> UserList) {
                using (var context = new Storage.StorageEntities()) {
                    foreach (var user in UserList) {
                        var userDb = context.Users.SingleOrDefault(u => String.Compare(u.Id, user.TwitchID) == 0);

                        if (userDb != null) {
                            userDb.Currency.Value += user.Value;
                        }
                    }

                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }

            public bool Add(string twitchID, int value) {
                using (var context = new Storage.StorageEntities()) {
                    var user = context.Users.SingleOrDefault(u => String.Compare(u.Id, twitchID) == 0);

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
                using (var context = new Storage.StorageEntities()) {
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
            public bool Remove(string twitchID, int value) {
                using (var context = new Storage.StorageEntities()) {
                    var user = context.Users.SingleOrDefault(u => String.Compare(u.Id, twitchID) == 0);

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
            public async void Remove(List<Models.DatabaseCurrencyModel.ListCurrencyUpdate> UserList) {
                using (var context = new Storage.StorageEntities()) {
                    foreach (var user in UserList) {
                        var userDb = context.Users.SingleOrDefault(u => String.Compare(u.Id, user.TwitchID) == 0);

                        if (userDb != null) {
                            userDb.Currency.Value -= user.Value;
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
            internal bool Transfer(string userid1, string userid2, int value) {
                using (var context = new Storage.StorageEntities()) {
                    var user1 = context.Users.SingleOrDefault(u => String.Compare(u.Id, userid1) == 0);
                    var user2 = context.Users.SingleOrDefault(u => String.Compare(u.Id, userid2) == 0);

                    if (userid1 != null && userid2 != null) {
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
        public long? GetCurrency(string twitchID) {
            using (var context = new Core.Storage.StorageEntities()) {
                var user = context.Users.SingleOrDefault(u => String.Compare(u.Id, twitchID) == 0);

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
        public bool HasUserEnoughCurrency(string twitchID, int currencyToCheck) {
            using (var context = new Storage.StorageEntities()) {
                var user = context.Users.SingleOrDefault(u => String.Compare(twitchID, u.Id, true) == 0);

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
        public async void AddUserToCurrencyTable(string twitchID) {
            using (var context = new Storage.StorageEntities()) {
                var currencyEntry = context.Currency.SingleOrDefault(c => String.Compare(c.ID, twitchID) == 0);

                if (currencyEntry == null) {
                    context.Currency.Add(
                        new Storage.Currency {
                            ID = twitchID,
                            Value = 0
                        });

                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }

        #endregion Functions
    }
}