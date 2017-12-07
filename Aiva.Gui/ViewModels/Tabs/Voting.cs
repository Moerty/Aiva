using Aiva.Models.Voting;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.Tabs {
    [AddINotifyPropertyChangedInterface]
    public class Voting {
        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        private Models.ChartValues ChartValues { get; set; }
        public SeriesCollection Chart { get; set; }
        public ObservableCollection<Aiva.Models.Voting.VotedUser> VotedUsers { get; set; }

        public bool IsStarted { get; set; }

        private Extensions.Voting.Handler _handler;

        public Voting() {
            StartCommand = new Internal.RelayCommand(
                start => StartVoting(),
                start => _handler == null);

            StopCommand = new Internal.RelayCommand(
                stop => StopVoting(),
                stop => IsStarted);

            ResetCommand = new Internal.RelayCommand(
                reset => DoReset());
        }

        private void DoReset() {
            ChartValues = null;
            Chart = null;
            VotedUsers = null;
            IsStarted = false;
            _handler?.StopListining();
            _handler = null;
        }

        private void StopVoting() {
            _handler.StopListining();
        }

        private async void StartVoting() {
            var start = new Views.ChildWindows.Voting.Start() { IsModal = true, AllowMove = true };

            ((ChildWindows.Voting.Start)start.DataContext).CloseEvent
                += (sender, EventArgs) => CloseStartWindow(start, true);
            start.CloseButtonCommand = new Internal.RelayCommand(c => IsStarted = false);

            start.Closing += (sender, EventArgs) => CloseStartWindow(start);

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(start).ConfigureAwait(false);
        }

        private void CloseStartWindow(Views.ChildWindows.Voting.Start start, bool fromDatacontext = false) {
            var dataContext = Internal.ChildWindow.GetDatacontext
                <ViewModels.ChildWindows.Voting.Start>(start.DataContext);

            if (dataContext?.IsCompleted() == true && fromDatacontext) {
                _handler = new Extensions.Voting.Handler(dataContext.Properties);
                _handler.StartListining();
                ChartValues = new Models.ChartValues();
                CreateChart(dataContext.Properties);
                VotedUsers = new ObservableCollection<VotedUser>();
                _handler.OnUserVoted += OnUserVoted;
                IsStarted = true;
                start.Close();
            } else {
                IsStarted = false;
            }
        }

        /// <summary>
        /// Create Chart cause class 'piechart' is only
        /// available in livecharts.wpf
        /// </summary>
        /// <param name="addModel"></param>
        /// <param name="properties"></param>
        private void CreateChart(Aiva.Models.Voting.Properties properties) {
            Chart = new SeriesCollection();

            if (properties.Options.Option1.ActiveOption) {
                Chart.Add(
                    new PieSeries {
                        Title = properties.Options.Option1.Option,
                        Values = new ChartValues<ObservableValue> { ChartValues.Option1 },
                        DataLabels = true,
                    });
            }

            if (properties.Options.Option2.ActiveOption) {
                Chart.Add(
                    new PieSeries {
                        Title = properties.Options.Option2.Option,
                        Values = new ChartValues<ObservableValue> { ChartValues.Option2 },
                        DataLabels = true,
                    });
            }

            if (properties.Options.Option3.ActiveOption) {
                Chart.Add(
                    new PieSeries {
                        Title = properties.Options.Option3.Option,
                        Values = new ChartValues<ObservableValue> { ChartValues.Option3 },
                        DataLabels = true,
                    });
            }

            if (properties.Options.Option4.ActiveOption) {
                Chart.Add(
                    new PieSeries {
                        Title = properties.Options.Option4.Option,
                        Values = new ChartValues<ObservableValue> { ChartValues.Option4 },
                        DataLabels = true,
                    });
            }

            if (properties.Options.Option4.ActiveOption) {
                Chart.Add(
                    new PieSeries {
                        Title = properties.Options.Option5.Option,
                        Values = new ChartValues<ObservableValue> { ChartValues.Option5 },
                        DataLabels = true
                    });
            }

            if (properties.Options.Option6.ActiveOption) {
                Chart.Add(
                    new PieSeries {
                        Title = properties.Options.Option6.Option,
                        Values = new ChartValues<ObservableValue> { ChartValues.Option6 },
                        DataLabels = true
                    });
            }
        }

        private void OnUserVoted(object sender, VotedUser e) {
            switch (e.Option) {
                case Aiva.Models.Enums.VotingOption.Option1:
                    ChartValues.Option1.Value++;
                    break;
                case Aiva.Models.Enums.VotingOption.Option2:
                    ChartValues.Option2.Value++;
                    break;
                case Aiva.Models.Enums.VotingOption.Option3:
                    ChartValues.Option3.Value++;
                    break;
                case Aiva.Models.Enums.VotingOption.Option4:
                    ChartValues.Option4.Value++;
                    break;
                case Aiva.Models.Enums.VotingOption.Option5:
                    ChartValues.Option5.Value++;
                    break;
                case Aiva.Models.Enums.VotingOption.Option6:
                    ChartValues.Option6.Value++;
                    break;
            }
            Application.Current.Dispatcher.Invoke(() => VotedUsers.Add(e));
        }
    }
}