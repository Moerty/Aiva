using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Events.Client;

namespace Aiva.Core.Client.Internal {
    public class Commands {

        public ModCommands Mods;

        public Commands() {
            Mods = new ModCommands();
        }

        /// <summary>
        /// Class for ModCommands
        /// </summary>
        public class ModCommands {

            public Currency CurrencyHandler;

            public ModCommands() {
                CurrencyHandler = new Currency();
            }

            public void ParseModCommand(object sender, OnChatCommandReceivedArgs e) {
                if (e.Command.ChatMessage.UserType != TwitchLib.Enums.UserType.Viewer ||
                            e.Command.ChatMessage.UserType != TwitchLib.Enums.UserType.Staff) {

                    // Currency
                    if (String.Compare(e.Command.CommandText, Config.Config.Instance.Storage.ModCommands.ModCurrency.AddCurrency, true) == 0) {
                        CurrencyHandler.Add.AddCurrencyToUser(e);
                    }
                }
            }

            /// <summary>
            /// Stores Currency ModCommands
            /// </summary>
            public class Currency {

                public AddCurrency Add;
                public TransferCurrency Transfer;
                public RemoveCurrency Remove;

                public Currency() {
                    Add = new AddCurrency();
                    Transfer = new TransferCurrency();
                    Remove = new RemoveCurrency();
                }

                /// <summary>
                /// Add Currency ModCommands
                /// </summary>
                public class AddCurrency {

                    private DatabaseHandlers.Currency.AddCurrency _addCurrencyDatabaseHandler;

                    public AddCurrency() {
                        _addCurrencyDatabaseHandler = new DatabaseHandlers.Currency.AddCurrency();
                    }

                    public async void AddCurrencyToUser(OnChatCommandReceivedArgs e) {
                        if (e.Command.ChatMessage.UserType != TwitchLib.Enums.UserType.Viewer ||
                            e.Command.ChatMessage.UserType != TwitchLib.Enums.UserType.Staff) {

                            if (int.TryParse(e.Command.ArgumentsAsList[1], out int value)) {

                                var user = await AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(e.Command.ArgumentsAsList[0]);

                                if (user != null && user.Total > 0) {
                                    _addCurrencyDatabaseHandler.Add(user.Matches[0].Id, value);
                                    Core.AivaClient.Instance.AivaTwitchClient.SendMessage($"@{e.Command.ChatMessage.DisplayName} : {user.Matches[0].DisplayName} added {e.Command.ArgumentsAsList[1]} currency!");
                                }
                            }
                        }
                    }
                }

                /// <summary>
                /// Transfer Currency ModCommands
                /// </summary>
                public class TransferCurrency {

                }

                /// <summary>
                /// Remove Currency ModCommands
                /// </summary>
                public class RemoveCurrency {

                }
            }
        }
    }
}
