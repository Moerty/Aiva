using System;
using System.Windows.Input;
using Aiva.Bot.Views.ChildWindows.Voting;
using MahApps.Metro.Controls;
using System.Windows;
using MahApps.Metro.SimpleChildWindow;

namespace Aiva.Bot.ViewModels.ChildWindows.Voting {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Start {
        #region Models

        public Extensions.Models.Voting.Properties Properties { get; set; }

        public ICommand OptionsCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public event EventHandler CloseEvent;

        public bool IsCompleted;

        #endregion Models

        #region Constructor

        public Start() {
            Properties = new Extensions.Models.Voting.Properties();
            SetCommands();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Set the commands
        /// </summary>
        private void SetCommands() {
            SubmitCommand = new Internal.RelayCommand(submit => Submit(), submit => Properties.Options != null);
            CancelCommand = new Internal.RelayCommand(cancel => Cancel());
            OptionsCommand = new Internal.RelayCommand(options => ShowOptions());
        }

        /// <summary>
        /// Start giveaway form
        /// IMHO a giant hack against mvvm
        /// </summary>
        private async void ShowOptions() {
            var startOptionsWindow = new Views.ChildWindows.Voting.Options() { IsModal = true, AllowMove = true };
            ((Options)startOptionsWindow.DataContext).CloseEvent += (sender, EventArgs)
                => CloseOptionsWindow(startOptionsWindow);

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(startOptionsWindow).ConfigureAwait(false);
        }

        /// <summary>
        /// Fires when the start giveaway form closed
        /// </summary>
        /// <param name="startOptionsWindow"></param>
        private void CloseOptionsWindow(Views.ChildWindows.Voting.Options startOptionsWindow) {
            var windowInfo = Internal.SimpleChildWindow.GetDataContext<Views.ChildWindows.Voting.Options, ViewModels.ChildWindows.Voting.Options>
                (startOptionsWindow, startOptionsWindow.DataContext);

            if(windowInfo?.Item1 != null && windowInfo?.Item2 != null) {
                Properties.Options = windowInfo.Item2.OptionsModel;
                windowInfo.Item1.Close();
            }
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
