using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Events.Client;
using Client.Tasks;

namespace Client.Tasks
{
    public class Tasks
    {
        public static TwitchLib.TwitchClient GetEvents(TwitchLib.TwitchClient Client)
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

            return Client;
        }

        private static void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            // Check Currency
            if (String.Compare(e.Command.Command, Config.Currency.Config["General"]["UserCommandCheckCurrency"], true) == 0)
                Commands.Currency.WriteCurrencyForUser(e.Command.ChatMessage.Username);

            // ModCommand
            ModCommands.Currency modCurrency = new ModCommands.Currency(e.Command.Command, e.Command.ArgumentsAsList);
        }

        private static void Client_OnExistingUsersDetected(object sender, OnExistingUsersDetectedArgs e)
        {
            foreach (var user in e.Users)
            {
                //while (Client.ClientBBB.TwitchClientBBB.IsConnected == false)
                //{
                //    Task.Delay(500);
                //}

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
                    TwitchApi.Users.GetUserAsync(e.Username).Result);

            Database.UserHandler.UpdateUser.SetIsViewingAsync(e.Username, true);
        }

        public static async Task<List<TwitchLib.Models.API.Block.Block>> LoadBlockedUserAsync()
        {
            var blockedUser = await _getBansAsync();


            return blockedUser;
        }

        public static async Task<List<TwitchLib.Models.API.Block.Block>> loadListAsync()
        {
            return await _getBansAsync();
        }

        private async static Task<List<TwitchLib.Models.API.Block.Block>> _getBansAsync()
        {
            var result = await TwitchLib.TwitchApi.Blocks.GetBlockedListAsync(Config.General.Config["General"]["BotName"]);

            return result;
        }

        public async static void BannUserAsync(string name)
        {
            await TwitchLib.TwitchApi.Blocks.BlockUserAsync(
                Config.General.Config["General"]["BotName"],
                name,
                Config.General.Config["Credentials"]["TwitchOAuth"]
                );
        }

        public static void UnbanUser(string name)
        {
            TwitchLib.TwitchApi.Blocks.UnblockUserAsync(
                Config.General.Config["General"]["BotName"],
                name,
                Config.General.Config["Credentials"]["TwitchOAuth"]);
        }

        public static async Task<TwitchLib.Models.API.User.User> GetUserAsync(string name) => await TwitchLib.TwitchApi.Users.GetUserAsync(name);

        /*private static void TwitchClient_OnUserBanned(object sender, OnUserBannedArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                Controls.Flyout.ucBans.bannedUsers.Add(e.Username);
            });
        }*/
    }
}
