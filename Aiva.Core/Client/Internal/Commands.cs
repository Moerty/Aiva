using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Events.Client;

namespace Aiva.Core.Client.Internal {
    public class Commands {

        /// <summary>
        /// Class for ModCommands
        /// </summary>
        public class ModCommands {
            public static void ParseModCommand(object sender, OnChatCommandReceivedArgs e) {
                if (e.Command.ChatMessage.UserType != TwitchLib.Enums.UserType.Viewer ||
                            e.Command.ChatMessage.UserType != TwitchLib.Enums.UserType.Staff) {

                    // Currency
                    if (String.Compare(e.Command.Command, Config.Config.Instance["ModCommand"]["AddCurrency"], true) == 0) {
                        Currency.Add.AddCurrencyToUser(e);
                    }
                }
            }

            /// <summary>
            /// Stores Currency ModCommands
            /// </summary>
            public class Currency {

                /// <summary>
                /// Add Currency ModCommands
                /// </summary>
                public class Add {
                    public static void AddCurrencyToUser(OnChatCommandReceivedArgs e) {
                        if (e.Command.ChatMessage.UserType != TwitchLib.Enums.UserType.Viewer ||
                            e.Command.ChatMessage.UserType != TwitchLib.Enums.UserType.Staff) {

                            if (int.TryParse(e.Command.ArgumentsAsList[1], out int value)) {
                                Database.Currency.Add.AddCurrencyToUser(e.Command.ChatMessage.UserId, value);
                            }
                        }
                    }
                }

                /// <summary>
                /// Transfer Currency ModCommands
                /// </summary>
                public class Transfer {

                }

                /// <summary>
                /// Remove Currency ModCommands
                /// </summary>
                public class Remove {

                }
            }
        }
    }
}
