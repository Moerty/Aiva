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
            CurrencyModel = new Models.CurrencyModel();
            CurrencyDatabaseList = CurrencyHandler.GetCurrencyList();
            CurrencyModel.UserList = new Models.AsyncObservableCollection<Currency>();

            CurrencyModel.AddCurrencyOnOff = Boolean.Parse(GeneralConfig.Config[nameof(Currency)]["Active"]);

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
                    CurrencyModel.BankheistTileColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CC53F324"));
                    CurrencyModel.BankheistText = LanguageConfig.Instance.GetString("BankheistGUITileRunning");
                    break;
                case BankheistModel.Enums.BankheistStatus.OnCooldown:
                    CurrencyModel.BankheistTileColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CC1E570E"));
                    CurrencyModel.BankheistText = LanguageConfig.Instance.GetString("BankheistGUITileCooldown");
                    break;
                case BankheistModel.Enums.BankheistStatus.Ready:
                    CurrencyModel.BankheistTileColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CCF32424"));
                    CurrencyModel.BankheistText = LanguageConfig.Instance.GetString("BankheistGUITileOffline");
                    break;
            }
        }

        #region AddCurrency
        /// <summary>
        /// Currency Timer
        /// </summary>
        private static void CurrencyTimer() {
            if (Convert.ToBoolean(GeneralConfig.Config[nameof(Currency)]["Active"])) {
                if (TimeSpan.TryParse(GeneralConfig.Config[nameof(Currency)]["TimerAddCurrency"], out TimeSpan Interval)) {
                    var timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Tick += CurrencyHandler.AddCurrencyFrequentlyAsync;
                    timer.Interval = Interval;
                    timer.Start();
                }
            }
        }
        #endregion
        public Models.CurrencyModel CurrencyModel { get; set; }
    }
}
