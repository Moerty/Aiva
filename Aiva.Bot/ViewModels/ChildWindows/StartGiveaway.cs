using Aiva.Extensions.Models;
using System;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels.ChildWindows {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class StartGiveaway {

        #region Models

        public string Command { get; set; }
        public JoinPermission SelectedJoinPermission { get; set; } = JoinPermission.Everyone;
        public int Price { get; set; } = 100;
        public int MinutesActive { get; set; } = 3;
        public int SubLuck { get; set; } = 1;
        public bool BeFollower { get; set; } = true;
        public bool NotifyWinner { get; set; } = true;
        public bool RemoveWinnerFromList { get; set; } = true;
        public bool BlockReEntry { get; set; } = true;
        public bool IsSubluckActive { get; set; } = true;

        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public event EventHandler CloseEvent;

        public bool IsCompleted;

        #endregion Models

        #region Constructor
        public StartGiveaway() {
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