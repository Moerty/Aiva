using Aiva.Core.Twitch;
using Aiva.Models.Enums;
using Aiva.Models.Voting;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Events.Client;

namespace Aiva.Extensions.Voting {
    public class Handler {
        public event EventHandler<Models.Voting.VotedUser> OnUserVoted;

        private readonly Properties _properties;
        private readonly List<int> _registredUserIds;
        private readonly Core.Database.Handlers.Currency _databaseCurrencyHandler;

        public Handler(Properties properties) {
            _properties = properties;
            _registredUserIds = new List<int>();
            _databaseCurrencyHandler = new Core.Database.Handlers.Currency();
        }

        public void StartListining() {
            AivaClient.Instance.TwitchClient.OnChatCommandReceived += ChatCommandReceived;
        }

        private async void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e) {
            if (!CheckCommand(e.Command.CommandText)) {
                return;
            }

            // multi vote
            if (_properties.BlockMultiRegister) {
                if ((_registredUserIds.SingleOrDefault(u => u == Convert.ToInt32(e.Command.ChatMessage.UserId))) != 0) {
                    return;
                }
            }

            // join permission
            if (_properties.JoinPermission != Models.Enums.JoinPermission.Everyone) {
                if (_properties.JoinPermission == Models.Enums.JoinPermission.Subscriber) {
                    if (!e.Command.ChatMessage.IsSubscriber) {
                        return;
                    }
                }

                if (_properties.JoinPermission == Models.Enums.JoinPermission.Moderation) {
                    if (!e.Command.ChatMessage.IsModerator) {
                        return;
                    }
                }
            }

            // check follower
            if (_properties.BeFollower) {
                var isFollowing = await AivaClient.Instance.TwitchApi.Users.v5.UserFollowsChannelAsync(
                    userId: e.Command.ChatMessage.UserId,
                    channelId: AivaClient.Instance.ChannelId).ConfigureAwait(false);

                if (!isFollowing) {
                    return;
                }
            }

            // check currency
            if (_properties.IsCostEnabled) {
                var userId = Convert.ToInt32(e.Command.ChatMessage.UserId);
                if (!_databaseCurrencyHandler.HasUserEnoughCurrency(
                    twitchID: userId,
                    currencyToCheck: _properties.Cost)) {
                    return;
                } else {
                    _databaseCurrencyHandler.Remove.Remove(
                        twitchID: userId,
                        value: _properties.Cost);
                }
            }

            ConfirmUserRegistration(e);
        }

        public void StopListining() {
            AivaClient.Instance.TwitchClient.OnChatCommandReceived -= ChatCommandReceived;
        }

        private void ConfirmUserRegistration(OnChatCommandReceivedArgs e) {
            var userID = Convert.ToInt32(e.Command.ChatMessage.UserId);
            var votingOption = GetVotingOption(e.Command.ArgumentsAsString);

            if (votingOption != VotingOption.Unknown) {
                var votedUser = new Models.Voting.VotedUser {
                    Id = userID,
                    Name = e.Command.ChatMessage.DisplayName,
                    Option = votingOption
                };

                OnUserVoted?.Invoke(this, votedUser);
                _registredUserIds.Add(userID);
            }
        }

        private VotingOption GetVotingOption(string argumentsAsString) {
            if (string.Compare(argumentsAsString, _properties.Options.Option1.Option) == 0) {
                return VotingOption.Option1;
            }

            if (string.Compare(argumentsAsString, _properties.Options.Option2.Option) == 0) {
                return VotingOption.Option2;
            }

            if (string.Compare(argumentsAsString, _properties.Options.Option3.Option) == 0) {
                return VotingOption.Option2;
            }

            if (string.Compare(argumentsAsString, _properties.Options.Option4.Option) == 0) {
                return VotingOption.Option2;
            }

            if (string.Compare(argumentsAsString, _properties.Options.Option5.Option) == 0) {
                return VotingOption.Option2;
            }

            if (string.Compare(argumentsAsString, _properties.Options.Option6.Option) == 0) {
                return VotingOption.Option2;
            }

            return VotingOption.Unknown;
        }

        private bool CheckCommand(string commandText) {
            return string.Compare(
                _properties.Command.TrimStart('!'),
                commandText) == 0;
        }
    }
}