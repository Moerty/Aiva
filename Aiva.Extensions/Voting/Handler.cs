using System;
using System.Collections.ObjectModel;
using TwitchLib.Events.Client;
using System.Linq;
using System.Windows;

namespace Aiva.Extensions.Voting {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Handler {
        #region Models
        public bool IsStarted { get; set; }
        public Models.Voting.Properties Properties { get; set; }
        public Models.Voting.ChartValues ChartValues { get; set; }
        public ObservableCollection<Models.Voting.VotedUsers> VotedUsers { get; set; }

        private readonly Core.DatabaseHandlers.Currency _currencyDatabaseHandler;
        #endregion Models

        #region Constructor
        public Handler() {
            VotedUsers = new ObservableCollection<Models.Voting.VotedUsers>();
            _currencyDatabaseHandler = new Core.DatabaseHandlers.Currency();
        }
        #endregion Constructor

        #region Functions
        public void StopRegistration() {
            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived -= ChatCommandReceived;
        }

        private async void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e) {
            var command = Properties.Command.TrimStart('!');

            if (string.Compare(command, e.Command.CommandText, true) == 0) {
                // price
                if (Properties.IsCostEnabled) {
                    if (!_currencyDatabaseHandler.HasUserEnoughCurrency(e.Command.ChatMessage.UserId, Properties.Cost)) {
                        return;
                    }
                }

                // multi vote
                if (Properties.BlockMultiRegister) {
                    if (VotedUsers.SingleOrDefault(u => String.Compare(u.Id, e.Command.ChatMessage.UserId) == 0) != null) {
                        return;
                    }
                }

                // permissions
                if (Properties.JoinPermission != Enums.JoinPermission.Everyone) {
                    if (Properties.JoinPermission == Enums.JoinPermission.Subscriber) {
                        if (!e.Command.ChatMessage.IsSubscriber) {
                            return;
                        }
                    }

                    if (Properties.JoinPermission == Enums.JoinPermission.Moderation) {
                        if (!e.Command.ChatMessage.IsModerator) {
                            return;
                        }
                    }
                }

                // follwing
                if (Properties.BeFollower) {
                    var follwing = await Core.AivaClient.Instance.TwitchApi.Users.v5.UserFollowsChannelAsync(e.Command.ChatMessage.UserId, Core.AivaClient.Instance.ChannelID).ConfigureAwait(false);

                    if (!follwing) {
                        return;
                    }
                }

                // remove costs from user
                if (Properties.IsCostEnabled) {
                    _currencyDatabaseHandler.Remove.Remove(
                        e.Command.ChatMessage.UserId,
                        Properties.Cost);
                }

                if (String.Compare(e.Command.ArgumentsAsString, Properties.Options.Option1.Option, true) == 0) {
                    ChartValues.Option1.Value++;
                    AddToVotedUsers(e.Command.ChatMessage.UserId, e.Command.ChatMessage.DisplayName, e.Command.ArgumentsAsString);
                } else if (String.Compare(e.Command.ArgumentsAsString, Properties.Options.Option2.Option, true) == 0) {
                    ChartValues.Option2.Value++;
                    AddToVotedUsers(e.Command.ChatMessage.UserId, e.Command.ChatMessage.DisplayName, e.Command.ArgumentsAsString);
                } else if (String.Compare(e.Command.ArgumentsAsString, Properties.Options.Option3.Option, true) == 0) {
                    ChartValues.Option3.Value++;
                    AddToVotedUsers(e.Command.ChatMessage.UserId, e.Command.ChatMessage.DisplayName, e.Command.ArgumentsAsString);
                } else if (String.Compare(e.Command.ArgumentsAsString, Properties.Options.Option4.Option, true) == 0) {
                    ChartValues.Option4.Value++;
                    AddToVotedUsers(e.Command.ChatMessage.UserId, e.Command.ChatMessage.DisplayName, e.Command.ArgumentsAsString);
                } else if (String.Compare(e.Command.ArgumentsAsString, Properties.Options.Option5.Option, true) == 0) {
                    ChartValues.Option5.Value++;
                    AddToVotedUsers(e.Command.ChatMessage.UserId, e.Command.ChatMessage.DisplayName, e.Command.ArgumentsAsString);
                } else if (String.Compare(e.Command.ArgumentsAsString, Properties.Options.Option6.Option, true) == 0) {
                    ChartValues.Option6.Value++;
                    AddToVotedUsers(e.Command.ChatMessage.UserId, e.Command.ChatMessage.DisplayName, e.Command.ArgumentsAsString);
                }
            }
        }

        private void AddToVotedUsers(string userid, string name, string options) {
            var user = new Models.Voting.VotedUsers {
                Id = userid,
                Name = name,
                Option = options
            };

            Application.Current.Dispatcher.Invoke(() => VotedUsers.Add(user));
        }

        public void StartRegistration() {
            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived += ChatCommandReceived;
        }

        public void StopVoting() {
            StopRegistration();
            IsStarted = false;
        }
        #endregion Functions
    }
}