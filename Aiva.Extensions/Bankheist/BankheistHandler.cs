using System;
using System.Collections.Generic;
using System.Linq;
using Aiva.Extensions.Models.Bankheist;
using Aiva.Core.Client;
using Aiva.Core.Config;
using Aiva.Core.Database;

namespace Aiva.Extensions.Bankheist {
    class BankheistHandler {
        private static Bankheist bankheist;
        private static System.Timers.Timer NewBankheist;

        /// <summary>
        /// EntryPoint 4 New Bankheist
        /// </summary>
        /// <param name="e">Chat Command Receive Args</param>
        public static void ProcessBankheist(TwitchLib.Events.Client.OnChatCommandReceivedArgs e) {
            // Bankheist locked?
            if (NewBankheist == null) {
                // Is Argument a valid Int?
                if (int.TryParse(e.Command.ArgumentsAsString, out int result)) {
                    // Does have the User enough Currency?
                    if (CheckifEnoughCurrency(e.Command.ChatMessage.Username, result)) {
                        if (bankheist == null) {
                            bankheist = new Bankheist(
                                new BankheistModel(e.Command.ChatMessage.Username, result));
                        }
                        // Or add user to list
                        else {
                            var adduser = bankheist.AddUserToList(e.Command.ChatMessage.Username, result);

                            // Bankheist closed while check winners
                            if (bankheist.determineWinners) {

                                AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("BankheistAddUserBankheistRunning"));
                                return;
                            }

                            // User already exists?
                            if (!adduser) {
                                AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("BankheistAddUserExists"));
                                return;
                            }
                        }
                    }
                    // return if not enough currency
                    else {
                        AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("BankheistNotEnoughCurrency")
                                                                                                       .Replace("@USERNAME@", e.Command.ChatMessage.Username)
                                                                                                       .Replace("@VALUE@", result.ToString())
                                                                                                       .Replace("@CURRENCY@", LanguageConfig.Instance.GetString("CurrencyName")));

                        return;
                    }
                }
// Something went wrong with the argument
else {
                    AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("BankheistStartErrorText"));
                }
            }
            // When Bankheist is on cooldown
            else {
                AivaClient.Client.AivaTwitchClient.SendMessage(
                    LanguageConfig.Instance.GetString("BankheistCooldownText").Replace("@TIMELEFT@", Bankheist.timeToNewBackheist.TotalMinutes.ToString()));

                return;
            }

            bankheist.bankheistActiveTimer.Elapsed += _bankheistActiveTimer_Elapsed;
            bankheist.bankheistUsers.CollectionChanged += BankheistUsers_CollectionChanged;


            AivaClient.Client.AivaTwitchClient.SendMessage(
                LanguageConfig.Instance.GetString("BankheistStartText")
                    .Replace("@USERNAME@", e.Command.ChatMessage.Username)
                    .Replace("@COMMAND@", Core.Config.BankheistConfig.Config["General"]["Command"])
                );
        }

        /// <summary>
        /// Bankheist end Timer Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _bankheistActiveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            // Stopping Bankheist & initial setup for ending
            Bankheist.Status = BankheistModel.Enums.BankheistStatus.OnCooldown;
            bankheist.bankheistActiveTimer.Stop();
            bankheist.determineWinners = true;
            bankheist.bankType = bankheist.WhichBank();
            bankheist.successRate = Convert.ToInt32(Core.Config.BankheistConfig.Config[bankheist.bankType]["SuccessRate"]);

            // Logic
            var Winners = new List<BankheistModel>();
            foreach (var user in bankheist.bankheistUsers.ToList()) {
                var result = bankheist.ModifyUserIfWinner(user);


                // Win
                if (result.value > 0) {
                    AivaClient.Client.AivaTwitchClient.SendMessage(
                        LanguageConfig.Instance.GetString("BankheistWinning")
                                            .Replace("@USERNAME@", user.name)
                                            .Replace("@CURRENCY@", LanguageConfig.Instance.GetString("CurrencyName"))
                                            .Replace("@VALUE@", user.value.ToString()));

                    Winners.Add(result);
                }
                // Loose
                else {
                    CurrencyHandler.RemoveCurrencyAsync(user.name, user.value);

                    AivaClient.Client.AivaTwitchClient.SendMessage(
                        LanguageConfig.Instance.GetString("BankheistLoosing")
                                            .Replace("@USERNAME@", user.name)
                                            .Replace("@CURRENCY@", LanguageConfig.Instance.GetString("CurrencyName"))
                                            .Replace("@VALUE@", user.value.ToString()));

                    bankheist.bankheistUsers.Remove(user);
                }
            }

            // Update
            var List = new List<Core.Models.Database.CurrencyHandlerModels.CurrencyAddList>();
            Winners.ForEach(x => {
                List.Add(
                    new Core.Models.Database.CurrencyHandlerModels.CurrencyAddList {
                        Name = x.name,
                        Value = x.value
                    });
            });
            CurrencyHandler.UpdateCurrencyListAsync(List);

            NewBankheist = bankheist.GetTimerForNewBankheist();
            NewBankheist.Elapsed += NewBankheist_Elapsed;
            NewBankheist.Start();

            bankheist.determineWinners = false;
        }

        /// <summary>
        /// End locked Timer, can start new Bankheist, Info 4 free Bankheist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void NewBankheist_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            NewBankheist.Stop();
            NewBankheist = null;
            Bankheist.timeToNewBackheist = new TimeSpan();


            AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("BankheistCooldownOverText"));
            bankheist = null;
            Bankheist.Status = BankheistModel.Enums.BankheistStatus.Ready;
        }


        private static bool CheckifEnoughCurrency(string name, int value) {


            var currency = CurrencyHandler.GetCurrency(name);

            return (currency >= value);
        }

        /// <summary>
        /// Info 4 new Bank if many users will play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void BankheistUsers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            if (e.NewStartingIndex != -1) {
                switch (bankheist.bankheistUsers.Count) {
                    case 10: {
                            AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("BankheistBank2Text"));
                            break;
                        }
                    case 20: {
                            AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("BankheistBank3Text"));
                            break;
                        }
                    case 30: {
                            AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("BankheistBank4Text"));
                            break;
                        }
                    case 40: {
                            AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("BankheistBank5Text"));
                            break;
                        }
                    default: {
                            AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("BankheistAddUserText"));
                            break;
                        }
                }
            }
        }
    }
}