using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System.ComponentModel;

namespace Aiva.Bot.ViewModels {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Timers {
        #region Models
        public Extensions.Models.Timers.AddModel AddModel { get; set; }
        public Extensions.Timers.TimersHandler Handler { get; set; }

        public List<string> UserRightsItems { get; private set; } = Enum.GetNames(typeof(Extensions.Models.Commands.UserRights)).ToList();

        public ICommand AddCommand { get; set; }
        public ICommand ResetAddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }



        #endregion Models

        #region Constructor
        public Timers() {
            AddModel = new Extensions.Models.Timers.AddModel();
            Handler = new Extensions.Timers.TimersHandler();

            SetCommands();
        }

        #endregion Constructor

        #region Commands
        /// <summary>
        /// Init Commands
        /// </summary>
        private void SetCommands() {
            AddCommand = new Internal.RelayCommand(add => AddCommandToList(), add => !String.IsNullOrEmpty(AddModel.Timer) && !String.IsNullOrEmpty(AddModel.Text));
            ResetAddCommand = new Internal.RelayCommand(reset => AddModel = new Extensions.Models.Timers.AddModel(), add => true);
            DeleteCommand = new Internal.RelayCommand(d => Delete(), delete => Handler.SelectedTimer != null);
        }

        /// <summary>
        /// Remove the selected Command from list and Database
        /// </summary>
        private void Delete() => Handler.RemoveTimer();

        /// <summary>
        /// Add a Command to the List
        /// </summary>
        public void AddCommandToList() => Handler.AddTimerAsync(AddModel);

        #endregion Commands
    }
}