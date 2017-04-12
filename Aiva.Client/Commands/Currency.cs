namespace Client.Commands {
    public class Currency {
        /// <summary>
        /// If the user want to know how much currency he/she has
        /// </summary>
        /// <param name="username"></param>
        internal static void WriteCurrencyForUser(string username) {
            var currency = Database.CurrencyHandler.GetCurrency(username);

            string message = $"@{username}: {currency} " + Config.Language.Instance.GetString("CurrencyName");

            Client.ClientBBB.TwitchClientBBB.SendMessage(message);
        }
    }
}
