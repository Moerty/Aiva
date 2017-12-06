using Aiva.Gui.Views.ChildWindows.Timers;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.Tabs {
    [AddINotifyPropertyChangedInterface]
    public class Timers {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        public ObservableCollection<Core.Database.Storage.Timers> TimersList { get; set; }
        public Core.Database.Storage.Timers SelectedTimer { get; set; }

        private readonly Extensions.Timers.Handler _handler;

        public Timers() {
            _handler = new Extensions.Timers.Handler();
            TimersList = new ObservableCollection<Core.Database.Storage.Timers>(_handler.GetTimers());

            AddCommand = new Internal.RelayCommand(
                add => ShowTimersOptionWindow(false));

            EditCommand = new Internal.RelayCommand(
                edit => ShowTimersOptionWindow(true),
                edit => SelectedTimer != null);

            RemoveCommand = new Internal.RelayCommand(
                remove => RemoveSelectedTimer(),
                remove => SelectedTimer != null);
        }

        private void RemoveSelectedTimer() {
            _handler.RemoveTimerFromDatabase(SelectedTimer);
            TimersList.Remove(SelectedTimer);
            SelectedTimer = null;
        }

        private async void ShowTimersOptionWindow(bool editing) {
            Add optionsTimerWindow;
            if (editing) {
                optionsTimerWindow = new Views.ChildWindows.Timers.Add(
                    name: SelectedTimer.Name,
                    text: SelectedTimer.Text,
                    interval: SelectedTimer.Interval,
                    id: SelectedTimer.TimersId) {
                    IsModal = true
                };
            } else {
                optionsTimerWindow = new Add {
                    IsModal = true
                };
            }

            optionsTimerWindow.Closing
                += (sender, EventArgs) => CloseOptionsWindow(optionsTimerWindow);

            ((ChildWindows.Timers.Add)optionsTimerWindow.DataContext).CloseEvent
                += (sender, EventArgs) => CloseOptionsWindow(optionsTimerWindow, true);

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(optionsTimerWindow).ConfigureAwait(false);
        }

        private void CloseOptionsWindow(Add addTimerWindow, bool fromDatacontext = false) {
            var dataContext = Internal.ChildWindow.GetDatacontext
                <ChildWindows.Timers.Add>(addTimerWindow.DataContext);

            if (dataContext?.IsCompleted() == true && fromDatacontext) {
                if (!dataContext.IsEditing) {
                    var timer = new Core.Database.Storage.Timers {
                        CreatedAt = DateTime.Now,
                        Name = dataContext.Name.Replace(" ", ""),
                        Text = dataContext.Text,
                        Interval = dataContext.Interval,
                        NextExecution = DateTime.Now.AddMinutes(dataContext.Interval)
                    };

                    _handler.AddTimerToDatabase(timer);
                    TimersList.Add(timer);
                    addTimerWindow.Close();
                    // if editing (using the same view for add and edit)
                } else {
                    _handler.EditTimer(
                        name: dataContext.Name,
                        text: dataContext.Text,
                        interval: dataContext.Interval,
                        id: Convert.ToInt32(dataContext.DatabaseID));

                    // refresh timer in timers list
                    TimersList = new ObservableCollection<Core.Database.Storage.Timers>(_handler.GetTimers());
                }

                addTimerWindow.Close();
            }
        }
    }
}