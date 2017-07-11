using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Giveaway {

        public Extensions.Giveaway.GiveawayHandler Handler { get; set; }

        public ICommand StartGiveawayCommand { get; set; }
        public ICommand GetWinnerCommand { get; set; }
        public ICommand ResetCommand { get; set; }



        public Giveaway() {
            SetCommands();
        }

        private void SetCommands() {
            Handler = new Extensions.Giveaway.GiveawayHandler();

            StartGiveawayCommand = new Internal.RelayCommand(g => StartGiveaway(), g => CanStartGiveaway());
        }

        private bool CanStartGiveaway() {

            return
                !String.IsNullOrEmpty(Handler.Command)
                && Handler.Price > 0;
        }

        private void StartGiveaway() {
            throw new NotImplementedException();
        }
    }
}
