using Aiva.Core.Twitch;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using PropertyChanged;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TwitchLib.Api.Enums;
using TwitchLib.Client.Events;

namespace Aiva.Gui.ViewModels.Tabs {
    [AddINotifyPropertyChangedInterface]
    public class Dashboard {
        #region Models
        #region Graph
        public bool IsOptionsStaticCurrentViewerActive { get; set; }
        public bool IsTotalFollowersStatisticActuve { get; set; }

        public SeriesCollection CurrentViewersSeries { get; set; }
        public SeriesCollection TotalFollowersSeries { get; set; }

        private int _currentViewers = 0;
        #endregion Graph

        #region StreamOptions
        public List<Aiva.Models.Dashboard.Streamgame> Games { get; set; }
        public Aiva.Models.Dashboard.Streamgame SelectedGame { get; set; }
        public string StreamTitle { get; set; }
        #endregion StreamOptions
        public ICommand ChangeGameCommand { get; set; }
        public ICommand UpdateStreamTitleCommand { get; set; }
        public ICommand ShowCommercialFlyoutCommand { get; set; }

        // for the future
        public ICommand SlowModeOnCommand { get; set; }
        public ICommand SlowModeOffCommand { get; set; }
        public ICommand SubscriberOnlyOnCommand { get; set; }
        public ICommand SubscriberOnlyOffCommand { get; set; }
        public ICommand FollowersOnlyOnCommand { get; set; }
        public ICommand FollowersOnlyOffCommand { get; set; }
        public ICommand ClearChatCommand { get; set; }

        public int TotalViews { get; set; }
        public int TotalFollowers { get; set; }

        #endregion Models
        #region Constructor
        public Dashboard() {
            AivaClient.Instance.TwitchClient.OnExistingUsersDetected
                += ExistingUsersDetected;
            SetCurrentViewersChart();
            SetTotalFollowersChart();
            IsOptionsStaticCurrentViewerActive = true; // default
            Games = new List<Aiva.Models.Dashboard.Streamgame>();

            AivaClient.Instance.Tasks.Channel.OnNewStreamGame
                += (sender, e)
                => SelectedGame = Games.SingleOrDefault(g => string.Compare(g.Name, e) == 0) ?? default(Aiva.Models.Dashboard.Streamgame);

            AivaClient.Instance.Tasks.Channel.OnNewStreamTitle
                += (sender, e)
                => StreamTitle = e;

            AivaClient.Instance.Tasks.Channel.OnNewTotalFollower
                += (sender, e)
                => TotalFollowers = e;

            AivaClient.Instance.Tasks.Channel.OnNewTotalViews
                += (sender, e)
                => TotalViews = e;

            ChangeGameCommand = new Internal.RelayCommand(
                change => ChangeStreamTitleAndGame(),
                change => !string.IsNullOrEmpty(StreamTitle));

            UpdateStreamTitleCommand = new Internal.RelayCommand(
                change => ChangeStreamTitleAndGame(),
                change => SelectedGame != null && !string.IsNullOrEmpty(StreamTitle));

            ShowCommercialFlyoutCommand = new Internal.RelayCommand(
                show => ShowCommercialFlyout(),
                show => AivaClient.Instance.IsPartnered); // only possible when twitch partner

            //SlowModeOnCommand = new Internal.RelayCommand(
            //    slowModeOn => AivaClient.Instance.Tasks.Channel.SetSlowMode(true, SlowModeValue));

            //SlowModeOffCommand = new Internal.RelayCommand(
            //    slowModeOn => AivaClient.Instance.Tasks.Channel.SetSlowMode(false));

            //SubscriberOnlyOnCommand = new Internal.RelayCommand(
            //    subOnlyOn => AivaClient.Instance.Tasks.Channel.SetSubMode(true));

            //SubscriberOnlyOffCommand = new Internal.RelayCommand(
            //    subOnlyOff => AivaClient.Instance.Tasks.Channel.SetSubMode(true));

            //FollowersOnlyOnCommand = new Internal.RelayCommand(
            //    followerOnlyOn => AivaClient.Instance.Tasks.Channel.SetFollowersMode(true));

            Task.Factory.StartNew(async () => {
                var games = await AivaClient.Instance.TwitchApi.Games.v5.GetTopGamesAsync(100).ConfigureAwait(false);

                if (games?.Top != null) {
                    foreach (var game in games.Top) {
                        Games.Add(
                            new Aiva.Models.Dashboard.Streamgame {
                                GameId = game.Game.Id.ToString(),
                                Name = game.Game.Name
                            });
                    }

                    SetStreamGame();
                }
            });

            SetViewsAndFollowers();
            SetStreamTitle();
            //SetSlowMode();
        }

        //private void SetSlowMode() {
        //    IsSlowModeActive = AivaClient.Instance.Tasks.Channel.IsSlowModeActive;

        //    AivaClient.Instance.Tasks.Channel.SlowMode
        //        += (sender, e)
        //        => Application.Current.Dispatcher.Invoke(() => IsSlowModeActive = e);
        //}

