using Aiva.Gui.Interfaces;
using System;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.ChildWindows.Voting {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Start : IChildWindow {
        #region Models

        public Aiva.Models.Voting.Properties Properties { get; set; }

        public ICommand SubmitCommand { get; set; }

        public event EventHandler CloseEvent;

        private bool _isCompleted;

        #endregion Models

        #region Constructor

        public Start() {
            Properties = new Aiva.Models.Voting.Properties();
            SetCommands();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Set the commands
        /// </summary>
        private void SetCommands() {
            SubmitCommand = new Internal.RelayCommand(submit => Submit(), submit => CanSubmit());
        }

        /// <summary>
        /// Checks if user can submit
        /// checks user options
        /// </summary>
        /// <returns></returns>
        private bool CanSubmit() {
            if (Properties?.Options?.Option1.ActiveOption == true) {
                return true;
            } else if (Properties?.Options?.Option2.ActiveOption == true) {
                return true;
            } else if (Properties?.Options?.Option3.ActiveOption == true) {
                return true;
            } else if (Properties?.Options?.Option4.ActiveOption == true) {
                return true;
            } else if (Properties?.Options?.Option5.ActiveOption == true) {
                return true;
            } else if (Properties?.Options?.Option6.ActiveOption == true) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Submit Button
        /// </summary>
        private void Submit() {
            _isCompleted = true;
            CloseEvent(this, EventArgs.Empty);
        }

        public bool IsCompleted() {
            return _isCompleted;
        }

        #endregion Methods
    }
}