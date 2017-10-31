using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Core.Client.Internal {
    public class Currency {

        /// <summary>
        /// Add Currency frequently class
        /// </summary>
        public System.Timers.Timer Timer { get; private set; }

        private Core.DatabaseHandlers.Currency.AddCurrency _addCurrencyDatabaseHandler;

        public Currency() {
            if (TimeSpan.TryParse(Config.Config.Instance[nameof(Currency)]["TimerAddCurrencyFrequently"], out TimeSpan Interval)) {
                Timer = new System.Timers.Timer();
                Timer.Elapsed += CurrencyTimerTick;
                Timer.Interval = Interval.TotalMilliseconds;
                Timer.AutoReset = true;
                Timer.Start();
            }

            _addCurrencyDatabaseHandler = new DatabaseHandlers.Currency.AddCurrency();
        }

        /// <summary>
        /// Timer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrencyTimerTick(object sender, System.Timers.ElapsedEventArgs e) 
                    => _addCurrencyDatabaseHandler.AddCurrencyActiveViewer();
    }
}
