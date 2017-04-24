using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Events.Client;

namespace Aiva.Core.Client.Tasks
{
    public class Tasks
    {
        public static TwitchClient GetEvents(TwitchLib.TwitchClient Client)
        {
            //case "OnUserJoined":
            Client.OnUserJoined += Client_OnUserJoined;
            Client.OnUserJoined += TimeWatched.AddUser;

            //case "OnUserLeft":
            Client.OnUserLeft += Client_OnUserLeft;
            Client.OnUserLeft += TimeWatched.RemoveUser;

            //case "OnChatCommandReceived":
            Client.OnChatCommandReceived += Client_OnChatCommandReceived;

            //case "OnExistingUsersDetected":
            Client.OnExistingUsersDetected += Client_OnExistingUsersDetected;

            // case "OnMessageReceive
            if (Database.UserSettingsHandler.GetBoolean("BlackListedWordsActive"))
                Client.OnMessageReceived += ChatChecker.BlacklistWordsChecker;

            if (Database.UserSettingsHandler.GetBoolean("Spamcheck"))
                Client.OnMessageReceived += ChatChecker.CheckMessage;

            if (!Convert.ToBoolean(Config.GeneralConfig.Config["SpamCheck"]["AllowViewerToPostLinks"]))
                Client.OnMessageReceived += ChatChecker.LinkChecker;

            // OnNewSubscriber
            Client.OnNewSubscriber += Client_OnNewSubscriber;

            return Client;
        }

        /// <summary>
        /// Send Message when new Susbcriber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (!e.Subscriber.IsTwitchPrime)
            {
                if (Convert.ToBoolean(Config.GeneralConfig.Config["Interactions"]["WriteInChatNormalSub"]))
                {
                    AivaClient.Client.AivaTwitchClient.SendMessage(
                        Config.LanguageConfig.Instance.GetString("InteractionChatMessageNormalSub")
                        .Replace("@USERNAME@", e.Subscriber.Name));
                }
            }
            else
            {
                if (Convert.ToBoolean(Config.GeneralConfig.Config["Interactions"]["WriteInChatPrimeSub"]))
                {
                    AivaClient.Client.AivaTwitchClient.SendMessage(
                        Config.LanguageConfig.Instance.GetString("InteractionChatMessagePrimeSub")
                        .Replace("@USERNAME@", e.Subscriber.Name));
                }
            }
        }

        private static void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            // Check Currency
            if (String.Compare(e.Command.Command, Config.CurrencyConfig.Config["General"]["UserCommandCheckCurrency"], true) == 0)
                Commands.Currency.WriteCurrencyForUser(e.Command.ChatMessage.Username);

            // ModCommand
            var modCurrency = new Core.Commands.ModCommands.Currency(e.Command.Command, e.Command.ArgumentsAsList);
        }

        private static void Client_OnExistingUsersDetected(object sender, OnExistingUsersDetectedArgs e)
        {
            foreach (var user in e.Users)
            {
                Database.UserHandler.AddUser.AddUserToDatabase(
                        TwitchApi.Users.GetUserAsync(user).Result);


                Database.UserHandler.UpdateUser.UpdateLastSeenAsync(user);

                Database.UserHandler.UpdateUser.SetIsViewingAsync(user, true);
            }
        }

        private static void Client_OnUserLeft(object sender, OnUserLeftArgs e)
        {
            Database.UserHandler.UpdateUser.UpdateLastSeenAsync(e.Username);

            Database.UserHandler.UpdateUser.SetIsViewingAsync(e.Username, false);
        }

        private static void Client_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            Database.UserHandler.AddUser.AddUserToDatabase(
                    TwitchApi.Users.GetUser(e.Username));

            Database.UserHandler.UpdateUser.SetIsViewingAsync(e.Username, true);
        }

        public static async Task<List<TwitchLib.Models.API.Block.Block>> LoadBlockedUserAsync()
        {
            var blockedUser = await _getBansAsync();


            return blockedUser;
        }

        public static async Task<List<TwitchLib.Models.API.Block.Block>> LoadListAsync()
        {
            return await _getBansAsync();
        }

        private async static Task<List<TwitchLib.Models.API.Block.Block>> _getBansAsync()
        {
            var result = await TwitchLib.TwitchApi.Blocks.GetBlockedListAsync(Config.GeneralConfig.Config["General"]["BotName"]);

            return result;
        }

        public async static void BannUserAsync(string name)
        {
            await TwitchLib.TwitchApi.Blocks.BlockUserAsync(
                Config.GeneralConfig.Config["General"]["BotName"],
                name,
                Config.GeneralConfig.Config["Credentials"]["TwitchOAuth"]
                );
        }

        public static void UnbanUser(string name)
        {
            TwitchLib.TwitchApi.Blocks.UnblockUserAsync(
                Config.GeneralConfig.Config["General"]["BotName"],
                name,
                Config.GeneralConfig.Config["Credentials"]["TwitchOAuth"]);
        }

        public static async Task<TwitchLib.Models.API.User.User> GetUserAsync(string name) => await TwitchLib.TwitchApi.Users.GetUserAsync(name);
    }
}
