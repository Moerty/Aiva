using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Events.Client;
using Aiva.Core.Client.Tasks.Functions;

namespace Aiva.Core.Client.Tasks {
    public class Tasks {
        public static TwitchClient GetEvents(TwitchLib.TwitchClient Client) {
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
            Client = OnMessageReceived.OnMessageReceive(Client);

            // OnNewSubscriber
            Client.OnNewSubscriber += Client_OnNewSubscriber;

            return Client;
        }

        /// <summary>
        ///  On Message Received Events
        /// </summary>
        public static class OnMessageReceived {
            public static bool IsBlacklistedWordsActive { get; set; }
            public static bool IsSpamCheckActive { get; set; }
            public static bool IsAllowViewerToPostLinksActive { get; set; }
            public static bool IsCapsCheckActive { get; set; }

            /// <summary>
            /// Initial Events
            /// </summary>
            /// <param name="Client"></param>
            /// <returns></returns>
            public static TwitchClient OnMessageReceive(TwitchClient Client) {
                if (Database.UserSettingsHandler.GetBoolean("BlackListedWordsActive")) {
                    Client.OnMessageReceived += ChatChecker.BlacklistWordsChecker;
                    IsBlacklistedWordsActive = true;
                }


                if (Database.UserSettingsHandler.GetBoolean("Spamcheck")) {
                    Client.OnMessageReceived += ChatChecker.CheckMessage;
                    IsSpamCheckActive = true;
                }

                if (!Convert.ToBoolean(Config.GeneralConfig.Config["SpamCheck"]["AllowViewerToPostLinks"])) {
                    Client.OnMessageReceived += ChatChecker.LinkChecker;
                    IsAllowViewerToPostLinksActive = true;
                }

                if (Convert.ToBoolean(Config.GeneralConfig.Config["SpamCheck"]["CapsRestriction"])) {
                    Client.OnMessageReceived += ChatChecker.CapsChecker;
                    IsCapsCheckActive = true;
                }

                return Client;
            }

            /// <summary>
            /// Blacklisted Words active / inactive
            /// </summary>
            /// <param name="SetActive"></param>
            public static void SetBlacklistedWords(bool SetActive) {
                if (SetActive) {
                    if (!IsBlacklistedWordsActive) {
                        AivaClient.Client.AivaTwitchClient.OnMessageReceived += ChatChecker.BlacklistWordsChecker;
                        IsBlacklistedWordsActive = true;
                    }
                } else {
                    AivaClient.Client.AivaTwitchClient.OnMessageReceived -= ChatChecker.BlacklistWordsChecker;
                    IsBlacklistedWordsActive = false;
                }
            }

            /// <summary>
            /// Set SpamCheck active / inactive
            /// </summary>
            /// <param name="SetActive"></param>
            public static void SetSpamCheck(bool SetActive) {
                if (SetActive) {
                    if (!IsSpamCheckActive) {
                        AivaClient.Client.AivaTwitchClient.OnMessageReceived += ChatChecker.CheckMessage;
                        IsSpamCheckActive = true;
                    }
                } else {
                    AivaClient.Client.AivaTwitchClient.OnMessageReceived += ChatChecker.CheckMessage;
                    IsSpamCheckActive = false;
                }
            }

            /// <summary>
            /// Set Allow Viewers to post Links active / inactive
            /// </summary>
            /// <param name="SetActive"></param>
            public static void SetAllowViewerToPostLinks(bool SetActive) {
                if (SetActive) {
                    if (!IsAllowViewerToPostLinksActive) {
                        AivaClient.Client.AivaTwitchClient.OnMessageReceived += ChatChecker.LinkChecker;
                        IsAllowViewerToPostLinksActive = true;
                    }
                } else {
                    AivaClient.Client.AivaTwitchClient.OnMessageReceived -= ChatChecker.LinkChecker;
                    IsAllowViewerToPostLinksActive = false;
                }

            }

