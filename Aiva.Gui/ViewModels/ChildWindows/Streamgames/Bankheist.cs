using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Aiva.Core.Config;
using Aiva.Models.Extensions;

namespace Aiva.Gui.ViewModels.ChildWindows.Streamgames {
    [AddINotifyPropertyChangedInterface]
    public class Bankheist {
        public Aiva.Models.Streamgames.Bankheist.Properties Properties { get; set; }

        public Bankheist() {
            LoadPropertiesFromConfig();
        }

        private void LoadPropertiesFromConfig() {
            Properties = new Aiva.Models.Streamgames.Bankheist.Properties {
                Command = Config.Instance.Storage.StreamGames.Bankheist.General.Command,
                BankheistCooldown = (int)Config.Instance.Storage.StreamGames.Bankheist.Cooldowns.BankheistCooldown,
                BankheistDuration = (int)Config.Instance.Storage.StreamGames.Bankheist.Cooldowns.BankheistDuration,

                MinUserBank1 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank1.MinimumPlayers,
                SuccessRateBank1 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank1.SuccessRate,
                WinningMultiplierBank1 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank1.WinningMultiplier,

                MinUserBank2 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank2.MinimumPlayers,
                SuccessRateBank2 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank2.SuccessRate,
                WinningMultiplierBank2 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank2.WinningMultiplier,

                MinUserBank3 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank3.MinimumPlayers,
                SuccessRateBank3 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank3.SuccessRate,
                WinningMultiplierBank3 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank3.WinningMultiplier,

                MinUserBank4 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank4.MinimumPlayers,
                SuccessRateBank4 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank4.SuccessRate,
                WinningMultiplierBank4 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank4.WinningMultiplier,

                MinUserBank5 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank5.MinimumPlayers,
                SuccessRateBank5 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank5.SuccessRate,
                WinningMultiplierBank5 = Config.Instance.Storage.StreamGames.Bankheist.Settings.Bank5.WinningMultiplier,
            };

        }
    }
}
