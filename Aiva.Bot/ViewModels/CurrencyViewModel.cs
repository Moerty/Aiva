using System;
using System.Collections.Generic;
using System.Windows.Media;
using Aiva.Core.Database;
using Aiva.Core.Storage;
using Aiva.Core.Config;
using Aiva.Extensions.Bankheist;
using Aiva.Extensions.Models.Bankheist;

namespace Aiva.Bot.ViewModels {
    class CurrencyViewModel {
        public List<Currency> CurrencyDatabaseList;

        private readonly System.Windows.Forms.Timer bankheistTimer;

        public CurrencyViewModel() {
            currencyModel = new Models.CurrencyModel();
            CurrencyDatabaseList = CurrencyHandler.GetCurrencyList();
            currencyModel.UserList = new Models.AsyncObservableCollection<Currency>();

            currencyModel.AddCurrencyOnOff = Boolean.Parse(GeneralConfig.Config["Currency"]["Active"]);

            bankheistTimer = new System.Windows.Forms.Timer();
            bankheistTimer.Tick += BankheistTileActiveCheck;
            bankheistTimer.Interval = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
            bankheistTimer.Start();

            // AddCurrency
            CurrencyTimer();
        }

        /// <summary>
        /// Change the color from tile if Bankheist is running or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BankheistTileActiveCheck(object sender, EventArgs e) {
            switch (Aiva.Extensions.Bankheist.Bankheist.Status)//Extensions.Bankheist.Status)
            {
                case BankheistModel.Enums.BankheistStatus.IsActive:
                    currencyModel.BankheistTileColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CC53F324"));
                    currencyModel.BankheistText = LanguageConfig.Instance.GetString("BankheistGUITileRunning");
                    break;
                case BankheistModel.Enums.BankheistStatus.OnCooldown:
                    currencyModel.BankheistTileColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CC1E570E"));
                    currencyModel.BankheistText = LanguageConfig.Instance.GetString("BankheistGUITileCooldown");
                    break;
                case BankheistModel.Enums.BankheistStatus.Ready:
                    currencyModel.BankheistTileColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CCF32424"));
                    currencyModel.BankheistText = LanguageConfig.Instance.GetString("BankheistGUITileOffline");
                    break;
            }
        }

        #region AddCurrency
        /// <summary>
        /// Currency Timer
        /// </summary>
        private static void CurrencyTimer() {
            //var generalConfig = new Config.General();

            if (Convert.ToBoolean(GeneralConfig.Config["Currency"]["Active"])) {
                TimeSpan Interval;
                if (TimeSpan.TryParse(GeneralConfig.Config["Currency"]["TimerAddCurrency"], out Interval)) {
                    var timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Tick += CurrencyHandler.AddCurrencyFrequentlyAsync;
                    timer.Interval = Interval;
                    timer.Start();
                }
            }
        }
        #endregion
        public Models.CurrencyModel currencyModel { get; set; }
    }
}