            public static void SetCapsChecker(bool SetActive) {
                if (SetActive) {
                    if (!IsCapsCheckActive) {
                        AivaClient.Client.AivaTwitchClient.OnMessageReceived += ChatChecker.CapsChecker;
                        IsCapsCheckActive = true;
                    }
                } else {
                    AivaClient.Client.AivaTwitchClient.OnMessageReceived -= ChatChecker.CapsChecker;
                    IsCapsCheckActive = false;
                }
            }
        }

        /// <summary>
        /// Send Message when new Susbcriber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e) {
            if (!e.Subscriber.IsTwitchPrime) {
                if (Convert.ToBoolean(Config.GeneralConfig.Config["Interactions"]["WriteInChatNormalSub"])) {
                    AivaClient.Client.AivaTwitchClient.SendMessage(
                        Config.LanguageConfig.Instance.GetString("InteractionChatMessageNormalSub")
                        .Replace("@USERNAME@", e.Subscriber.Name));
                }
            } else {
                if (Convert.ToBoolean(Config.GeneralConfig.Config["Interactions"]["WriteInChatPrimeSub"])) {
                    AivaClient.Client.AivaTwitchClient.SendMessage(
                        Config.LanguageConfig.Instance.GetString("InteractionChatMessagePrimeSub")
                        .Replace("@USERNAME@", e.Subscriber.Name));
                }
            }
        }

        private static void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e) {
            // Check Currency
            if (String.Compare(e.Command.Command, Config.CurrencyConfig.Config["General"]["UserCommandCheckCurrency"], true) == 0)
                Commands.Currency.WriteCurrencyForUser(e.Command.ChatMessage.Username);

            // ModCommand
            var modCurrency = new Core.Commands.ModCommands.Currency(e.Command.Command, e.Command.ArgumentsAsList);
        }

        private static void Client_OnExistingUsersDetected(object sender, OnExistingUsersDetectedArgs e) {
            foreach (var user in e.Users) {
                Database.UserHandler.AddUser.AddUserToDatabase(
                        TwitchApi.Users.GetUserAsync(user).Result);


                Database.UserHandler.UpdateUser.UpdateLastSeenAsync(user);

                Database.UserHandler.UpdateUser.SetIsViewingAsync(user, true);
            }
        }

        private static void Client_OnUserLeft(object sender, OnUserLeftArgs e) {
            Database.UserHandler.UpdateUser.UpdateLastSeenAsync(e.Username);

            Database.UserHandler.UpdateUser.SetIsViewingAsync(e.Username, false);
        }

        private static void Client_OnUserJoined(object sender, OnUserJoinedArgs e) {
            Database.UserHandler.AddUser.AddUserToDatabase(
                    TwitchApi.Users.GetUser(e.Username));

            Database.UserHandler.UpdateUser.SetIsViewingAsync(e.Username, true);
        }

        public static async Task<List<TwitchLib.Models.API.Block.Block>> LoadBlockedUserAsync() {
            var blockedUser = await _getBansAsync();


            return blockedUser;
        }

        public static async Task<List<TwitchLib.Models.API.Block.Block>> LoadListAsync() {
            return await _getBansAsync();
        }

        private async static Task<List<TwitchLib.Models.API.Block.Block>> _getBansAsync() {
            var result = await TwitchLib.TwitchApi.Blocks.GetBlockedListAsync(Config.GeneralConfig.Config["General"]["BotName"]);

            return result;
        }

        public async static void BannUserAsync(string name) {
            await TwitchLib.TwitchApi.Blocks.BlockUserAsync(
                Config.GeneralConfig.Config["General"]["BotName"],
                name,
                Config.GeneralConfig.Config["Credentials"]["TwitchOAuth"]
                );
        }

        public static void UnbanUser(string name) {
            TwitchLib.TwitchApi.Blocks.UnblockUserAsync(
                Config.GeneralConfig.Config["General"]["BotName"],
                name,
                Config.GeneralConfig.Config["Credentials"]["TwitchOAuth"]);
        }

        public static async Task<TwitchLib.Models.API.User.User> GetUserAsync(string name) => await TwitchLib.TwitchApi.Users.GetUserAsync(name);
    }
}
