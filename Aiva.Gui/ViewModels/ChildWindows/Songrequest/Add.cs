using Aiva.Gui.Interfaces;
using System;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.ChildWindows.Songrequest {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Add : IChildWindow {
        #region Models

        public string Video { get; set; }
        public bool InstantStart { get; set; }

        public ICommand SubmitCommand { get; set; }

        public event EventHandler CloseEvent;

        private bool _isCompleted;

        #endregion Models

        #region Constructor

        public Add() {
            SetCommands();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Set the commands
        /// </summary>
        private void SetCommands() {
            SubmitCommand = new Internal.RelayCommand(submit => Submit(), submit => !string.IsNullOrEmpty(Video));
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