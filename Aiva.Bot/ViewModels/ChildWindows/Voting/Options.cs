using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels.ChildWindows.Voting {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Options {
        public Extensions.Models.Voting.Options OptionsModel { get; set; }

        public ICommand SubmitCommand { get; set; }

        public event EventHandler CloseEvent;

        public Options() {
            OptionsModel = new Extensions.Models.Voting.Options();
            SubmitCommand = new Internal.RelayCommand(submit => Submit());
        }

        /// <summary>
        /// Submit Button
        /// </summary>
        private void Submit() {
            CloseEvent(this, EventArgs.Empty);
        }
    }
}
