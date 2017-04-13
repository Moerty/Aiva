using System;
using System.Collections.Generic;
using Aiva.Core.Config;
using Aiva.Core.Database;

namespace Aiva.Extensions.ModCommands {
    public class Currency {
        private readonly string Command;
        private readonly List<string> Arguments;
        private readonly CurrencyControls Action;

        public Currency(string command, List<string> arguments) {
            Command = command;
            Arguments = arguments;
            Action = IdentifyCommand();


            DoModification();
        }

        private CurrencyControls IdentifyCommand() {
            if (Command.Equals(ModCommandsConfig.Config["Currency"]["AddCurrency"], StringComparison.OrdinalIgnoreCase))
                return CurrencyControls.Add;
            if (Command.Equals(ModCommandsConfig.Config["Currency"]["DeleteCurrency"], StringComparison.OrdinalIgnoreCase))
                return CurrencyControls.Del;
            if (Command.Equals(ModCommandsConfig.Config["Currency"]["TransferCurrency"], StringComparison.OrdinalIgnoreCase))
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
            var checkValue = CheckInt(Arguments[1]);

            if (Arguments.Count == 2) {
                if (checkValue.HasValue) {
                    CurrencyHandler.AddCurrencyAsync(Arguments[0], checkValue.Value);
                }
            }
        }

        private void TransferCurrency() {
            var checkValue = CheckInt(Arguments[2]);

            if (Arguments.Count == 3) {
                if (checkValue.HasValue) {
                    CurrencyHandler.TransferCurrencyAsync(Arguments[0], Arguments[1], checkValue.Value);
                }
            }
        }

        private void DelCurrency() {
            var checkValue = CheckInt(Arguments[1]);

            if (Arguments.Count == 2) {
                if (checkValue.HasValue) {
                    CurrencyHandler.RemoveCurrencyAsync(Arguments[0], checkValue.Value);
                }
            }
        }

        private int? CheckInt(string value) {
            return int.TryParse(value, out int result) ? result : (int?)null;
        }
    }

    public enum CurrencyControls {
        Add,
        Trans,
        Del,
        None
    }
}
