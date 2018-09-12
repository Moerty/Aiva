using System;
using TwitchLib.Client.Events;

namespace Aiva.Core.Twitch.Tasks.Commands.ModCommands
{
    public class Currency
    {
        #region Models
        public AddCurrency Add;
        public TransferCurrency Transfer;
        public RemoveCurrency Remove;

        #endregion Models

        #region Constructor
        public Currency()
        {
            Add = new AddCurrency();
            Transfer = new TransferCurrency();
            Remove = new RemoveCurrency();
        }
        #endregion Constructor

        #region Functions

        /// <summary>
        /// Fires when a command received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            if (String.Compare(Config.Config.Instance.Storage.ModCommands.ModCurrency.AddCurrency, e.Command.CommandText, true) == 0)
            {
                Add.ChatCommandReceived(sender, e);
            }

            if (String.Compare(Config.Config.Instance.Storage.ModCommands.ModCurrency.RemoveCurrency, e.Command.CommandText, true) == 0)
            {
                Remove.ChatCommandReceived(sender, e);
            }

            if (String.Compare(Config.Config.Instance.Storage.ModCommands.ModCurrency.TransferCurrency, e.Command.CommandText, true) == 0)
            {
                Transfer.ChatCommandReceived(sender, e);
            }

            if(e.Command.CommandText == "currency" && e.Command.ArgumentsAsList.Count == 1) {
                GetCurrencyForUser(e);
            }
        }

        /// <summary>
        /// Get currency for user
        /// </summary>
        /// <param name="e"></param>
        private async void GetCurrencyForUser(OnChatCommandReceivedArgs e) {
            if(e.Command.ChatMessage.UserType.IsUserPermitted()) {
                var user = await AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(e.Command.ArgumentsAsList[0]).ConfigureAwait(false);

                if(user?.Total > 0) {
                    var currency = new Database.Handlers.Currency().GetCurrency(
                        Convert.ToInt32(user.Matches[0].Id));

                    if(currency.HasValue) {
                        AivaClient.Instance.TwitchClient.SendMessage(
                            AivaClient.Instance.Channel,
                            $"@{e.Command.ChatMessage.DisplayName}: Viewer {e.Command.ArgumentsAsList[0]} has {currency.Value} currency!",
                            AivaClient.DryRun);
                    }
                }
            }
        }

        #endregion Functions

        #region Add

        /// <summary>
        /// Add Currency ModCommands
        /// </summary>
        public class AddCurrency
        {
            private Database.Handlers.Currency.AddCurrency _addCurrency;
            private readonly Database.Handlers.Users _users;

            public AddCurrency()
            {
                _addCurrency = new Database.Handlers.Currency.AddCurrency();
                _users = new Database.Handlers.Users();
            }

            /// <summary>
            /// Main method to check if the sender is permitted
            /// and gets the user
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public async void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
            {
                if (e.Command.ChatMessage.UserType.IsUserPermitted())
                {
                    if (e.Command.ArgumentsAsList.Count > 1)
                    {
                        if (int.TryParse(e.Command.ArgumentsAsList[1], out int value))
                        {
                            var user = await AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(e.Command.ArgumentsAsList[0]).ConfigureAwait(false);

                            if (user?.Total > 0)
                            {
                                AddCurrencyToUser(
                                    senderName: e.Command.ChatMessage.DisplayName,
                                    username: user.Matches[0].DisplayName,
                                    userid: Convert.ToInt32(user.Matches[0].Id),
                                    value: value);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Add currency to a user
            /// </summary>
            /// <param name="senderName"></param>
            /// <param name="username"></param>
            /// <param name="userid"></param>
            /// <param name="value"></param>
            public void AddCurrencyToUser(string senderName, string username, int userid, int value)
            {
                if (_addCurrency == null)
                    _addCurrency = new Database.Handlers.Currency.AddCurrency();

                var result = _addCurrency.Add(userid, value);

                if (result)
                {
                    AivaClient.Instance.TwitchClient.SendMessage(
                        AivaClient.Instance.Channel,
                                    $"@{senderName} : {username} added {value} currency!");
                }
            }
        }

        #endregion Add

        #region Transfer
        /// <summary>
        /// Transfer Currency ModCommands
        /// </summary>
        public class TransferCurrency
        {
            private readonly Database.Handlers.Currency.TransferCurrency _transferCurrency;

            public TransferCurrency()
            {
                _transferCurrency = new Database.Handlers.Currency.TransferCurrency();
            }

            public async void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
            {
                if (e.Command.ChatMessage.UserType.IsUserPermitted())
                {
                    if (int.TryParse(e.Command.ArgumentsAsList[2], out int value))
                    {
                        var user1 = await AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(e.Command.ArgumentsAsList[0]).ConfigureAwait(false);
                        var user2 = await AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(e.Command.ArgumentsAsList[1]).ConfigureAwait(false);

                        if (user1?.Total > 0 && user2?.Total > 0)
                        {
                            TransferCurrencyFromUser(
                                senderName: e.Command.ChatMessage.DisplayName,
                                username1: user1.Matches[0].DisplayName,
                                userid1: Convert.ToInt32(user1.Matches[0].Id),
                                username2: user2.Matches[0].DisplayName,
                                userid2: Convert.ToInt32(user2.Matches[0].Id),
                                value: value);
                        }
                    }
                }
            }

            /// <summary>
            /// Transfer currency
            /// </summary>
            /// <param name="senderName"></param>
            /// <param name="username1"></param>
            /// <param name="userid1"></param>
            /// <param name="username2"></param>
            /// <param name="userid2"></param>
            /// <param name="value"></param>
            private void TransferCurrencyFromUser(string senderName, string username1, int userid1, string username2, int userid2, int value)
            {
                var result = _transferCurrency.Transfer(userid1, userid2, value);

                if (result)
                {
                    AivaClient.Instance.TwitchClient.SendMessage(
                        AivaClient.Instance.Channel,
                        $"@{senderName} : Transfer {value} currency from {username1} to {username2}");
                }
            }
        }

        #endregion Transfer

        #region Remove

        /// <summary>
        /// Remove Currency ModCommands
        /// </summary>
        public class RemoveCurrency
        {
            private readonly Database.Handlers.Currency.RemoveCurrency _removeCurrency;

            public RemoveCurrency()
            {
                _removeCurrency = new Database.Handlers.Currency.RemoveCurrency();
            }

            public async void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
            {
                if (e.Command.ChatMessage.UserType.IsUserPermitted())
                {
                    if (int.TryParse(e.Command.ArgumentsAsList[1], out int value))
                    {
                        var user = await AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(e.Command.ArgumentsAsList[0]).ConfigureAwait(false);

                        if (user?.Total > 0)
                        {
                            RemoveCurrencyFromUser(
                                senderName: e.Command.ChatMessage.DisplayName,
                                username: user.Matches[0].DisplayName,
                                userid: Convert.ToInt32(user.Matches[0].Id),
                                value: value);
                        }
                    }
                }
            }

            private void RemoveCurrencyFromUser(string senderName, string username, int userid, int value)
            {
                var result = _removeCurrency.Remove(userid, value);

                if (result)
                {
                    AivaClient.Instance.TwitchClient.SendMessage(
                        AivaClient.Instance.Channel,
                        $"@{senderName} : {username} removed {value} currency!");
                }
            }
        }

        #endregion Remove
    }
}