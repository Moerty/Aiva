using System;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels.ChildWindows.Songrequest {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class StartSongrequest {

        #region Models

        public Extensions.Models.Songrequest.AddModel AddModel { get; set; }

        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public event EventHandler CloseEvent;

        public bool IsCompleted;

        #endregion Models

        #region Constructor

        public StartSongrequest() {
            AddModel = new Extensions.Models.Songrequest.AddModel();
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