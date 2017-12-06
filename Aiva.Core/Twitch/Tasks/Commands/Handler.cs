namespace Aiva.Core.Twitch.Tasks.Commands {
    public class Handler {
        #region Models
        public ModCommands.Handler ModCommands;
        public Currency CurrencyCommands;
        #endregion Models

        #region Constructor
        public Handler() {
            ModCommands = new Commands.ModCommands.Handler();
            CurrencyCommands = new Currency();
        }
        #endregion Constructor
    }
}