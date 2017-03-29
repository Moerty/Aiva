using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackBoxBot.Bankheist.Models;
using static BlackBoxBot.Bankheist.Models.BankheistModel.Enums;

namespace BlackBoxBot.Bankheist
{
    class BankheistHandler
    {
        private static Bankheist bankheist;
        private static System.Timers.Timer NewBankheist;

        /// <summary>
        /// EntryPoint 4 New Bankheist
        /// </summary>
        /// <param name="e">Chat Command Receive Args</param>
        public static void ProcessBankheist(TwitchLib.Events.Client.OnChatCommandReceivedArgs e)
        {
            // Bankheist locked?
            if (NewBankheist == null)
            {
                // Is Argument a valid Int?
                int result;
                if (int.TryParse(e.Command.ArgumentsAsString, out result))
                {
                    // Does have the User enough Currency?
                    if (CheckifEnoughCurrency(e.Command.ChatMessage.Username, result))
                    {
                        if (bankheist == null)
                        {
                            bankheist = new Bankheist(
                                Config.Bankheist.Config,
                                new BankheistModel(e.Command.ChatMessage.Username, result));
                        }
                        // Or add user to list
                        else
                        {
                            var adduser = bankheist.addUserToList(e.Command.ChatMessage.Username, result);

                            // Bankheist closed while check winners
                            if (bankheist.determineWinners)
                            {
                                Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("BankheistAddUserBankheistRunning"));
                                return;
                            }

                            // User already exists?
                            if (!adduser)
                            {
                                Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("BankheistAddUserExists"));
                                return;
                            }
                        }
                    }
                    // return if not enough currency
                    else
                    {
                        Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("BankheistNotEnoughCurrency")
                                                                                                       .Replace("@USERNAME@", e.Command.ChatMessage.Username)
                                                                                                       .Replace("@VALUE@", result.ToString())
                                                                                                       .Replace("@CURRENCY@", Config.Language.Instance.GetString("CurrencyName")));

                        return;
                    }
                }
                // Something went wrong with the argument
                else
                {
                    Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("BankheistStartErrorText"));
                }
            }
            // When Bankheist is on cooldown
            else
            {
                Client.Client.ClientBBB.TwitchClientBBB.SendMessage(
                    Config.Language.Instance.GetString("BankheistCooldownText").Replace("@TIMELEFT@", Bankheist.timeToNewBackheist.TotalMinutes.ToString()));

                return;
            }

            bankheist.bankheistActiveTimer.Elapsed += _bankheistActiveTimer_Elapsed;
            bankheist.bankheistUsers.CollectionChanged += BankheistUsers_CollectionChanged;


            Client.Client.ClientBBB.TwitchClientBBB.SendMessage(
                Config.Language.Instance.GetString("BankheistStartText")
                    .Replace("@USERNAME@", e.Command.ChatMessage.Username)
                    .Replace("@COMMAND@", Config.Bankheist.Config["General"]["Command"])
                );
        }

        /// <summary>
        /// Bankheist end Timer Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _bankheistActiveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Stopping Bankheist & initial setup for ending
            Bankheist.Status = BankheistStatus.OnCooldown;
            bankheist.bankheistActiveTimer.Stop();
            bankheist.determineWinners = true;
            bankheist.bankType = bankheist.WhichBank();
            bankheist.successRate = Convert.ToInt32(Config.Bankheist.Config[bankheist.bankType]["SuccessRate"]);

            // Logic
            var Winners = new List<BankheistModel>();
            foreach (var user in bankheist.bankheistUsers.ToList())
            {
                var result = bankheist.ModifyUserIfWinner(user);


                // Win
                if (result.value > 0)
                {
                    Client.Client.ClientBBB.TwitchClientBBB.SendMessage(
                        Config.Language.Instance.GetString("BankheistWinning")
                                            .Replace("@USERNAME@", user.name)
                                            .Replace("@CURRENCY@", Config.Language.Instance.GetString("CurrencyName"))
                                            .Replace("@VALUE@", user.value.ToString()));

                    Winners.Add(result);
                }
                // Loose
                else
                {
                    Database.CurrencyHandler.RemoveCurrencyAsync(user.name, user.value);

                    Client.Client.ClientBBB.TwitchClientBBB.SendMessage(
                        Config.Language.Instance.GetString("BankheistLoosing")
                                            .Replace("@USERNAME@", user.name)
                                            .Replace("@CURRENCY@", Config.Language.Instance.GetString("CurrencyName"))
                                            .Replace("@VALUE@", user.value.ToString()));

                    bankheist.bankheistUsers.Remove(user);
                }
            }

            // Update
            List<Database.Models.CurrencyHandlerModels.CurrencyAddList> List = new List<Database.Models.CurrencyHandlerModels.CurrencyAddList>();
            Winners.ForEach(x =>
            {
                List.Add(
                    new Database.Models.CurrencyHandlerModels.CurrencyAddList
                    {
                        Name = x.name,
                        Value = x.value
                    });
            });
            Database.CurrencyHandler.UpdateCurrencyListAsync(List);

            NewBankheist = bankheist.getTimerForNewBankheist();
            NewBankheist.Elapsed += NewBankheist_Elapsed;
            NewBankheist.Start();

            bankheist.determineWinners = false;
        }

        /// <summary>
        /// End locked Timer, can start new Bankheist, Info 4 free Bankheist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void NewBankheist_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            NewBankheist.Stop();
            NewBankheist = null;
            Bankheist.timeToNewBackheist = new TimeSpan();


            Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("BankheistCooldownOverText"));
            bankheist = null;
            Bankheist.Status = BankheistStatus.Ready;
        }


        private static bool CheckifEnoughCurrency(string name, int value)
        {

            
            var currency = Database.CurrencyHandler.GetCurrency(name);

            return (currency >= value);
        }

        /// <summary>
        /// Info 4 new Bank if many users will play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void BankheistUsers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewStartingIndex != -1)
            {
                switch (bankheist.bankheistUsers.Count)
                {
                    case 10:
                        {
                            Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("BankheistBank2Text"));
                            break;
                        }
                    case 20:
                        {
                            Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("BankheistBank3Text"));
                            break;
                        }
                    case 30:
                        {
                            Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("BankheistBank4Text"));
                            break;
                        }
                    case 40:
                        {
                            Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("BankheistBank5Text"));
                            break;
                        }
                    default:
                        {
                            Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("BankheistAddUserText"));
                            break;
                        }
                }
            }
        }
    }
}
