using Aiva.Core.Config;
using Aiva.Core.Twitch;
using Aiva.Models.Enums;
using Aiva.Models.Streamgames.Bankheist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Events;

namespace Aiva.Extensions.Streamgames.Bankheist
{
    public class Handler
    {
        private DateTime _nextStartAfterCooldown;
        private List<Models.Streamgames.Bankheist.UserBetModel> _userList;
        private readonly Core.Database.Handlers.Currency _databaseCurrencyHandler;
        private System.Timers.Timer _bankheistEndTimer;

        public Handler()
        {
            _databaseCurrencyHandler = new Core.Database.Handlers.Currency();
            if (Core.Config.Config.Instance.Storage.StreamGames.Bankheist.General.Active)
            {
                AivaClient.Instance.TwitchClient.OnChatCommandReceived += ChatCommandReceived;
            }
        }

        private void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            if (string.Compare(e.Command.CommandText, Config.Instance.Storage.StreamGames.Bankheist.General.Command) == 0)
            {
                // continue only if CooldownDatetime is null (never running) or cooldown is lower than Datetime.now
                if (_nextStartAfterCooldown == default(DateTime) || _nextStartAfterCooldown.TimeOfDay <= DateTime.Now.TimeOfDay)
                {
                    // check if chat argument is an integer
                    if (int.TryParse(e.Command.ArgumentsAsString, out int bankheistBet))
                    {
                        var userId = Convert.ToInt32(e.Command.ChatMessage.UserId);
                        // check if user has enough currency
                        if (_databaseCurrencyHandler.HasUserEnoughCurrency(userId, bankheistBet))
                        {
                            // first startup? then create userlist and writes in chat that Bankheist is started
                            if (_userList == null)
                            {
                                _userList = new List<Models.Streamgames.Bankheist.UserBetModel>();
                                WriteBankheistStartupInChat();
                                AddUserToList(userId, bankheistBet, e.Command.ChatMessage.DisplayName);
                                StartBankheistEndTimer();
                            }
                            else
                            {
                                // check if user is already in list
                                if (_userList.SingleOrDefault(u => u.TwitchID == userId) == null)
                                {
                                    AddUserToList(userId, bankheistBet, e.Command.ChatMessage.DisplayName);
                                }
                            }

                            _databaseCurrencyHandler.Remove.Remove(userId, bankheistBet);
                        }
                    }
                }
                else
                {
                    AivaClient.Instance.TwitchClient.SendMessage(
                        AivaClient.Instance.Channel,
                        "Bankheist is on cooldown!",
                        AivaClient.DryRun);
                }
            }
        }

        private void StartBankheistEndTimer()
        {
            _bankheistEndTimer = new System.Timers.Timer
            {
                AutoReset = false,
                Interval = TimeSpan.FromMinutes(Config.Instance.Storage.StreamGames.Bankheist.Cooldowns.BankheistDuration).TotalMilliseconds
            };
            _bankheistEndTimer.Elapsed += (sender, ElapsedEventArgs) => StopBankheist();
            _bankheistEndTimer.Start();
        }

        private void StopBankheist()
        {
            _nextStartAfterCooldown = DateTime.Now.AddMinutes(Config.Instance.Storage.StreamGames.Bankheist.Cooldowns.BankheistCooldown);
            var bank = IdentifyBank();
            var winningDetailsForBank = GetWinningDetailsForBank(bank);

            var Winners = new List<Models.Streamgames.Bankheist.UserBetModel>();

            foreach (var user in _userList)
            {
                if (IsUserWinner(winningDetailsForBank.Item1))
                {
                    user.Bet = (int)(user.Bet * winningDetailsForBank.Item2);
                    Winners.Add(user);
                }
            }

            if (Winners.Count > 0)
            {
                Winners.ForEach(u => _databaseCurrencyHandler.Add.Add(u.TwitchID, u.Bet));
            }

            WriteWinnersInChat(Winners);
        }

        private void WriteWinnersInChat(List<UserBetModel> winners)
        {
            var sb = new StringBuilder();
            sb.Append("Winners: ");
            winners.ForEach(w => sb.Append(w.Name).Append("|"));
            AivaClient.Instance.TwitchClient.SendMessage(
                channel: AivaClient.Instance.Channel,
                message: sb.ToString().TrimEnd('|'),
                dryRun: AivaClient.DryRun);
        }

        private bool IsUserWinner(long item1)
        {
            return new Random().Next(100) <= item1;
        }

        private Tuple<long, double> GetWinningDetailsForBank(Bank bank)
        {
            switch (bank)
            {
                case Bank.Bank5:
                    return new Tuple<long, double>(
                        Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank5.SuccessRate,
                        Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank5.WinningMultiplier);
                case Bank.Bank4:
                    return new Tuple<long, double>(
                        Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank4.SuccessRate,
                        Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank4.WinningMultiplier);
                case Bank.Bank3:
                    return new Tuple<long, double>(
                        Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank3.SuccessRate,
                        Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank3.WinningMultiplier);
                case Bank.Bank2:
                    return new Tuple<long, double>(
                        Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank2.SuccessRate,
                        Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank2.WinningMultiplier);
                default: // bank 1
                    return new Tuple<long, double>(
                        Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank1.SuccessRate,
                        Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank1.WinningMultiplier);
            }
        }

        private Models.Enums.Bank IdentifyBank()
        {
            var count = _userList.Count;

            if (count >= Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank5.MinimumPlayers)
            {
                return Models.Enums.Bank.Bank5;
            }
            else if (count >= Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank4.MinimumPlayers)
            {
                return Models.Enums.Bank.Bank4;
            }
            else if (count >= Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank3.MinimumPlayers)
            {
                return Models.Enums.Bank.Bank3;
            }
            else if (count >= Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank2.MinimumPlayers)
            {
                return Models.Enums.Bank.Bank2;
            }
            else
            {
                return Models.Enums.Bank.Bank1;
            }
        }

        private void AddUserToList(int userId, int bankheistBet, string displayName)
        {
            _userList.Add(
                new Models.Streamgames.Bankheist.UserBetModel
                {
                    Name = displayName,
                    TwitchID = userId,
                    Bet = bankheistBet
                });
        }

        private void WriteBankheistStartupInChat()
        {
            AivaClient.Instance.TwitchClient.SendMessage(
                channel: AivaClient.Instance.Channel,
                message: $"New Bankheist started! Write !{Config.Instance.Storage.StreamGames.Bankheist.General.Command} in the Chat to rob the bank!",
                dryRun: AivaClient.DryRun);
        }
    }
}