using System;
using System.Collections.Generic;
using System.Windows.Media;
using BlackBoxBot.Bankheist.Models;

namespace BlackBoxBot.ViewModels {
    class CurrencyViewModel
    {
        public List<Database.Currency> CurrencyDatabaseList;

        private readonly System.Windows.Forms.Timer bankheistTimer;

        public string BankheistCommand
        {
            get
            {
                return Config.Bankheist.Config["General"]["Command"];
            }
            set
            {
                Config.Bankheist.Config["General"]["Command"] = value;
                Config.Bankheist.WriteConfig();
            }
        }

        public bool IsBankheistEnabled
        {
            get
            {
                return Convert.ToBoolean(Config.Bankheist.Config["General"]["Active"]);
            }
            set
            {
                Config.Bankheist.Config["General"]["Active"] = value.ToString();
                Config.Bankheist.WriteConfig();
            }
        }

        public CurrencyViewModel()
        {
            currencyModel = new Models.CurrencyModel();
            CurrencyDatabaseList = Database.CurrencyHandler.GetCurrencyList();
            currencyModel.UserList = new Models.AsyncObservableCollection<Database.Currency>();
            currencyModel.UserList.Add(
                new Database.Currency
                {
                    Name = "abcdefg",
                    Value = 123,
                });

            currencyModel.AddCurrencyOnOff = Boolean.Parse(Config.General.Config["Currency"]["Active"]);

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
        private void BankheistTileActiveCheck(object sender, EventArgs e)
        {
            switch(Bankheist.Bankheist.Status)//Extensions.Bankheist.Status)
            {
                case BankheistModel.Enums.BankheistStatus.IsActive:
                    currencyModel.BankheistTileColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CC53F324"));
                    currencyModel.BankheistText = Config.Language.Instance.GetString("BankheistGUITileRunning");
                    break;
                case BankheistModel.Enums.BankheistStatus.OnCooldown:
                    currencyModel.BankheistTileColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CC1E570E"));
                    currencyModel.BankheistText = Config.Language.Instance.GetString("BankheistGUITileCooldown"); 
                    break;
                case BankheistModel.Enums.BankheistStatus.Ready:
                    currencyModel.BankheistTileColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CCF32424"));
                    currencyModel.BankheistText = Config.Language.Instance.GetString("BankheistGUITileOffline");
                    break;
            }
        }

        #region AddCurrency
        /// <summary>
        /// Currency Timer
        /// </summary>
        private static void CurrencyTimer()
        {
            var generalConfig = new Config.General();

            if (Convert.ToBoolean(Config.General.Config["Currency"]["Active"]))
            {
                TimeSpan Interval;
                if(TimeSpan.TryParse(Config.General.Config["Currency"]["TimerAddCurrency"], out Interval)) {
                    var timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Tick += Database.CurrencyHandler.AddCurrencyFrequentlyAsync;
                    timer.Interval = Interval;
                    timer.Start();
                }

                
            }
        }
        #endregion
        public Models.CurrencyModel currencyModel { get; set; }
    }
}
