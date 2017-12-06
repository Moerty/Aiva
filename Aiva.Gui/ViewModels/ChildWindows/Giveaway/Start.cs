using Aiva.Gui.Interfaces;
using System;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.ChildWindows.Giveaway {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Start : IChildWindow {
        #region Models
        public ICommand SubmitCommand { get; set; }

        public Aiva.Models.Giveaway.Properties Properties { get; set; }

        public event EventHandler CloseEvent;

        private bool _isCompleted;

        #endregion Models

        #region Constructor

        public Start() {
            Properties = new Aiva.Models.Giveaway.Properties();
            SetCommands();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Set the commands
        /// </summary>
        private void SetCommands() {
            SubmitCommand = new Internal.RelayCommand(submit => Submit());
        }

        /// <summary>
        /// Submit Button
        /// </summary>
        private void Submit() {
            _isCompleted = true;
            CloseEvent.Invoke(this, EventArgs.Empty);
        }

        public bool IsCompleted() {
            return _isCompleted;
        }

        #endregion Methods
    }
}