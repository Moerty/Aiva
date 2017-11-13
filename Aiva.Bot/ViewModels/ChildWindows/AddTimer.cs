using System;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels.ChildWindows {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AddTimer {
        #region Models

        public string Name { get; set; }
        public string Text { get; set; }
        public int Interval { get; set; } = 15;
        public int Lines { get; set; } = 1;

        public bool IsEditing { get; set; }
        public long DatabaseID { get; set; }

        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public event EventHandler CloseEvent;

        public bool IsCompleted;

        #endregion Models

        #region Constructor

        public AddTimer() {
            SetCommands();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Set the commands
        /// </summary>
        private void SetCommands() {
            SubmitCommand = new Internal.RelayCommand(submit => Submit());
            CancelCommand = new Internal.RelayCommand(cancel => Cancel());
        }

        /// <summary>
        /// Cancel Button
        /// </summary>
        private void Cancel() {
            CloseEvent(this, EventArgs.Empty);
        }

        /// <summary>
        /// Submit Button
        /// </summary>
        private void Submit() {
            IsCompleted = true;
            CloseEvent(this, EventArgs.Empty);
        }

        #endregion Methods
    }
}