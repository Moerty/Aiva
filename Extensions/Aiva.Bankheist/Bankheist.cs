using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib;
using AivaBot;
using static AivaBot.Bankheist.Models.BankheistModel;
using AivaBot.Bankheist.Models;

namespace AivaBot.Bankheist {
    public class Bankheist {
        // Must static
        public static TimeSpan timeToNewBackheist;
        public static Enums.BankheistStatus Status { get; set; } = Enums.BankheistStatus.Ready;

        // non static
        public bool determineWinners;
        public int successRate;
        private readonly IniData bankheistConfig;
        private readonly IniData languageConfig;
        public System.Timers.Timer bankheistActiveTimer;
        public string bankType;

        public ObservableCollection<BankheistModel> bankheistUsers;

        /// <summary>
        /// Initialize new Bankheist
        /// </summary>
        /// <param name="command"></param>
        public Bankheist(IniData bankheistConfig, BankheistModel model) {
            this.bankheistConfig = bankheistConfig;
            bankheistUsers = new ObservableCollection<BankheistModel>();

            // Add initial User
            bankheistUsers.Add(new BankheistModel(model.name, model.value));

            // Starting Bankheist
            Status = Enums.BankheistStatus.IsActive;
            this.bankheistActiveTimer = new System.Timers.Timer();
            this.bankheistActiveTimer.Interval = TimeSpan.Parse(bankheistConfig["General"]["BankheistTime"]).TotalMilliseconds;
            bankheistActiveTimer.Start();
        }

        /// <summary>
        /// Add User to existing Bankheist Event
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="value">Amount</param>
        public bool addUserToList(string name, int value) {
            if (Status == Enums.BankheistStatus.IsActive) {
                var bankheist = new BankheistModel(name, value);

                var existUser = bankheistUsers.SingleOrDefault(b => b.name == name);


                if (existUser == null) {
                    bankheistUsers.Add(bankheist);
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }


        public System.Timers.Timer getTimerForNewBankheist() {
            // Timer 4 new Bankheist
            var NewBankheist = new System.Timers.Timer {
                Interval = TimeSpan.Parse(bankheistConfig["General"]["TimeToNewBankheist"]).TotalMilliseconds
            };


            timeToNewBackheist = new TimeSpan((DateTime.Now.Ticks + TimeSpan.Parse(bankheistConfig["General"]["TimeToNewBankheist"]).Ticks) - DateTime.Now.Ticks);

            return NewBankheist;
        }



        public BankheistModel ModifyUserIfWinner(BankheistModel user) {
            var generator = new Random();

            if (generator.Next(100) <= successRate) {
                user.value = (int)(user.value * Convert.ToDouble(bankheistConfig[bankType]["WinningMultiplier"]));

                return user;
            }
            else {
                user.value = user.value - user.value - user.value;

                return user;
            }
        }

        /// <summary>
        /// Get Bank
        /// </summary>
        /// <returns></returns>
        public string WhichBank() {
            if (bankheistUsers.Count >= Convert.ToInt32(bankheistConfig["Bank5"]["MinimumPlayers"])) return "Bank5";

            if (bankheistUsers.Count >= Convert.ToInt32(bankheistConfig["Bank4"]["MinimumPlayers"])) return "Bank4";

            if (bankheistUsers.Count >= Convert.ToInt32(bankheistConfig["Bank3"]["MinimumPlayers"])) return "Bank3";

            if (bankheistUsers.Count >= Convert.ToInt32(bankheistConfig["Bank2"]["MinimumPlayers"])) return "Bank2";

            if (bankheistUsers.Count >= Convert.ToInt32(bankheistConfig["Bank1"]["MinimumPlayers"])) return "Bank1";

            return null;
        }
    }
}
