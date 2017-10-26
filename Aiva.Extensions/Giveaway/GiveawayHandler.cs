using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TwitchLib.Events.Client;

namespace Aiva.Extensions.Giveaway {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class GiveawayHandler {

        private string _Command;
        public string Command {
            get {
                return _Command;
            }
            set {
                _Command = value.StartsWith("!") ? value : "!" + value;
                _Command = _Command.TrimEnd('!');
            }
        }

        public string Winner { get; set; }
        public int Price { get; set; }
        public int Timer { get; set; }
        public int SubLuck { get; set; }

        public bool BeFollower { get; set; } = true;
        public bool NotifyWinner { get; set; } = true;
        public bool RemoveWinnerFromList { get; set; } = true;
        public bool BlockReEntry { get; set; } = true;

        public bool IsTimerActive { get; set; }
        public bool IsSubLuckActive { get; set; }

        public bool IsStarted { get; set; }

        public Models.JoinPermission SelectedJoinPermission { get; set; }

        public ObservableCollection<Models.Giveaway> JoinedUsers { get; set; }
        public ObservableCollection<Models.Giveaway> Winners { get; set; }
        public ObservableCollection<Models.Giveaway.Messages> Messages { get; set; }

        private Timer EndTimer;

        public GiveawayHandler() {

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
                if (user.IsSub && IsSubLuckActive) {
                    for (int i = 0; i < SubLuck; i++) {
                        tempList.Add(user.Username);
                    }
                } else {
                    tempList.Add(user.Username);
                }
            }

            var winner = tempList.ElementAt(new Random().Next(tempList.Count));
            Winner = winner;

            var joinedUsersWinnerEntry = JoinedUsers.SingleOrDefault(w => String.Compare(w.Username, winner, true) == 0);

            Winners.Add(joinedUsersWinnerEntry);

            if (NotifyWinner)
                DoNotification(winner);

            if (RemoveWinnerFromList)
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
        private async void ChatCommandReceived(object sender, TwitchLib.Events.Client.OnChatCommandReceivedArgs e) {
            if (String.Compare(Command.TrimStart('!'), e.Command.CommandText, true) == 0) {

                if (HasUserAlreadyJoined(e.Command.ChatMessage.UserId)) {
                    return;
                }


                if (HasUserAlreadyWon(e.Command.ChatMessage.UserId)) {
                    if (BlockReEntry)
                        return;
                }

                // Check if the User has enough Currency
                var hasEnoughCurrency = CheckIfEnoughCurrency(e.Command.ChatMessage.UserId);
                if (!hasEnoughCurrency) {
                    return;
                }

                // Check if the User is a follower
                if (BeFollower) {
                    var isFollowing = await TwitchLib.TwitchAPI.Users.v5.UserFollowsChannelAsync(e.Command.ChatMessage.UserId, Core.AivaClient.Instance.ChannelID);

                    if (!isFollowing) {
                        return;
                    }
                }

                // remove currency
                Core.Database.Currency.Remove.RemoveCurrencyFromUser(e.Command.ChatMessage.UserId, Price);

                // add user to list
                JoinedUsers.Add(
                    new Models.Giveaway {
                        Username = e.Command.ChatMessage.Username,
                        UserID = e.Command.ChatMessage.UserId,
                        IsSub = await IsUserSub(e.Command.ChatMessage.Username),
                    });
            }
        }

        /// <summary>
        /// Checks if the user is sub
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task<bool> IsUserSub(string username) {
            return await TwitchLib.TwitchAPI.Subscriptions.v3.ChannelHasUserSubscribedAsync(Core.AivaClient.Instance.Channel, username) != null;
        }

        /// <summary>
        /// Check if the user is already won
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool HasUserAlreadyWon(string userId) {
            return (Winners != null && Winners.SingleOrDefault(u => String.Compare(userId, u.UserID, true) == 0) != null);
        }

        /// <summary>
        /// Check if the user is already in the list
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool HasUserAlreadyJoined(string userId) {
            // exist in joinedusers || winners ?
            return JoinedUsers != null && JoinedUsers.SingleOrDefault(u => String.Compare(u.UserID, userId, true) == 0) != null;
        }

        /// <summary>
        /// Check if the User has enough currency
        /// </summary>
        /// <param name="userId">Twitch UserID</param>
        /// <returns>True -> Has enough | False -> Hasnt enough</returns>
        private bool CheckIfEnoughCurrency(string userId) {
            var currency = Core.Database.Currency.GetCurrencyFromUser(userId);

            if (currency.HasValue) {
                if (currency.Value >= Price) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Reset values to standard
        /// </summary>
        public void Reset() {
            Command = String.Empty;
            Winner = String.Empty;
            Price = 1;
            Timer = 1;
            SubLuck = 0;
            BeFollower = true;
            NotifyWinner = true;
            RemoveWinnerFromList = true;
            BlockReEntry = true;

            IsTimerActive = false;
            IsSubLuckActive = false;
            SelectedJoinPermission = Models.JoinPermission.Everyone;
            JoinedUsers = new ObservableCollection<Models.Giveaway>();
            Winners = new ObservableCollection<Models.Giveaway>();
            Messages = new ObservableCollection<Models.Giveaway.Messages>();
            EndTimer = null;
            Core.AivaClient.Instance.AivaTwitchClient.OnMessageReceived -= ChatMessageReceived;
            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived -= ChatCommandReceived;
            IsStarted = false;
        }

        #region Timer
        /// <summary>
        /// Set the Timer
        /// </summary>
        private void SetTimer() {
            EndTimer = new Timer(60000) {
                AutoReset = true
            }; // every minute
            EndTimer.Elapsed += EndTimer_Elapsed;
            EndTimer.Start();
        }

        /// <summary>
        /// When timer ticks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndTimer_Elapsed(object sender, ElapsedEventArgs e) {
            if (Timer != 0) {
                Timer--;
            } else {
                EndTimer.Stop();
                StopGiveaway();
            }
        }
        #endregion Timer
    }
}
