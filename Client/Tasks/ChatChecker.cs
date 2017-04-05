using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using TwitchLib.Enums;
using TwitchLib.Events.Client;
using TwitchLib.Extensions.Client;

namespace Client.Tasks
{
    public static class ChatChecker
    {
        private static readonly System.Resources.ResourceManager languageConfig = Config.Language.Instance;

        public static void CheckMessage(object sender, OnMessageReceivedArgs e)
        {
            Checks check;
            switch (e.ChatMessage.UserType)
            {
                case UserType.Admin:
                    check = new Checks(e, Convert.ToBoolean(Config.General.Config["SpamCheck"]["SkipMessageCheckAdmin"]));
                    break;
                case UserType.Broadcaster:
                    check = new Checks(e, Convert.ToBoolean(Config.General.Config["SpamCheck"]["SkipMessageCheckBroadcaster"]));
                    break;
                case UserType.GlobalModerator:
                    check = new Checks(e, Convert.ToBoolean(Config.General.Config["SpamCheck"]["SkipMessageCheckGlobalMod"]));
                    break;
                case UserType.Moderator:
                    check = new Checks(e, Convert.ToBoolean(Config.General.Config["SpamCheck"]["SkipMessageCheckMod"]));
                    break;
                case UserType.Staff:
                    check = new Checks(e, Convert.ToBoolean(Config.General.Config["SpamCheck"]["SkipMessageCheckStaff"]));
                    break;
                case UserType.Viewer:
                    check = new Checks(e, Convert.ToBoolean(Config.General.Config["SpamCheck"]["SkipMessageCheckViewer"]));
                    break;
            }
        }

        private class Checks
        {
            /// <summary>
            /// </summary>
            /// <param name="e"></param>
            /// <param name="skipChecks">Skip Checks for Mods</param>
            public Checks(OnMessageReceivedArgs e, bool skipChecks = false)
            {
                if (skipChecks)
                    return;
                SpamSchutz.addToSpamSchutz(e);
            }

            public class SpamSchutz
            {
                private static Dictionary<DateTime, OnMessageReceivedArgs> nachrichten;

                /// <summary>
                ///     Dictionary to store Messages and check if a User spam
                /// </summary>
                /// <param name="e"></param>
                public static void addToSpamSchutz(OnMessageReceivedArgs e)
                {
                    var eingangszeit = DateTime.Now;

                    if (nachrichten == null)
                        nachrichten = new Dictionary<DateTime, OnMessageReceivedArgs>();

                    /*
					 * Spamschutz
					 * */

                    // Delete
                    deleteFromList();

                    // Check
                    var checkSpamResult = checkIfSpam(e.ChatMessage.Username, eingangszeit);

                    // Add
                    if (!checkSpamResult)
                        nachrichten.Add(DateTime.Now, e);
                }

                /// <summary>
                ///     Delete Message when older than X
                /// </summary>
                private static void deleteFromList()
                {
                    var result = nachrichten.Where(n => n.Key <= DateTime.Now
                                                            .Subtract(TimeSpan.Parse(Config.General.Config["SpamCheck"]["TimeToNewMessage"])))
                        .ToList();

                    foreach (var items in result)
                        nachrichten.Remove(items.Key);
                }

