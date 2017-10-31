using System;
using System.Collections.Generic;
using System.Linq;

namespace Aiva.Core.DatabaseHandlers {
    public class Currency {

        public AddCurrency Add;
        public RemoveCurrency Remove;
        public TransferCurrency Transfer;

        public Currency() {
            Add = new AddCurrency();
            Remove = new RemoveCurrency();
            Transfer = new TransferCurrency();
        }

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

                    await context.SaveChangesAsync();
                }
            }

            public void Add(string twitchID, int value) {
                using (var context = new Storage.StorageEntities()) {
                    var user = context.Users.SingleOrDefault(u => String.Compare(u.Id, twitchID) == 0);

                    if (user != null) {
                        user.Currency.Value += value;

                        context.SaveChanges();
                    }
                }
            }

            /// <summary>
            /// Add currency to active viewers
            /// </summary>
            public void AddCurrencyActiveViewer() {
                using (var context = new Storage.StorageEntities()) {
                    var activeUsers = context.ActiveUsers.ToList();

                    foreach (var user in activeUsers) {
                        user.Users.Currency.Value += Convert.ToInt64(Config.Config.Instance[nameof(Currency)]["CurrencyToAddFrequently"]);
                    }

                    context.SaveChanges();
                }
            }
        }

        public class RemoveCurrency {

            /// <summary>
            /// Remove Currency from User
            /// </summary>
            /// <param name="twitchID"></param>
            /// <param name="value"></param>
            public void Remove(string twitchID, int value) {
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
            public async void Remove(List<Models.DatabaseCurrencyModel.ListCurrencyUpdate> UserList) {
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

        public class TransferCurrency {

        }

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

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
