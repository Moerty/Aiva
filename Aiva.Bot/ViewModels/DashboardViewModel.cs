using System;
using System.Threading.Tasks;
using Aiva.Core.Config;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class DashboardViewModel {
        public Models.DashboardModel Model { get; set; }

        public ICommand ChangeTitleCommand { get; set; } = new RoutedCommand();
        public ICommand ChangeGameCommand { get; set; } = new RoutedCommand();
        public ICommand ShowCommercialCommand { get; set; } = new RoutedCommand();

        private DispatcherTimer RefreshData;

        public DashboardViewModel() {
            //Create Models
            CreateModels();

            // Commands
            SetCommands();

            // Set Timers
            SetTimers();
        }

#region timers
        /// <summary>
        /// Set Timer
        /// </summary>
        private void SetTimers() {
            // Viewers
            RefreshData = new DispatcherTimer(DispatcherPriority.Background) {
                Interval = new TimeSpan(0, 1, 0)
            };
            RefreshData.Tick += ViewersTimer_Tick;
            RefreshData.Start();
        }

        /// <summary>
        /// Timer tick to check Static values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ViewersTimer_Tick(object sender, EventArgs e) {
            var streamOnline = TwitchLib.TwitchApi.Streams.StreamIsLive(Core.Client.AivaClient.Client.Channel);
            if (streamOnline) {
                Model.Viewers = TwitchLib.TwitchApi.Streams.GetStream(Core.Client.AivaClient.Client.Channel).Viewers;
            }

            // Followers
            Model.Followers = new ObservableCollection<Extensions.Models.Dashboard.Followers>(await Extensions.Dashboard.Followers.GetFollowersAsync());

            // SelectedGame | StreamTitle | TotalViews
            var channelInfo = await GetChannelDetails();
            Model.SelectedGame = channelInfo.Item2;
            Model.StreamTitle = channelInfo.Item1;
            Model.TotalViews = channelInfo.Item3;

            // Viewers
            Model.Viewers = await GetActiveViewers();

            // Last Follower
            Model.LastFollower = await GetLastFollower();

            RefreshData.Start();
        }

        #endregion timers

#region commands
        /// <summary>
        /// Set Commands
        /// </summary>
        private void SetCommands() {
            var type = new MahApps.Metro.Controls.MetroContentControl().GetType();

            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(ChangeTitleCommand, ChangeTitle));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(ChangeGameCommand, ChangeGame));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(ShowCommercialCommand, ShowCommercial));
        }

        /// <summary>
        /// Show Commercial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowCommercial(object sender, ExecutedRoutedEventArgs e) {
            Extensions.Dashboard.Stream.ShowCommercial(Model.CommercialLength);
        }

        /// <summary>
        /// Change the Game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChangeGame(object sender, ExecutedRoutedEventArgs e) {
            Extensions.Dashboard.Games.ChangeGame(Model.SelectedGame);
        }

        /// <summary>
        /// Change the Title
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChangeTitle(object sender, ExecutedRoutedEventArgs e) {
            Extensions.Dashboard.Stream.ChangeTitle(Model.StreamTitle);
        }

        #endregion commands

#region models

        /// <summary>
        /// Create Models
        /// </summary>
        private async void CreateModels() {
            var channelInfo = await GetChannelDetails();

            Model = new Models.DashboardModel {
                Followers = new ObservableCollection<Extensions.Models.Dashboard.Followers>(await Extensions.Dashboard.Followers.GetFollowersAsync()),
                Games = await GetTwitchGames(),
                SelectedGame = channelInfo.Item2,
                StreamTitle = channelInfo.Item1,
                TotalViews = channelInfo.Item3,
                Viewers = await GetActiveViewers(),
                LastFollower = await GetLastFollower(),

                CommercialLengthList = new System.Collections.Generic.List<int> {
                    30,60,90,120,150,180
                },

                Text = new Models.DashboardModel.TextModel {
                    DashboardExpanderFollowerNameText = LanguageConfig.Instance.GetString("DashboardExpanderFollowerNameText"),
                    DashboardExpanderStatisticNameText = LanguageConfig.Instance.GetString("DashboardExpanderStatisticNameText"),
                    DashboardExpanderFollowerCreatedAtColumn = LanguageConfig.Instance.GetString("DashboardExpanderFollowerCreatedAtColumn"),
                    DashboardExpanderFollowerNameColumn = LanguageConfig.Instance.GetString("DashboardExpanderFollowerNameColumn"),
                    DashboardExpanderFollowerNotificationColumn = LanguageConfig.Instance.GetString("DashboardExpanderFollowerNotificationColumn"),
                    DashboardButtonGameText = LanguageConfig.Instance.GetString("DashboardButtonGameText"),
                    DashboardButtonTitleText = LanguageConfig.Instance.GetString("DashboardButtonTitleText"),
                    DashboardLabelCommercialText = LanguageConfig.Instance.GetString("DashboardLabelCommercialText"),
                    DashboardLabelGameText = LanguageConfig.Instance.GetString("DashboardLabelGameText"),
                    DashboardLabelTitleText = LanguageConfig.Instance.GetString("DashboardLabelTitleText"),
                    DashboardLabelTotalViewsText = LanguageConfig.Instance.GetString("DashboardLabelTotalViewsText"),
                    DashboardLabelViewersCountText = LanguageConfig.Instance.GetString("DashboardLabelViewersCountText"),
                    DashboardLabelLastFollowerText = LanguageConfig.Instance.GetString("DashboardLabelLastFollowerText"),
                    DashboardButtonCommercialText = LanguageConfig.Instance.GetString("DashboardButtonCommercialText"),
                }
            };
        }

        /// <summary>
        /// Get active Viewers
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetActiveViewers() {
            return await Extensions.Dashboard.Stream.GetActiveViewers();
        }

        /// <summary>
        /// Get last Follower
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetLastFollower() {
            return await Extensions.Dashboard.Stream.GetLastFollower();
        }

        /// <summary>
        /// Get Channelname and Game
        /// </summary>
        /// <returns>Channel ; Game</returns>
        private async Task<Tuple<string, string, int>> GetChannelDetails() {
            return await Extensions.Dashboard.Stream.GetChannelDetails();
        }

        /// <summary>
        /// Get Twitch Games
        /// </summary>
        /// <returns></returns>
        private async Task<ObservableCollection<string>> GetTwitchGames() {
            return new ObservableCollection<string>(await Extensions.Dashboard.Games.GetTwitchGamesAsync());
        }

#endregion models
    }
}

