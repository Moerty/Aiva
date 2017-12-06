using Aiva.Core.Twitch;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Events.Client;

namespace Aiva.Extensions.Giveaway {
    public class Handler {
        public event EventHandler<Models.Giveaway.Users> OnWinnerFound;
        public event EventHandler<Models.Giveaway.Users> OnJoinedUser;
        public event EventHandler<Models.Giveaway.Messages> OnWinnerMessageReceived;
        public event EventHandler OnTimerEnds;

        public readonly Models.Giveaway.Properties Properties;

        private readonly List<Models.Giveaway.Users> _winners;
        private readonly List<Models.Giveaway.Users> _joinedUsers;
        private readonly Core.Database.Handlers.Currency _databaseCurrencyHandler;
        private readonly System.Timers.Timer timerToEnd;

        public Handler(Models.Giveaway.Properties properties) {
            Properties = properties;
            _joinedUsers = new List<Models.Giveaway.Users>();
            _winners = new List<Models.Giveaway.Users>();
            _databaseCurrencyHandler = new Core.Database.Handlers.Currency();

            if (Properties.TimerActive) {
                timerToEnd = new System.Timers.Timer(
                    TimeSpan.FromMinutes(Properties.TimerValue).TotalMilliseconds);

                timerToEnd.Elapsed += (sender, EventArgs) => StopRegistration(true);
                timerToEnd.Start();
            }
        }

        public void StartRegistration() {
            AivaClient.Instance.TwitchClient.OnChatCommandReceived += ChatCommandReceived;
        }

        public void StopRegistration(bool stopTimer = false) {
            AivaClient.Instance.TwitchClient.OnChatCommandReceived -= ChatCommandReceived;

            if (stopTimer) {
                timerToEnd.Stop();
                OnTimerEnds.Invoke(this, EventArgs.Empty);
            }
        }

        private void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e) {
            if (!CheckCommand(e.Command.CommandText)) {
                return;
            }

            if (HasUserAlreadyWon(Convert.ToInt32(e.Command.ChatMessage.UserId))) {
                return;
            }

            if (!CheckUserCurrency(Convert.ToInt32(e.Command.ChatMessage.UserId))) {
                return;
            }

            if (Properties.BeFollower && !IsViewerFollower(e.Command.ChatMessage.UserId)) {
                return;
            }

            RemoveCurrencyFromViewer(e.Command.ChatMessage.UserId);

            var isSub = CheckIfUserIsSub(e.Command.ChatMessage.DisplayName);
            var user = new Models.Giveaway.Users {
                UserID = Convert.ToInt32(e.Command.ChatMessage.UserId),
                IsSub = isSub,
                Username = e.Command.ChatMessage.DisplayName
            };

            _joinedUsers.Add(user);
            OnJoinedUser.Invoke(this, user);
        }

        public void DoReset() {
            AivaClient.Instance.TwitchClient.OnChatCommandReceived -= ChatCommandReceived;
            AivaClient.Instance.TwitchClient.OnMessageReceived -= MessageReceived;
        }

        public void GetWinner() {
            // templist to add subluck (add multiple times the username to list)
            var tempList = new List<string>();

            // add names to list
            foreach (var user in _joinedUsers) {
                if (user.IsSub && Properties.IsSubLuckActive) {
                    for (int i = 0; i < Properties.SubLuck; i++) {
                        tempList.Add(user.Username);
                    }
                } else {
                    tempList.Add(user.Username);
                }
            }

            // get random winner
            var winnerNumber = new Random().Next(tempList.Count); // generate random number based on count from templist
            var winnerName = tempList[winnerNumber]; // get name from templist with random number
            var winner = _joinedUsers.First(user => string.Compare(user.Username, winnerName) == 0); // get user class from joined users
            _winners.Add(winner); // add winners to list

            // if notify winner
            if (Properties.NotifyWinner) {
                DoNotification(winnerName);
            }

            if (Properties.RemoveWinnerFromList) {
                _joinedUsers.Remove(winner);
            }

            OnWinnerFound.Invoke(this, winner);

            AivaClient.Instance.TwitchClient.OnMessageReceived += MessageReceived;
        }

        private void MessageReceived(object sender, OnMessageReceivedArgs e) {
            if (CheckIfMessageIsFromWinner(e.ChatMessage.DisplayName)) {
                var message = new Models.Giveaway.Messages {
                    Message = e.ChatMessage.Message,
                    Username = e.ChatMessage.Username
                };

                OnWinnerMessageReceived.Invoke(this, message);
            }
        }

        private bool CheckIfMessageIsFromWinner(string displayName) {
            return _winners.Any(u => string.Compare(u.Username, displayName) == 0);
        }

        private void DoNotification(string winnerName) {
            AivaClient.Instance.TwitchClient.SendMessage($"Winner is: @{winnerName}");
        }

        private bool CheckIfUserIsSub(string username) {
            return AivaClient.Instance.TwitchApi.Subscriptions.v3.UserSubscribedToChannelAsync(
                user: username,
                targetChannel: AivaClient.Instance.Channel).Result != null;
        }

        private void RemoveCurrencyFromViewer(string userId) {
            _databaseCurrencyHandler.Remove.Remove(
                twitchID: Convert.ToInt32(userId),
                value: Properties.Price);
        }

        private bool IsViewerFollower(string userId) {
            return AivaClient.Instance.TwitchApi.Users.v5.UserFollowsChannelAsync(
                userId: userId,
                channelId: AivaClient.Instance.ChannelId).Result;
        }

        private bool CheckUserCurrency(int userId) {
            var currency = _databaseCurrencyHandler.GetCurrency(userId);

            if (currency.HasValue) {
                return currency.Value >= Properties.Price;
            }

            return false;
        }

        private bool HasUserAlreadyWon(int userId) {
            return _winners.SingleOrDefault(winner => winner.UserID == userId) != null;
        }

        private bool CheckCommand(string commandText) {
            return string.Compare(Properties.Command.TrimStart('!'), commandText) == 0;
        }
    }
}