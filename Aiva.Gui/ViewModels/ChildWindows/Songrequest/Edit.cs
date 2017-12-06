using Aiva.Gui.Interfaces;
using System;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.ChildWindows.Songrequest {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Edit : IChildWindow {
        #region Models

        public Aiva.Models.Songrequest.Properties Properties { get; set; }

        public ICommand SubmitCommand { get; set; }

        public event EventHandler CloseEvent;

        private bool _isCompleted;

        #endregion Models

        #region Constructor

        public Edit() {
            Properties = new Aiva.Models.Songrequest.Properties();
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
            CloseEvent(this, EventArgs.Empty);
        }

        public bool IsCompleted() {
            return _isCompleted;
        }

        #endregion Methods
    }
}