                /// <summary>
                ///     Check Message frequenz
                /// </summary>
                /// <param name="username"></param>
                /// <param name="jetztzeit"></param>
                /// <returns></returns>
                private static bool checkIfSpam(string username, DateTime jetztzeit)
                {
                    var result = nachrichten.Where(n => n.Value.ChatMessage.Username == username).ToList();
                    var timeoutTime = TimeSpan.Parse(Config.General.Config["SpamCheck"]["TimeoutTime"]);
                    var warningTimeout = TimeSpan.Parse(Config.General.Config["SpamCheck"]["MinutesTimeoutWarning"]);

                    if (result.Count == 0)
                        return false;
                    switch (Warnings.CheckWarnings(username))
                    {
                        case 1:
                            Client.ClientBBB.TwitchClientBBB.TimeoutUser(result[0].Value.ChatMessage.Username, timeoutTime,
                                languageConfig.GetString("SpamCheckFirstWarningText")
                                    .Replace("@TIMEOUTTIME@", timeoutTime.TotalSeconds.ToString()));

                            return true;
                        case 2:
                            Client.ClientBBB.TwitchClientBBB.TimeoutUser(result[0].Value.ChatMessage.Username, timeoutTime,
                                languageConfig.GetString("SpamCheckFirstWarningText")
                                    .Replace("@TIMEOUTTIME@", timeoutTime.TotalSeconds.ToString()));
                            return true;
                        case 3:
                            Client.ClientBBB.TwitchClientBBB.TimeoutUser(result[0].Value.ChatMessage.Username, timeoutTime,
                                languageConfig.GetString("SpamCheckThirdWarningText")
                                    .Replace("@TIMEOUTTIME@", timeoutTime.TotalSeconds.ToString()));
                            return true;
                        default:
                            Client.ClientBBB.TwitchClientBBB.TimeoutUser(result[0].Value.ChatMessage.Username, timeoutTime,
                                languageConfig.GetString("SpamCheckBanWarningText")
                                    .Replace("@TIMEOUTTIME@", timeoutTime.TotalSeconds.ToString()));
                            return true;
                    }
                }


                private class Warnings
                {
                    private static List<Warnings> warning;
                    private int _sumWarning;
                    private readonly DateTime _time;

                    private readonly string _username;

                    public Warnings(string username, DateTime time, int warnings)
                    {
                        _username = username;
                        _time = time;
                        _sumWarning = warnings;
                    }

                    /// <summary>
                    ///     Check if a username is in the warning list
                    ///     count 3 times
                    ///     if not exists -> add
                    /// </summary>
                    /// <param name="username"></param>
                    /// <returns>Number 0 means he spams </returns>
                    public static int CheckWarnings(string username)
                    {
                        // Create
                        if (warning == null)
                            warning = new List<Warnings>();

                        // Delete Entry

                        var deleteWarnings = warning.Where(d => d._time <= DateTime.Now
                                                                    .Subtract(
                                                                        TimeSpan.Parse(Config.General.Config["SpamCheck"]["TimeActiveWarning"])))
                            .ToList();

                        foreach (var del in deleteWarnings)
                            warning.Remove(del);

                        // Get Entry
                        var result = warning.FirstOrDefault(w => w._username == username);
                        warning.Remove(result);

                        // count or add warning
                        if (result != null)
                            if (result._sumWarning <= 3)
                            {
                                result._sumWarning += 1;
                                warning.Add(result);
                                return result._sumWarning;
                            }
                            else
                            {
                                return 0;
                            }
                        warning.Add(new Warnings(username, DateTime.Now, 1));
                        return 1;
                    }
                }
            }
        }

        /// <summary>
        /// Check if User have written a BlacklistedWord and timeout him
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void BlacklistWordsChecker(object sender, OnMessageReceivedArgs e) {
            BlacklistedWords.ForEach(x => {
                if (e.ChatMessage.Message.Contains(x)) {
                    Client.ClientBBB.TwitchClientBBB.TimeoutUser(e.ChatMessage.Username, new TimeSpan(0, 5, 0), "Du wurdest getimeoutet wegen Benutzung eines bösen Wortes!");
                }
            });
        }
        public static List<string> BlacklistedWords { get; set; } = Database.UserSettingsHandler.GetBlacklistedWords();

        /*public class Commands
		{
            private CommandChecker commandChecker;
            public ReturnModel.returnModel CommandType;
            private OnChatCommandReceivedArgs args;

            public Commands(OnChatCommandReceivedArgs e)
			{
                commandChecker = new CommandChecker(Settings.Default.Language);
                CommandType = commandChecker.CheckCommand(e.Command.Command);
                args = e;
			}

			public void StartCommand()
			{
				switch (CommandType)
				{
					case ReturnModel.returnModel.Bankheist:
						BankheistHandler.ProcessBankheist(args);
						break;
					case ReturnModel.returnModel.Giveaway:

						var giveaway = new GiveawayHandler(args);
						giveaway.AddUser();
						break;
					default:

						break;
				}
			}
		} */
    }
}