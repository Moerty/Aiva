using Aiva.Bot.Views.ChildWindows;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Aiva.Bot.ViewModels.ChildWindows.Voting;
using Aiva.Extensions.Models;
using System;
using LiveCharts.Wpf;
using LiveCharts;
using LiveCharts.Defaults;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Voting {
        #region Models

        public Extensions.Voting.Handler Handler { get; set; }
        public SeriesCollection Chart { get; set; }

        public ICommand StartVotingCommand { get; set; }
        public ICommand StopVotingCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        #endregion Models

        #region Constructor

        public Voting() {
            SetCommands();
        }

        private void SetCommands() {
            StartVotingCommand = new Internal.RelayCommand(g => StartVotingSetup(), g => Handler == null);
            StopVotingCommand = new Internal.RelayCommand(g => StopGiveaway(), g => CanStopVoting());
            ResetCommand = new Internal.RelayCommand(g => Reset());
        }

        #endregion Constructor

        #region Functions

        /// <summary>
        /// Resets the giveaway
        /// </summary>
        private void Reset() {
            Handler?.StopVoting();
            Handler = null;
        }

        private bool CanStopVoting() {
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
        private async void StartVotingSetup() {
            var startVotingWindow = new Views.ChildWindows.Voting.StartVoting() { IsModal = true, AllowMove = true };
            ((ViewModels.ChildWindows.Voting.Start)startVotingWindow.DataContext).CloseEvent += (sender, EventArgs) => CloseStartWindow(startVotingWindow);

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(startVotingWindow).ConfigureAwait(false);
        }

        /// <summary>
        /// Fires when the start giveaway form closed
        /// </summary>
        /// <param name="startTimerWindow"></param>
        /// <param name="startVotingWindow"></param>
        private void CloseStartWindow(Views.ChildWindows.Voting.StartVoting startVotingWindow) {
            //Views.ChildWindows.StartGiveaway window;
            //ChildWindows.StartGiveaway dataContext;

            var dataContext = Internal.SimpleChildWindow.GetDataContext<Views.ChildWindows.Voting.StartVoting, ChildWindows.Voting.Start>
                (startVotingWindow, startVotingWindow.DataContext);

            if (dataContext?.Item1 != null && dataContext?.Item2 != null) {
                dataContext.Item1.Close();
                StartVoting(dataContext.Item2);
            }
        }

        /// <summary>
        /// Start Voting
        /// </summary>
        /// <param name="dataContext"></param>
        private void StartVoting(Start dataContext) {
            Handler = new Extensions.Voting.Handler {
                IsStarted = true,
                Properties = dataContext.Properties,
                ChartValues = new Extensions.Models.Voting.ChartValues {
                    Option1Usernames = new System.Collections.ObjectModel.ObservableCollection<string>(),
                    Option2Usernames = new System.Collections.ObjectModel.ObservableCollection<string>(),
                    Option3Usernames = new System.Collections.ObjectModel.ObservableCollection<string>(),
                    Option4Usernames = new System.Collections.ObjectModel.ObservableCollection<string>(),
                    Option5Usernames = new System.Collections.ObjectModel.ObservableCollection<string>(),
                    Option6Usernames = new System.Collections.ObjectModel.ObservableCollection<string>()
                }
            };
            Handler.StartRegistration();

            CreateChart(dataContext.Properties);
        }

        /// <summary>
        /// Create Chart cause class 'piechart' is only
        /// available in livecharts.wpf
        /// </summary>
        /// <param name="addModel"></param>
        private void CreateChart(Extensions.Models.Voting.Properties addModel) {
            Chart = new SeriesCollection();

            if(addModel.Options.Option1.ActiveOption) {
                Chart.Add(
                    new PieSeries {
                        Title = addModel.Options.Option1.Option,
                        Values = new ChartValues<ObservableValue> { Handler.ChartValues.Option1 },
                        DataLabels = true
                    });
            }

            if (addModel.Options.Option2.ActiveOption) {
                Chart.Add(
                    new PieSeries {
                        Title = addModel.Options.Option2.Option,
                        Values = new ChartValues<ObservableValue> { Handler.ChartValues.Option2 },
                        DataLabels = true
                    });
            }

            if (addModel.Options.Option3.ActiveOption) {
                Chart.Add(
                    new PieSeries {
                        Title = addModel.Options.Option3.Option,
                        Values = new ChartValues<ObservableValue> { Handler.ChartValues.Option3 },
                        DataLabels = true
                    });
            }

            if (addModel.Options.Option4.ActiveOption) {
                Chart.Add(
                    new PieSeries {
                        Title = addModel.Options.Option4.Option,
                        Values = new ChartValues<ObservableValue> { Handler.ChartValues.Option4 },
                        DataLabels = true
                    });
            }

            if (addModel.Options.Option4.ActiveOption) {
                Chart.Add(
                    new PieSeries {
                        Title = addModel.Options.Option5.Option,
                        Values = new ChartValues<ObservableValue> { Handler.ChartValues.Option5 },
                        DataLabels = true
                    });
            }

            if (addModel.Options.Option6.ActiveOption) {
                Chart.Add(
                    new PieSeries {
                        Title = addModel.Options.Option6.Option,
                        Values = new ChartValues<ObservableValue> { Handler.ChartValues.Option6 },
                        DataLabels = true
                    });
            }
        }

        ///// <summary>
        ///// Creates the handler
        ///// </summary>
        ///// <param name="data"></param>
        //private void StartGiveaway(ChildWindows.StartGiveaway data) {
        //    Handler = new Extensions.Giveaway.GiveawayHandler() {
        //        Properties = new Extensions.Models.Giveaway.Properties {
        //            BeFollower = data.BeFollower,
        //            BlockReEntry = data.BlockReEntry,
        //            Command = data.Command,
        //            SubLuck = data.IsSubluckActive ? data.SubLuck : 1,
        //            IsSubLuckActive = data.IsSubluckActive,
        //            Price = data.Price,
        //            JoinPermission = data.SelectedJoinPermission
        //        }
        //    };

        //    Handler.StartGiveaway();
        //}

        #endregion Functions
    }
}