using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Timers {

        public ICommand AddTimerCommand { get; set; }

        private Core.DatabaseHandlers.Timers _databaseHandler;

        public Timers() {
            _databaseHandler = new Core.DatabaseHandlers.Timers();
            SetCommands();
        }

        /// <summary>
        /// Set commands
        /// </summary>
        private void SetCommands() {
            AddTimerCommand = new Internal.RelayCommand(async add => await ShowAddWindow());
        }

        /// <summary>
        /// Show that add child window
        /// IMHO a giant hack against mvvm
        /// </summary>
        /// <returns></returns>
        private async Task ShowAddWindow() {
            var addTimerWindow = new Views.ChildWindows.AddTimer() { IsModal = true, AllowMove = true };
            ((ViewModels.ChildWindows.AddTimer)addTimerWindow.DataContext).CloseEvent += (sender, EventArgs) => ClosingAddWindow(addTimerWindow);
            addTimerWindow.Closing += (sender, CancelEventArgs) => ClosingAddWindow(addTimerWindow.DataContext);

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
                        _databaseHandler.AddTimer(dataContext.Name, dataContext.Text, dataContext.Interval, dataContext.Lines);
                        ShowConfirmWindow();
                    }
                }

                window.Close();
            }
        }

        private async void ShowConfirmWindow() {
            await Application.Current.MainWindow.ShowChildWindowAsync(new ChildWindow() { Content = "Completed", IsModal = true, AllowMove = true });
            ;
        }
    }
}
