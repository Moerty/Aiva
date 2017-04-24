using System;
using System.Collections.Generic;
using System.Linq;
using Aiva.Core.Config;
using Aiva.Core.Storage;

namespace Aiva.Core.Database {
    public class CurrencyHandler {

        /// <summary>
        /// Add currency frequently
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static async void AddCurrencyFrequentlyAsync(object sender, EventArgs e) {
            using (var context = new StorageEntities()) {
                var updatedUsers = context.Users.Where(u => u.IsViewing == 1).ToList();

                foreach (var user in updatedUsers) {
                    var currencyItem = context.Currency.SingleOrDefault(c => c.Name == user.Name);

                    var currencyToAdd = Convert.ToInt64(GeneralConfig.Config["Currency"]["CurrencyToAdd"]);
                    currencyItem.Value += currencyToAdd;
                }

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Get Currency from User
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Int64 GetCurrency(string name) {
            Int64 currency;
            using (var context = new StorageEntities()) {
                var user = context.Currency.SingleOrDefault(u => String.Compare(u.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
                currency = user.Value;
            }

            return currency;
        }

        /// <summary>
        /// Update Currency from List
        /// </summary>
        /// <param name="list"></param>
        public static async void UpdateCurrencyListAsync(List<Models.Database.CurrencyHandlerModels.CurrencyAddList> list) {
            using (var context = new StorageEntities()) {
                foreach (var user in list) {
                    var entry = context.Currency.SingleOrDefault(c => c.Name == user.Name);
                    entry.Value += user.Value;
                }

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Remove Currency from User
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static async void RemoveCurrencyAsync(string name, int value) {
            using (var context = new StorageEntities()) {
                var entry = context.Currency.SingleOrDefault(c => c.Name == name);

                if (entry != null) {
                    if (entry.Value >= value)
                        entry.Value -= value;
                }

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add Currency to User
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static async void AddCurrencyAsync(string name, int value) {
            using (var context = new StorageEntities()) {
                var entry = context.Currency.SingleOrDefault(c => c.Name == name);

                if (entry != null) {
                    entry.Value += value;
                }

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Transfer Currency
        /// </summary>
        /// <param name="giver"></param>
        /// <param name="taker"></param>
        /// <param name="value"></param>
        public static async void TransferCurrencyAsync(string giver, string taker, int value) {
            using (var context = new StorageEntities()) {
                var giverEntry = context.Currency.SingleOrDefault(g => g.Name == giver);
                var takerEntry = context.Currency.SingleOrDefault(t => t.Name == taker);

                if (giverEntry != null && takerEntry != null) {
                    if (giverEntry.Value >= value) {
                        giverEntry.Value -= value;
                        takerEntry.Value += value;
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Get Currency List
        /// </summary>
        /// <returns></returns>
        public static List<Storage.Currency> GetCurrencyList() {
            using (var context = new StorageEntities()) {
                return context.Currency.ToList();
            }
        }
    }
}
