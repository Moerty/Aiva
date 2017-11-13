using Aiva.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using TwitchLib.Events.Client;

namespace Aiva.Extensions.Giveaway {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class GiveawayHandler {
        public ObservableCollection<Models.Giveaway> JoinedUsers { get; set; }
        public ObservableCollection<Models.Giveaway> Winners { get; set; }
        public ObservableCollection<Models.Giveaway.Messages> Messages { get; set; }
        public bool IsStarted { get; set; }
        public bool IsTimerActive { get; set; }
        public string Winner { get; set; }

        public Models.Giveaway.Properties Properties;

        private Timer _endTimer;
        private readonly Core.DatabaseHandlers.Currency _currencyDatabaseHandler;

        public GiveawayHandler() {
            _currencyDatabaseHandler = new Core.DatabaseHandlers.Currency();
        }

        /// <summary>
        /// Start the Giveaway
        /// </summary>
        public void StartGiveaway() {
            JoinedUsers = new ObservableCollection<Models.Giveaway>();
            Winners = new ObservableCollection<Models.Giveaway>();

            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived += ChatCommandReceived;
            IsStarted = true;

            if (IsTimerActive)
                SetTimer();
        }

        /// <summary>
        /// Stop all events
        /// Stop from joining the giveaway
        /// </summary>
        public void StopRegistration() {
            Core.AivaClient.Instance.AivaTwitchClient.OnMessageReceived -= ChatMessageReceived;
            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived -= ChatCommandReceived;
        }

        /// <summary>
        /// Stop the Giveaway
        /// </summary>
        public void StopGiveaway() {
            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived -= ChatCommandReceived;
            IsStarted = false;

            Core.AivaClient.Instance.AivaTwitchClient.OnMessageReceived += ChatMessageReceived;
        }

        /// <summary>
        /// Adds the winner message to list for the listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatMessageReceived(object sender, OnMessageReceivedArgs e) {
            var isUserInList = Winners.SingleOrDefault(u => String.Compare(u.UserID, e.ChatMessage.UserId, true) == 0);

            if (isUserInList != null) {
                Messages.Add(
                    new Models.Giveaway.Messages {
                        Username = e.ChatMessage.Username,
                        Message = e.ChatMessage.Message
                    });
            }
        }

        /// <summary>
        /// Get one Winner
        /// </summary>
        public void GetWinner() {
            var tempList = new List<string>();

            foreach (var user in JoinedUsers) {
                if (user.IsSub && Properties.IsSubLuckActive) {
                    for (int i = 0; i < Properties.SubLuck; i++) {
                        tempList.Add(user.Username);
                    }
                } else {
                    tempList.Add(user.Username);
                }
            }

            var winner = tempList[new Random().Next(tempList.Count)];
            Winner = winner;

            var joinedUsersWinnerEntry = JoinedUsers.SingleOrDefault(w => String.Compare(w.Username, winner, true) == 0);

            Winners.Add(joinedUsersWinnerEntry);

            if (Properties.NotifyWinner)
                DoNotification(winner);

            if (Properties.RemoveWinnerFromList)
                JoinedUsers.Remove(joinedUsersWinnerEntry);
        }

        /// <summary>
        /// Do notification in chat
        /// </summary>
        /// <param name="winner"></param>
        private void DoNotification(string winner) {
            Core.AivaClient.Instance.AivaTwitchClient.SendMessage($"Winner is: @{winner}");
        }

        /// <summary>
        /// User wants to join Giveaway
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e) {
            if (String.Compare(Properties.Command.TrimStart('!'), e.Command.CommandText, true) == 0) {
                if (HasUserAlreadyJoined(e.Command.ChatMessage.UserId)) {
                    return;
                }

                if (HasUserAlreadyWon(e.Command.ChatMessage.UserId)) {
                    if (Properties.BlockReEntry)
                        return;
                }

                // Check if the User has enough Currency
                var hasEnoughCurrency = CheckIfEnoughCurrency(e.Command.ChatMessage.UserId);
                if (!hasEnoughCurrency) {
                    return;
                }

                // Check if the User is a follower
                if (Properties.BeFollower) {
                    var isFollowing = await AivaClient.Instance.TwitchApi.Users.v5.UserFollowsChannelAsync(e.Command.ChatMessage.UserId, Core.AivaClient.Instance.ChannelID).ConfigureAwait(false);

                    if (!isFollowing) {
                        return;
                    }
                }

                // remove currency
                _currencyDatabaseHandler.Remove.Remove(e.Command.ChatMessage.UserId, Properties.Price);

                // add user to list
                var isUserSub = await IsUserSub(e.Command.ChatMessage.Username).ConfigureAwait(false);
                Application.Current.Dispatcher.Invoke(() => {
                    JoinedUsers.Add(
                    new Models.Giveaway {
                        Username = e.Command.ChatMessage.Username,
                        UserID = e.Command.ChatMessage.UserId,
                        IsSub = isUserSub,
                    });
                });
            }
        }

        /// <summary>
        /// Checks if the user is sub
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task<bool> IsUserSub(string username) {
            return await AivaClient.Instance.TwitchApi.Subscriptions.v3.ChannelHasUserSubscribedAsync(Core.AivaClient.Instance.Channel, username).ConfigureAwait(false) != null;
        }

        /// <summary>
        /// Check if the user is already won
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool HasUserAlreadyWon(string userId) {
            return Winners?.SingleOrDefault(u => String.Compare(userId, u.UserID, true) == 0) != null;
        }

        /// <summary>
        /// Check if the user is already in the list
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool HasUserAlreadyJoined(string userId) {
            // exist in joinedusers || winners ?
            return JoinedUsers?.SingleOrDefault(u => String.Compare(u.UserID, userId, true) == 0) != null;
        }

        /// <summary>
        /// Check if the User has enough currency
        /// </summary>
        /// <param name="userId">Twitch UserID</param>
        /// <returns>True -> Has enough | False -> Hasnt enough</returns>
        private bool CheckIfEnoughCurrency(string userId) {
            var currency = _currencyDatabaseHandler.GetCurrency(userId);

            if (currency.HasValue) {
                if (currency.Value >= Properties.Price) {
                    return true;
                }
            }

            return false;
        }

        #region Timer

        /// <summary>
        /// Set the Timer
        /// </summary>
        private void SetTimer() {
            _endTimer = new Timer(60000) {
                AutoReset = true
            }; // every minute
            _endTimer.Elapsed += EndTimer_Elapsed;
            _endTimer.Start();
        }

        /// <summary>
        /// When timer ticks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndTimer_Elapsed(object sender, ElapsedEventArgs e) {
            if (Properties.Timer != 0) {
                Properties.Timer--;
            } else {
                _endTimer.Stop();
                StopGiveaway();
            }
        }

        #endregion Timer
    }
}