using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Timers {
        #region Models

        public ICommand AddTimerCommand { get; set; }
        public ICommand EditTimerCommand { get; set; }
        public ICommand RemoveTimerCommand { get; set; }

        public Extensions.Timers.Handler Handler { get; set; }

        #endregion Models

        #region Constructor

        public Timers() {
            Handler = new Extensions.Timers.Handler();
            SetCommands();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Set commands
        /// </summary>
        private void SetCommands() {
            AddTimerCommand = new Internal.RelayCommand(async add => await ShowAddWindow().ConfigureAwait(false));
            EditTimerCommand = new Internal.RelayCommand(edit => EditTimer(), edit => Handler.SelectedTimer != null);
            RemoveTimerCommand = new Internal.RelayCommand(remove => RemoveTimer(), remove => Handler.SelectedTimer != null);
        }

        private void RemoveTimer() => Handler.RemoveTimer();

        /// <summary>
        /// Show the child window
        /// IMHO a giant hack against mvvm
        /// </summary>
        private async void EditTimer() {
            var addTimerWindow = new Views.ChildWindows.AddTimer(Handler.SelectedTimer.Name, Handler.SelectedTimer.Text, (int)Handler.SelectedTimer.Interval, Handler.SelectedTimer.ID);
            ((ViewModels.ChildWindows.AddTimer)addTimerWindow.DataContext).CloseEvent += (sender, EventArgs) => ClosingAddWindow(addTimerWindow);

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(addTimerWindow).ConfigureAwait(false);
        }

        /// <summary>
        /// Show that add child window
        /// IMHO a giant hack against mvvm
        /// </summary>
        /// <returns></returns>
        private async Task ShowAddWindow() {
            var addTimerWindow = new Views.ChildWindows.AddTimer() { IsModal = true, AllowMove = true };
            ((ViewModels.ChildWindows.AddTimer)addTimerWindow.DataContext).CloseEvent += (sender, EventArgs) => ClosingAddWindow(addTimerWindow);
            //addTimerWindow.Closing += (sender, CancelEventArgs) => ClosingAddWindow(addTimerWindow.DataContext);

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(addTimerWindow).ConfigureAwait(false);
        }

        /// <summary>
        /// Fires when add child window close
        /// </summary>
        /// <param name="rawWindow"></param>
        private void ClosingAddWindow(Views.ChildWindows.AddTimer rawWindow) {
            var dataContext = Internal.SimpleChildWindow.GetDataContext<Views.ChildWindows.AddTimer, ViewModels.ChildWindows.AddTimer>
                (rawWindow, rawWindow.DataContext);

            if(dataContext?.Item1 != null && dataContext?.Item2 != null) {
                if(!dataContext.Item2.IsCompleted) {
                    if (!dataContext.Item2.IsEditing) {
                        var result = Handler.AddTimerToDatabase(dataContext.Item2.Name, dataContext.Item2.Text, dataContext.Item2.Interval);
                        if (result) {
                            ShowConfirmWindow();
                        }
                    } else {
                        Handler.EditTimer(dataContext.Item2.Name, dataContext.Item2.Text, dataContext.Item2.Interval, dataContext.Item2.DatabaseID);
                    }
                }

                dataContext.Item1.Close();
            }
        }

        /// <summary>
        /// Shows the confirm message for successfull
        /// </summary>
        private async void ShowConfirmWindow() => await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync("Successful", "Timer saved").ConfigureAwait(false);

        #endregion Methods
    }
}