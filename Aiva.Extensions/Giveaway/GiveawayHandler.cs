using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Giveaway {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class GiveawayHandler {
        public string Command { get; set; }
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

        public Models.JoinPermission SelectedJoinPermission { get; set; }

        public ObservableCollection<Models.Giveaway> JoinedUsers { get; set; }
        public ObservableCollection<Models.Giveaway> Winners { get; set; }

        public GiveawayHandler() {

        }

        public void StartGiveaway() {
            JoinedUsers = new ObservableCollection<Models.Giveaway>();

            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived += ChatCommandReceived;
        }

        private async void ChatCommandReceived(object sender, TwitchLib.Events.Client.OnChatCommandReceivedArgs e) {
            if (String.Compare(Command, e.Command.Command, true) == 0) {

                // exist in joinedusers || winners ?
                if (JoinedUsers != null && JoinedUsers.SingleOrDefault(u => String.Compare(u.UserID, e.Command.ChatMessage.UserId, true) == 0) != null)
                    return;

                if (Winners != null && Winners.SingleOrDefault(u => String.Compare(e.Command.ChatMessage.UserId, u.UserID, true) == 0) != null)
                    return;



                // Check if follower
                if (BeFollower) {
                    var isFollowing = await TwitchLib.TwitchAPI.Users.v5.UserFollowsChannel(e.Command.ChatMessage.UserId, Core.AivaClient.Instance.ChannelID);

                    if (isFollowing) {

                        // CheckIfEnoughCurrency
                        var enoughCurrency = CheckIfEnoughCurrency(e.Command.ChatMessage.UserId);

                        if (enoughCurrency) {

                            // Remove Currency from User
                            Core.Database.Currency.Remove.RemoveCurrencyFromUser(e.Command.ChatMessage.UserId, Price);

                            // Add User to List
                            JoinedUsers.Add(new Models.Giveaway {
                                Username = e.Command.ChatMessage.Username,
                                UserID = e.Command.ChatMessage.UserId
                            });
                        }
                    }
                }
            }
        }

        private bool CheckIfEnoughCurrency(string userId) {
            var currency = Core.Database.Currency.GetCurrencyFromUser(userId);

            if (currency.HasValue) {
                if (currency.Value >= Price) {
                    return true;
                }
            }

            return false;
        }

        class Model {

        }
    }
}
