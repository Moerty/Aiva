using Aiva.Bot.Views.ChildWindows;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Giveaway {
        #region Models

        public Extensions.Giveaway.GiveawayHandler Handler { get; set; }

        public ICommand StartGiveawayCommand { get; set; }
        public ICommand StopGiveawayCommand { get; set; }
        public ICommand GetWinnerCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        #endregion Models

        #region Constructor

        public Giveaway() {
            SetCommands();
        }

        private void SetCommands() {
            StartGiveawayCommand = new Internal.RelayCommand(g => StartGiveawaySetup(), g => Handler == null);
            StopGiveawayCommand = new Internal.RelayCommand(g => StopGiveaway(), g => CanStopGiveaway());
            GetWinnerCommand = new Internal.RelayCommand(g => Handler.GetWinner(), g => CanGetWinner());
            ResetCommand = new Internal.RelayCommand(g => Reset());
        }

        #endregion Constructor

        #region Functions

        /// <summary>
        /// Resets the giveaway
        /// </summary>
        private void Reset() {
            Handler?.StopGiveaway();
            Handler = null;
        }

        /// <summary>
        /// Checks if a winner get selected
        /// </summary>
        /// <returns></returns>
        private bool CanGetWinner() {
            if (Handler != null) {
                if (Handler.JoinedUsers.Any()) {
                    return true;
                }
            }

            return false;
        }

        private bool CanStopGiveaway() {
            if (Handler != null) {
                if (Handler.IsStarted) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Stop the giveaway
        /// prevent joining
        /// </summary>
        private void StopGiveaway() => Handler.StopRegistration();

        /// <summary>
        /// Start giveaway form
        /// IMHO a giant hack against mvvm
        /// </summary>
        private async void StartGiveawaySetup() {
            var startTimerWindow = new Views.ChildWindows.StartGiveaway() { IsModal = true, AllowMove = true };
            ((ViewModels.ChildWindows.StartGiveaway)startTimerWindow.DataContext).CloseEvent += (sender, EventArgs) => CloseStartWindow(startTimerWindow);

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(startTimerWindow).ConfigureAwait(false);
        }

        /// <summary>
        /// Fires when the start giveaway form closed
        /// </summary>
        /// <param name="startTimerWindow"></param>
        private void CloseStartWindow(StartGiveaway startTimerWindow) {
            Views.ChildWindows.StartGiveaway window;
            ChildWindows.StartGiveaway dataContext;

            if ((window = startTimerWindow as Views.ChildWindows.StartGiveaway) != null) {
                if ((dataContext = window.DataContext as ChildWindows.StartGiveaway) != null) {
                    if (dataContext.IsCompleted) {
                        StartGiveaway(dataContext);
                    }
                }

                window.Close();
            }
        }

        /// <summary>
        /// Creates the handler
        /// </summary>
        /// <param name="data"></param>
        private void StartGiveaway(ChildWindows.StartGiveaway data) {
            Handler = new Extensions.Giveaway.GiveawayHandler() {
                Properties = new Extensions.Models.Giveaway.Properties {
                    BeFollower = data.BeFollower,
                    BlockReEntry = data.BlockReEntry,
                    Command = data.Command,
                    SubLuck = data.IsSubluckActive ? data.SubLuck : 1,
                    IsSubLuckActive = data.IsSubluckActive,
                    Price = data.Price,
                    JoinPermission = data.SelectedJoinPermission
                }
            };

            Handler.StartGiveaway();
        }

        #endregion Functions
    }
}