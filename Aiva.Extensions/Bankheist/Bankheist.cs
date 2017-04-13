using System;
using System.Collections.ObjectModel;
using System.Linq;
using Aiva.Extensions.Models.Bankheist;

namespace Aiva.Extensions.Bankheist {
    public class Bankheist {
        // Must static
        public static TimeSpan timeToNewBackheist;
        public static BankheistModel.Enums.BankheistStatus Status { get; set; } = BankheistModel.Enums.BankheistStatus.Ready;

        // non static
        public bool determineWinners;
        public int successRate;
        public System.Timers.Timer bankheistActiveTimer;
        public string bankType;

        public ObservableCollection<BankheistModel> bankheistUsers;

        /// <summary>
        /// Initialize new Bankheist
        /// </summary>
        /// <param name="command"></param>
        public Bankheist(BankheistModel model) {
            bankheistUsers = new ObservableCollection<BankheistModel> {
                // Add initial User
                new BankheistModel(model.name, model.value)
            };

            // Starting Bankheist
            Status = BankheistModel.Enums.BankheistStatus.IsActive;
            this.bankheistActiveTimer = new System.Timers.Timer {
                Interval = TimeSpan.Parse(Core.Config.BankheistConfig.Config["General"]["BankheistTime"]).TotalMilliseconds
            };
            bankheistActiveTimer.Start();
        }

        /// <summary>
        /// Add User to existing Bankheist Event
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="value">Amount</param>
        public bool AddUserToList(string name, int value) {
            if (Status == BankheistModel.Enums.BankheistStatus.IsActive) {
                var bankheist = new BankheistModel(name, value);

                var existUser = bankheistUsers.SingleOrDefault(b => b.name == name);


                if (existUser == null) {
                    bankheistUsers.Add(bankheist);
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }


        public System.Timers.Timer GetTimerForNewBankheist() {
            // Timer 4 new Bankheist
            var NewBankheist = new System.Timers.Timer {
                Interval = TimeSpan.Parse(Core.Config.BankheistConfig.Config["General"]["TimeToNewBankheist"]).TotalMilliseconds
            };


            timeToNewBackheist = new TimeSpan((DateTime.Now.Ticks + TimeSpan.Parse(Core.Config.BankheistConfig.Config["General"]["TimeToNewBankheist"]).Ticks) - DateTime.Now.Ticks);

            return NewBankheist;
        }



        public BankheistModel ModifyUserIfWinner(BankheistModel user) {
            var generator = new Random();

            if (generator.Next(100) <= successRate) {
                user.value = (int)(user.value * Convert.ToDouble(Core.Config.BankheistConfig.Config[bankType]["WinningMultiplier"]));

                return user;
            } else {
                user.value = user.value - user.value - user.value;

                return user;
            }
        }

        /// <summary>
        /// Get Bank
        /// </summary>
        /// <returns></returns>
        public string WhichBank() {
            if (bankheistUsers.Count >= Convert.ToInt32(Core.Config.BankheistConfig.Config["Bank5"]["MinimumPlayers"])) return "Bank5";

            if (bankheistUsers.Count >= Convert.ToInt32(Core.Config.BankheistConfig.Config["Bank4"]["MinimumPlayers"])) return "Bank4";

            if (bankheistUsers.Count >= Convert.ToInt32(Core.Config.BankheistConfig.Config["Bank3"]["MinimumPlayers"])) return "Bank3";

            if (bankheistUsers.Count >= Convert.ToInt32(Core.Config.BankheistConfig.Config["Bank2"]["MinimumPlayers"])) return "Bank2";

            if (bankheistUsers.Count >= Convert.ToInt32(Core.Config.BankheistConfig.Config["Bank1"]["MinimumPlayers"])) return "Bank1";

            return null;
        }
    }
}
