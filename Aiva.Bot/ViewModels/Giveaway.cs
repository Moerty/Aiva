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
        public ICommand StopGiveawayCommand { get; set; }
        public ICommand GetWinnerCommand { get; set; }
        public ICommand ResetCommand { get; set; }



        public Giveaway() {
            Handler = new Extensions.Giveaway.GiveawayHandler();
            SetCommands();
        }

        private void SetCommands() {
            StartGiveawayCommand = new Internal.RelayCommand(g => StartGiveaway(), g => CanStartGiveaway());
            StopGiveawayCommand = new Internal.RelayCommand(g => StopGiveaway(), g => CanStopGiveaway());
            GetWinnerCommand = new Internal.RelayCommand(g => GetWinner(), g => CanGetWinner());
            ResetCommand = new Internal.RelayCommand(g => Reset());
        }

        private void Reset() {
            Handler.Reset();
        }

        private bool CanGetWinner() {
            if (Handler != null) {
                if (Handler.JoinedUsers.Any()) {
                    return true;
                }
            }

            return false;
        }

        private void GetWinner() {
            throw new NotImplementedException();
        }

        private bool CanStopGiveaway() {
            if (Handler != null) {
                if (Handler.IsStarted) {
                    return true;
                }
            }

            return false;
        }

        private void StopGiveaway() {
            Handler.StopGiveaway();
        }

        private bool CanStartGiveaway() {

            return
                !String.IsNullOrEmpty(Handler.Command)
                && Handler.Price > 0;
        }

        private void StartGiveaway() {
            Handler.StartGiveaway();
        }
    }
}