        private void SetStreamGame() {
            if (!string.IsNullOrEmpty(AivaClient.Instance.Tasks.Channel.StreamGame)) {
                Application.Current.Dispatcher.Invoke(() => SelectedGame = Games.SingleOrDefault(g => g.Name == AivaClient.Instance.Tasks.Channel.StreamGame));
                AivaClient.Instance.Tasks.Channel.OnNewStreamGame
                    += (sender, e)
                    => Application.Current.Dispatcher.Invoke(() => SelectedGame = Games.SingleOrDefault(g => g.Name == e));
            } else {
                AivaClient.Instance.Tasks.Channel.OnNewStreamGame
                    += (sender, e)
                    => Application.Current.Dispatcher.Invoke(() => SelectedGame = Games.SingleOrDefault(g => g.Name == e));
            }
        }

        private void SetStreamTitle() {
            if (!string.IsNullOrEmpty(AivaClient.Instance.Tasks.Channel.StreamTitle)) {
                StreamTitle = AivaClient.Instance.Tasks.Channel.StreamTitle;
                AivaClient.Instance.Tasks.Channel.OnNewStreamTitle
                    += (sender, e)
                    => Application.Current.Dispatcher.Invoke(() => StreamTitle = e);
            } else {
                AivaClient.Instance.Tasks.Channel.OnNewStreamTitle
                    += (sender, e)
                    => Application.Current.Dispatcher.Invoke(() => StreamTitle = e);
            }
        }

        private void SetViewsAndFollowers() {
            if (AivaClient.Instance.Tasks.Channel.TotalViews != 0) {
                TotalViews = AivaClient.Instance.Tasks.Channel.TotalViews;
                AivaClient.Instance.Tasks.Channel.OnNewTotalViews
                    += (sender, e)
                    => Application.Current.Dispatcher.Invoke(() => TotalViews = e);
            } else {
                AivaClient.Instance.Tasks.Channel.OnNewTotalViews
                    += (sender, e)
                    => Application.Current.Dispatcher.Invoke(() => TotalViews = e);
            }

            if (AivaClient.Instance.Tasks.Channel.TotalFollowers != 0) {
                TotalFollowers = AivaClient.Instance.Tasks.Channel.TotalFollowers;
                AivaClient.Instance.Tasks.Channel.OnNewTotalFollower
                    += (sender, e)
                    => Application.Current.Dispatcher.Invoke(() => TotalFollowers = e);
            }
        }

        private void ShowCommercialFlyout() {
            var dataContext = ((ViewModels.Windows.MainWindow)Application.Current.MainWindow.DataContext);
            ((ViewModels.Flyouts.Commercial)dataContext.Model.SelectedTabItem.Flyouts[0].DataContext).OnClose
                += (sender, EventArgs)
                => CloseCommercialFlyout(EventArgs);

            dataContext.Model.SelectedTabItem.Flyouts[0].IsOpen = true;
        }

        private async void CloseCommercialFlyout(CommercialLength eventArgs) {
            var commercial = await AivaClient.Instance.TwitchApi.Channels.v5.StartChannelCommercialAsync(
                AivaClient.Instance.ChannelId, eventArgs).ConfigureAwait(false);
        }

        private void ChangeStreamTitleAndGame() {
            AivaClient.Instance.Tasks.Channel.UpdateStreamTitleAndGame(StreamTitle, SelectedGame.Name);
        }
        #endregion Constructor

        #region TotalFollowersChart
        private void SetTotalFollowersChart() {
            TotalFollowersSeries = new SeriesCollection {
                new LineSeries {
                    AreaLimit = -10,
                    Values = new ChartValues<ObservableValue> {
                        new ObservableValue(TotalFollowers)
                    }
                }
            };

            Task.Factory.StartNew(UpdateTotalFollowers);
        }

        private void UpdateTotalFollowers() {
            while (true) {
                Thread.Sleep(5000);

                Application.Current.Dispatcher.Invoke(() => TotalFollowersSeries[0].Values.Add(new ObservableValue(TotalFollowers)));

                if (TotalFollowersSeries[0].Values.Count >= 20) {
                    Application.Current.Dispatcher.Invoke(() => TotalFollowersSeries[0].Values.RemoveAt(0));
                }
            }
        }
        #endregion TotalFollowersChart

        #region CurrentViewerCount
        private void SetCurrentViewersChart() {
            CurrentViewersSeries = new SeriesCollection {
                new LineSeries {
                    AreaLimit = -10,
                    Values = new ChartValues<ObservableValue> {
                        new ObservableValue(TotalFollowers)
                    }
                }
            };

            Task.Factory.StartNew(UpdateCurrentViewers);

            AivaClient.Instance.TwitchClient.OnUserJoined
                += UserJoined;
        }

        private void ExistingUsersDetected(object sender, OnExistingUsersDetectedArgs e) {
            _currentViewers = e.Users.Count;
        }

        private void UpdateCurrentViewers() {
            while (true) {
                Thread.Sleep(5000);

                Application.Current.Dispatcher.Invoke(() => CurrentViewersSeries[0].Values.Add(new ObservableValue(_currentViewers)));

                if (CurrentViewersSeries[0].Values.Count >= 20) {
                    Application.Current.Dispatcher.Invoke(() => CurrentViewersSeries[0].Values.RemoveAt(0));
                }
            }
        }

        private void UserJoined(object sender, OnUserJoinedArgs e) {
            _currentViewers++;
        }
        #endregion CurrentViewerCount
    }
}