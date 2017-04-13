namespace Aiva.Core.Client.Commands {
    public class Currency {
        /// <summary>
        /// write user currency value
        /// </summary>
        /// <param name="username"></param>
        internal static void WriteCurrencyForUser(string username) {
            var currency = Database.CurrencyHandler.GetCurrency(username);

            var message = $"@{username}: {currency} " + Config.LanguageConfig.Instance.GetString("CurrencyName");

            AivaClient.Client.AivaTwitchClient.SendMessage(message);
        }
    }
}
