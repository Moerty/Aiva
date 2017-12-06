using Aiva.Gui.Interfaces;
using System;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.ChildWindows.Timers {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Add : IChildWindow {
        #region Models

        public string Name { get; set; }
        public string Text { get; set; }
        public int Interval { get; set; } = 15;
        public int Lines { get; set; } = 1;

        public bool IsEditing { get; set; }
        public int DatabaseID { get; set; }

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