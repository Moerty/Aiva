using System;
using System.Threading.Tasks;
using Aiva.Core.Config;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class DashboardViewModel {
        public Models.DashboardModel Model { get; set; }

        public ICommand ChangeTitleCommand { get; set; } = new RoutedCommand();
        public ICommand ChangeGameCommand { get; set; } = new RoutedCommand();


        public DashboardViewModel() {
            //Create Models
            CreateModels();

            // Commands
            SetCommands();
        }

        private void SetCommands() {
            var type = new MahApps.Metro.Controls.MetroContentControl().GetType();

            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(ChangeTitleCommand, ChangeTitle));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(ChangeGameCommand, ChangeGame));
        }

        /// <summary>
        /// Change the Game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChangeGame(object sender, ExecutedRoutedEventArgs e) {
            await TwitchLib.TwitchApi.Streams.UpdateStreamGameAsync(Model.SelectedGame, Core.Client.AivaClient.Client.Channel);
        }

        /// <summary>
        /// Change the Title
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChangeTitle(object sender, ExecutedRoutedEventArgs e) {
            await TwitchLib.TwitchApi.Streams.UpdateStreamTitleAsync(Model.StreamTitle, Core.Client.AivaClient.Client.Channel);
        }

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

                }
            };
        }

        /// <summary>
        /// Get Channelname and Game
        /// </summary>
        /// <returns>Channel ; Game</returns>
        async Task<Tuple<string, string>> GetChannelDetails() {
            var channel = await TwitchLib.TwitchApi.Channels.GetChannelAsync(Core.Client.AivaClient.Client.Channel);

            return new Tuple<string, string>(channel.Status, channel.Game);
        }

        /// <summary>
        /// Get Twitch Games
        /// </summary>
        /// <returns></returns>
        private async Task<ObservableCollection<string>> GetTwitchGames() {
            return new ObservableCollection<string>(
                await Extensions.Dashboard.Games.GetTwitchGamesAsync());
        }
    }
}

