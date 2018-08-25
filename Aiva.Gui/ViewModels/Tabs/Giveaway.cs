using Aiva.Gui.Views.ChildWindows.Giveaway;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace Aiva.Gui.ViewModels.Tabs {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Giveaway {
        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand TakeWinnerCommand { get; set; }
        public ObservableCollection<Aiva.Models.Giveaway.Users> JoinedUsers { get; set; }
        public ObservableCollection<Aiva.Models.Giveaway.Users> Winners { get; set; }
        public ObservableCollection<Aiva.Models.Giveaway.Messages> WinnerMessages { get; set; }

        public string WinnersSeperated { get; set; }

        public bool IsStarted { get; set; }

        private Extensions.Giveaway.Handler _handler;

        public Giveaway() {
            StartCommand = new Internal.RelayCommand(
                start => OpenOptionWindow(),
                start => _handler == null);

            StopCommand = new Internal.RelayCommand(
                stop => StopListining(),
                stop => IsStarted);

            TakeWinnerCommand = new Internal.RelayCommand(
                winner => _handler.GetWinner(),
                winner => _handler != null);

            ResetCommand = new Internal.RelayCommand(
                reset => DoReset(),
                reset => _handler != null);
        }

        private void DoReset() {
            _handler.DoReset();
            _handler = null;
            WinnersSeperated = string.Empty;
            JoinedUsers = new ObservableCollection<Aiva.Models.Giveaway.Users>();
            Winners = new ObservableCollection<Aiva.Models.Giveaway.Users>();
            WinnerMessages = new ObservableCollection<Aiva.Models.Giveaway.Messages>();
        }

        private void StopListining() {
            IsStarted = false;
            _handler.StopRegistration();
            _handler = null;
        }

        private async void OpenOptionWindow() {
            var start = new Start();

            start.Closing
                += (sender, EventArgs) => CloseStartWindow(start);

            ((ChildWindows.Giveaway.Start)start.DataContext).CloseEvent
                += (sender, EventArgs) => CloseStartWindow(start, true);

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(start).ConfigureAwait(false);
        }

        private void CloseStartWindow(Start start, bool fromDatacontext = false) {
            var dataContext = Internal.ChildWindow.GetDatacontext
                <ChildWindows.Giveaway.Start>(start.DataContext);

            if (dataContext?.IsCompleted() == true && fromDatacontext) {
                var properties = new Aiva.Models.Giveaway.Properties {
                    BeFollower = dataContext.Properties.BeFollower,
                    BlockReEntry = dataContext.Properties.BlockReEntry,
                    Command = dataContext.Properties.Command,
                    SubLuck = dataContext.Properties.IsSubLuckActive ? dataContext.Properties.SubLuck : 1, // when subluck active use subluck, otherwise 1 (for later calc with subluck)
                    IsSubLuckActive = dataContext.Properties.IsSubLuckActive,
                    Price = dataContext.Properties.Price,
                    JoinPermission = dataContext.Properties.JoinPermission
                };
                IsStarted = true;
                _handler = new Extensions.Giveaway.Handler(properties);

                start.Close();

                StartGiveaway();
            } else {
                IsStarted = false;
            }
        }

        private void StartGiveaway() {
            JoinedUsers = new ObservableCollection<Aiva.Models.Giveaway.Users>();
            Winners = new ObservableCollection<Aiva.Models.Giveaway.Users>();
            WinnerMessages = new ObservableCollection<Aiva.Models.Giveaway.Messages>();
            _handler.OnJoinedUser += UserJoined;
            _handler.OnWinnerFound += WinnerFound;
            _handler.OnWinnerMessageReceived += WinnerMessageReceived;
            _handler.OnTimerEnds += (sender, EventArgs) => _handler?.StopRegistration();
            _handler.StartRegistration();
        }

        private void WinnerMessageReceived(object sender, Aiva.Models.Giveaway.Messages e) {
            Application.Current.Dispatcher.Invoke(() => WinnerMessages.Add(e));
        }

        private void WinnerFound(object sender, Aiva.Models.Giveaway.Users e) {
            Winners.Add(e);
            JoinedUsers.Remove(e);

            if(!string.IsNullOrEmpty(WinnersSeperated)) {
                WinnersSeperated += $" | {e.Username}";
            } else {
                WinnersSeperated += e.Username;
            }
        }

        private void UserJoined(object sender, Aiva.Models.Giveaway.Users e) {
            Application.Current.Dispatcher.Invoke(() => JoinedUsers.Add(e));
        }
    }
}