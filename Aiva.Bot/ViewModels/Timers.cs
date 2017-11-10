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
            AddTimerCommand = new Internal.RelayCommand(async add => await ShowAddWindow());
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

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(addTimerWindow);
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

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(addTimerWindow);
        }

        /// <summary>
        /// Fires when add child window close
        /// </summary>
        /// <param name="rawWindow"></param>
        private void ClosingAddWindow(object rawWindow) {
            Views.ChildWindows.AddTimer window;
            ChildWindows.AddTimer dataContext;

            if ((window = rawWindow as Views.ChildWindows.AddTimer) != null) {
                if ((dataContext = window.DataContext as ChildWindows.AddTimer) != null) {
                    if (dataContext.IsCompleted) {
                        if (!dataContext.IsEditing) {
                            var result = Handler.AddTimerToDatabase(dataContext.Name, dataContext.Text, dataContext.Interval, dataContext.Lines);
                            if (result) {
                                ShowConfirmWindow();
                            }
                        } else {
                            Handler.EditTimer(dataContext.Name, dataContext.Text, dataContext.Interval, dataContext.Lines, dataContext.DatabaseID);
                        }
                    }
                }

                window.Close();
            }
        }

        /// <summary>
        /// Shows the confirm message for successfull
        /// </summary>
        private async void ShowConfirmWindow() => await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync("Successful", "Timer saved");

        #endregion Methods
    }
}