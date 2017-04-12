using System;
using System.Collections.Generic;

namespace ModCommands {
    public class Currency {
        private string Command;
        private List<string> Arguments;
        private CurrencyControls Action;

        public Currency(string command, List<string> arguments) {
            Command = command;
            Arguments = arguments;
            Action = IdentifyCommand();


            DoModification();
        }

        private CurrencyControls IdentifyCommand() {
            if (Command.Equals(Config.ModCommands.Config["Currency"]["AddCurrency"], StringComparison.OrdinalIgnoreCase))
                return CurrencyControls.Add;
            if (Command.Equals(Config.ModCommands.Config["Currency"]["DeleteCurrency"], StringComparison.OrdinalIgnoreCase))
                return CurrencyControls.Del;
            if (Command.Equals(Config.ModCommands.Config["Currency"]["TransferCurrency"], StringComparison.OrdinalIgnoreCase))
                return CurrencyControls.Trans;

            return CurrencyControls.None;
        }

        private void DoModification() {
            switch (Action) {
                case CurrencyControls.Add:
                    AddCurrency();
                    break;
                case CurrencyControls.Trans:
                    TransferCurrency();
                    break;
                case CurrencyControls.Del:
                    DelCurrency();
                    break;
                default:
                    break;
            }
        }

        private void AddCurrency() {
            int? checkValue = CheckInt(Arguments[1]);

            if (Arguments.Count == 2) {
                if (checkValue.HasValue) {
                    Database.CurrencyHandler.AddCurrencyAsync(Arguments[0], checkValue.Value);
                }
                // Value incorrect (maybe characters)
                else {

                }
            }
        }

        private void TransferCurrency() {
            int? checkValue = CheckInt(Arguments[2]);

            if (Arguments.Count == 3) {
                if (checkValue.HasValue) {
                    Database.CurrencyHandler.TransferCurrencyAsync(Arguments[0], Arguments[1], checkValue.Value);
                }
            }
        }

        private void DelCurrency() {
            int? checkValue = CheckInt(Arguments[1]);

            if (Arguments.Count == 2) {
                if (checkValue.HasValue) {
                    Database.CurrencyHandler.RemoveCurrencyAsync(Arguments[0], checkValue.Value);
                }
            }
        }

        private int? CheckInt(string value) {
            int result;
            if (int.TryParse(value, out result))
                return result;
            else
                return null;
        }
    }

    public enum CurrencyControls {
        Add,
        Trans,
        Del,
        None
    }
}
