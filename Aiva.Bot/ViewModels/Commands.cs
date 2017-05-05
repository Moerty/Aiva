using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class Commands {
        public Extensions.Models.Commands.AddModel AddModel { get; set; }
        public Extensions.Commands.CommandHandler Handler { get; set; }

        public List<string> UserRightsItems { get; private set; } = Enum.GetNames(typeof(Extensions.Models.Commands.UserRights)).ToList();


        public ICommand AddCommand { get; set; }
        public ICommand ResetAddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public Commands() {
            AddModel = new Extensions.Models.Commands.AddModel();
            Handler = new Extensions.Commands.CommandHandler();

            SetCommands();
        }

        /// <summary>
        /// Init Commands
        /// </summary>
        private void SetCommands() {
            AddCommand = new Internal.RelayCommand(add => AddCommandToList(), add => !String.IsNullOrEmpty(AddModel.Command) && !String.IsNullOrEmpty(AddModel.Text));
            ResetAddCommand = new Internal.RelayCommand(reset => AddModel = new Extensions.Models.Commands.AddModel(), add => true);
            DeleteCommand = new Internal.RelayCommand(d => Delete(), delete => Handler.SelectedCommand != null);
        }

        /// <summary>
        /// Remove the selected Command from list and Database
        /// </summary>
        private void Delete() => Handler.RemoveCommand();

        /// <summary>
        /// Add a Command to the List
        /// </summary>
        public void AddCommandToList() => Handler.AddCommandAsync(AddModel);
    }
}