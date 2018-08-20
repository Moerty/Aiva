using System;
using TwitchLib.Client.Events;

namespace Aiva.Core.Twitch.Tasks {
    public class Currency {
        private readonly Database.Handlers.Currency _currencyDatabaseHandler;

        public Currency() {
            _currencyDatabaseHandler = new Database.Handlers.Currency();
        }

        internal void CommandReceived(object sender, OnChatCommandReceivedArgs e) {
            if (String.Compare(e.Command.CommandText, Config.Config.Instance.Storage.Currency.CurrencyCommands.GetCurrency, true) == 0) {
                var currency = _currencyDatabaseHandler
                    .GetCurrency(Convert.ToInt32(e.Command.ChatMessage.UserId));

                if (currency.HasValue) {
                    AivaClient.Instance.TwitchClient.SendMessage(
                        AivaClient.Instance.Channel,
                        $"@{e.Command.ChatMessage.DisplayName} : You have {currency.Value} currency!");
                }
            }
        }
    }
}