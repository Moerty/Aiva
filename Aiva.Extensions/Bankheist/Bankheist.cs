using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Events.Client;
using Aiva.Core;
using Aiva.Core.Config;

namespace Aiva.Extensions.Bankheist {
    public class Bankheist {
        Models.Bankheist.BankheistInitModel InitModel;

        public List<Models.Bankheist.BankheistUserModel> UserList;
        public bool IsStarted { get; private set; }

        public Bankheist(Models.Bankheist.BankheistInitModel InitModel) {
            this.InitModel = InitModel;
            this.UserList = new List<Models.Bankheist.BankheistUserModel>();

            IsStarted = true;
        }

        /// <summary>
        /// Add user to list
        /// </summary>
        /// <param name="name">todo: describe name parameter on AddUserToBankheist</param>
        /// <param name="argument">todo: describe argument parameter on AddUserToBankheist</param>
        /// <param name="userID">todo: describe userID parameter on AddUserToBankheist</param>
        public void AddUserToBankheist(string name, string userID, string argument) {

            // Is Argument a valid Int
            if (int.TryParse(argument, out int bet)) {

                // Does the user have enough Currency?
                bool HasEnoughCurrency = UserHasEnoughCurrency(bet, userID);

                if (HasEnoughCurrency) {
                    // User has enough Currency
                    UserList.Add(new Models.Bankheist.BankheistUserModel {
                        Name = name,
                        TwitchID = userID,
                        Bet = bet
                    });
                }
            }
        }

        /// <summary>
        /// Check if the User has enough Currency
        /// </summary>
        /// <param name="bet"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        private bool UserHasEnoughCurrency(long bet, string userID) {
            using (var context = new Core.Storage.StorageEntities()) {

                var currency = context.Currency.SingleOrDefault(c => String.Compare(c.ID, userID) == 0);

                if (currency != null) {
                    if (currency.Value >= bet) {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Payout
        /// </summary>
        internal void PayOut() {
            int SuccessRate;
            double WinningMultiplicator;
            GetBankheistPayoutDetails(out SuccessRate, out WinningMultiplicator);

            var Winners = new List<Core.Models.DatabaseCurrencyModel.ListCurrencyUpdate>();
            var Loosers = new List<Core.Models.DatabaseCurrencyModel.ListCurrencyUpdate>();
            foreach (var user in UserList) {
                bool IsWinner = IsUserWinner(SuccessRate);

                if (IsWinner) {
                    Winners.Add(
                        new Core.Models.DatabaseCurrencyModel.ListCurrencyUpdate {
                            TwitchID = user.TwitchID,
                            Name = user.Name,
                            Value = (int)(user.Bet * WinningMultiplicator),
                        });
                } else {
                    Loosers.Add(new Core.Models.DatabaseCurrencyModel.ListCurrencyUpdate {
                        TwitchID = user.TwitchID,
                        Name = user.Name,
                        Value = -(int)(user.Bet * WinningMultiplicator)
                    });
                }
            }

            if (Winners.Any())
                Core.Database.Currency.Add.AddCurrencyToUserList(Winners);

            Core.Database.Currency.Remove.RemoveCurrencyToUserList(Loosers);
        }

        /// <summary>
        /// Write Winners in Chat
        /// </summary>
        /// <param name="Winners"></param>
        private void WriteWinnersInChat(List<Core.Models.DatabaseCurrencyModel.ListCurrencyUpdate> Winners) {
            StringBuilder sb = new StringBuilder();

            if (Winners.Count == 1)
                sb.Append("The Winner is: ");
            else
                sb.Append("The Winners are: ");


            foreach (var winner in Winners) {
                sb.Append($"{winner.Name} - {winner.Value} | ");
            }

            AivaClient.Instance.AivaTwitchClient.SendMessage(sb.ToString());
        }

        /// <summary>
        /// Get Bankheist payout details
        /// </summary>
        /// <param name="SuccessRate"></param>
        /// <param name="WinningMultiplicator"></param>
        private void GetBankheistPayoutDetails(out int SuccessRate, out double WinningMultiplicator) {
            var bank = IdentifyBank();
            switch (bank) {
                case Models.Bankheist.Bank.Bank2:
                    SuccessRate = Convert.ToInt32(Config.Instance["Bankheist"]["Bank2SuccessRate"]);
                    WinningMultiplicator = Convert.ToDouble(Config.Instance["Bankheist"]["Bank2WinningMultiplier"]);
                    break;
                case Models.Bankheist.Bank.Bank3:
                    SuccessRate = Convert.ToInt32(Config.Instance["Bankheist"]["Bank3SuccessRate"]);
                    WinningMultiplicator = Convert.ToDouble(Config.Instance["Bankheist"]["Bank3WinningMultiplier"]);
                    break;
                case Models.Bankheist.Bank.Bank4:
                    SuccessRate = Convert.ToInt32(Config.Instance["Bankheist"]["Bank4SuccessRate"]);
                    WinningMultiplicator = Convert.ToDouble(Config.Instance["Bankheist"]["Bank4WinningMultiplier"]);
                    break;
                case Models.Bankheist.Bank.Bank5:
                    SuccessRate = Convert.ToInt32(Config.Instance["Bankheist"]["Bank5SuccessRate"]);
                    WinningMultiplicator = Convert.ToDouble(Config.Instance["Bankheist"]["Bank5WinningMultiplier"]);
                    break;
                default: // Bank1
                    SuccessRate = Convert.ToInt32(Config.Instance["Bankheist"]["Bank1SuccessRate"]);
                    WinningMultiplicator = Convert.ToDouble(Config.Instance["Bankheist"]["Bank1WinningMultiplier"]);
                    break;
            }
        }

        /// <summary>
        /// Identify Bank
        /// </summary>
        /// <returns></returns>
        private Models.Bankheist.Bank IdentifyBank() {
            var MemberCount = UserList.Count;

            if (MemberCount >= Convert.ToInt32(Config.Instance["Bankheist"]["Bank5MinimumPlayers"])) {
                return Models.Bankheist.Bank.Bank5;
            } else if (MemberCount >= Convert.ToInt32(Config.Instance["Bankheist"]["Bank4MinimumPlayers"])) {
                return Models.Bankheist.Bank.Bank4;
            } else if (MemberCount >= Convert.ToInt32(Config.Instance["Bankheist"]["Bank3MinimumPlayers"])) {
                return Models.Bankheist.Bank.Bank3;
            } else if (MemberCount >= Convert.ToInt32(Config.Instance["Bankheist"]["Bank2MinimumPlayers"])) {
                return Models.Bankheist.Bank.Bank2;
            } else {
                return Models.Bankheist.Bank.Bank1;
            }
        }

        /// <summary>
        /// Calculate if the User is a Winner
        /// </summary>
        /// <param name="successRate"></param>
        /// <returns></returns>
        private bool IsUserWinner(int successRate) {
            var generator = new Random();

            if (generator.Next(100) <= successRate) {
                return true;
            } else {
                return false;
            }
        }
    }
}
