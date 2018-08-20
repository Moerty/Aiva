using Aiva.Core.Twitch;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;

namespace Aiva.Gui.ViewModels {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Chat {
        public ObservableCollection<Aiva.Models.Chat.Message> Messages { get; set; }
        public ObservableCollection<Aiva.Models.Chat.Viewer> Viewer { get; set; }
        public Aiva.Models.Chat.Viewer SelectedViewer { get; set; }
        public string MessageToSend { get; set; }

        public ICommand SendMessageCommand { get; set; }
        public ICommand MuteCommand { get; set; }
        public ICommand UnmuteCommand { get; set; }
        public ICommand ModCommand { get; set; }
        public ICommand UnmodCommand { get; set; }
        public ICommand CopyMessageCommand { get; set; }
        public ICommand CopyTwitchUsernameCommand { get; set; }
        public ICommand ShowUserInfo { get; set; }

        private readonly Core.Database.Handlers.Chat _databaseChatHandler;

        public Chat() {
            _databaseChatHandler = new Core.Database.Handlers.Chat();
            Messages = new ObservableCollection<Aiva.Models.Chat.Message>();
            Viewer = new ObservableCollection<Aiva.Models.Chat.Viewer>();
            Messages.CollectionChanged += MessageCountCheck;
            AivaClient.Instance.TwitchClient.OnMessageReceived += ChatMessageReceived;
            AivaClient.Instance.Tasks.OnModeratorsReceivedEvent += ModeratorsReceived;
            AivaClient.Instance.TwitchClient.OnUserLeft += RemoveViewerFromList;
            AivaClient.Instance.TwitchClient.OnUserJoined += AddViewerToList;
            AivaClient.Instance.TwitchClient.OnExistingUsersDetected += AddViewerToList;

            // commands
            SendMessageCommand = new Internal.RelayCommand(
                send => SendMessage(),
                send => !string.IsNullOrEmpty(MessageToSend));

            MuteCommand = new Internal.RelayCommand(
                mute => AivaClient.Instance.TwitchClient.TimeoutUser(
                    viewer: SelectedViewer.Name,
                    duration: TimeSpan.FromMinutes(5),
                    message: $"@{SelectedViewer.Name} muted through streamer!",
                    dryRun: AivaClient.DryRun),
                mute => SelectedViewer != null);

            UnmuteCommand = new Internal.RelayCommand(
                unmute => AivaClient.Instance.TwitchClient.UnbanUser(
                    viewer: SelectedViewer.Name,
                    dryRun: AivaClient.DryRun),
                unmute => SelectedViewer != null);

            ModCommand = new Internal.RelayCommand(
                mod => AivaClient.Instance.TwitchClient.Mod(AivaClient.Instance.ChannelId, SelectedViewer.Name),
                mod => SelectedViewer != null);

            UnmodCommand = new Internal.RelayCommand(
                unmod => AivaClient.Instance.TwitchClient.Unmod(AivaClient.Instance.ChannelId, SelectedViewer.Name),
                unmod => SelectedViewer != null);
        }

        private void SendMessage() {
            AivaClient.Instance.TwitchClient.SendMessage(
                channel: AivaClient.Instance.ChannelId,
                message: MessageToSend,
                dryRun: AivaClient.DryRun);

            AddMessageToList();
            _databaseChatHandler.AddMessageToDatabase(
                twitchID: Convert.ToInt32(AivaClient.Instance.TwitchId),
                message: MessageToSend,
                timeStamp: DateTime.Now);

            MessageToSend = string.Empty;
        }

        private void AddMessageToList() {
            Messages.Add(
                new Aiva.Models.Chat.Message {
                    DisplayName = AivaClient.Instance.BotName,
                    IsMe = true,
                    IsBroadcaster = true,
                    IsModerator = true,
                    MessageText = MessageToSend,
                    Username = AivaClient.Instance.BotName,
                    UserId = AivaClient.Instance.TwitchId,
                });
        }

        /// <summary>
        /// Fires when existing users detected in Channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddViewerToList(object sender, OnExistingUsersDetectedArgs e) {
            foreach (var user in e.Users) {
                var twitchUser = await AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(user).ConfigureAwait(false);

                if (twitchUser != null) {
                    AddViewerToList(twitchUser.Matches[0].Name, twitchUser.Matches[0].Id);
                }
            }
        }

        /// <summary>
        /// Fires when a user join the channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddViewerToList(object sender, OnUserJoinedArgs e) {
            var twitchUser =
                await AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(e.Username).ConfigureAwait(false);

            if (twitchUser != null) {
            }
        }

        /// <summary>
        /// Add viewer to list
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        private async void AddViewerToList(string name, string id) {
            if (Viewer.Any(v => String.Compare(v.TwitchID, id) == 0)) // check if user is already in List
                return;

            var isUserSubscriber = await AivaClient.Instance.TwitchApi.Channels.v5.CheckChannelSubscriptionByUserAsync(AivaClient.Instance.ChannelId, name);

            var rnd = new Random();
            var viewer = new Aiva.Models.Chat.Viewer {
                Name = name,
                TwitchID = id,
                IsSub = isUserSubscriber != null,
                Type = isUserSubscriber != null ? nameof(Aiva.Models.Enums.SortDirectionListView.Subscriber)
                                            : nameof(Aiva.Models.Enums.SortDirectionListView.Viewer),
                ChatNameColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256))
                //IsMod = will be filled from the event "ModeratoersReceived"
            };

            Application.Current.Dispatcher.Invoke(() => Viewer.Add(viewer));

            // Get Channel Moderators to fire "ModeratorsReceived"
            AivaClient.Instance.TwitchClient.GetChannelModerators(AivaClient.Instance.Channel);
        }

        /// <summary>
        /// Remove viewer then disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveViewerFromList(object sender, OnUserLeftArgs e) {
            var viewer = Viewer.SingleOrDefault(
                u => string.Compare(u.Name, e.Username, true) == 0);

            if (viewer != null) {
                Application.Current.Dispatcher.Invoke(() => Viewer.Remove(viewer));
            }
        }

        private void ModeratorsReceived(object sender, OnModeratorsReceivedArgs e) {
            var matches = Viewer.Where(v => e.Moderators.Contains(v.Name));

            foreach (var match in matches) {
                match.IsMod = true;
                match.Type = nameof(Aiva.Models.Enums.SortDirectionListView.Mod);
            }
        }

        /// <summary>
        /// Fires when a chat message received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatMessageReceived(object sender, OnMessageReceivedArgs e) {
            var message = new Aiva.Models.Chat.Message {
                Color = e.ChatMessage.Color.IsEmpty ? GetChatColor(e.ChatMessage.UserId) : e.ChatMessage.Color,
                DisplayName = e.ChatMessage.DisplayName,
                IsBroadcaster = e.ChatMessage.IsBroadcaster,
                IsMe = e.ChatMessage.IsMe,
                IsModerator = e.ChatMessage.IsModerator,
                IsSubscriber = e.ChatMessage.IsSubscriber,
                IsTurbo = e.ChatMessage.IsTurbo,
                MessageText = e.ChatMessage.Message,
                SubscribedMonthCount = e.ChatMessage.SubscribedMonthCount,
                UserId = Convert.ToInt32(e.ChatMessage.UserId),
                Username = e.ChatMessage.Username,
                UserType = e.ChatMessage.UserType,
            };

            Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
        }

        /// <summary>
        /// Get Chat Message for viewer
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private Color GetChatColor(string userId) {
            var viewer = Viewer.SingleOrDefault(v => String.Compare(v.TwitchID, userId, true) == 0);

            if (viewer != null) {
                return viewer.ChatNameColor;
            } else {
                var rnd = new Random();
                return Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
        }

        /// <summary>
        /// Remove last message when count > 1000
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageCountCheck(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                if (Messages.Count >= 1000) {
                    Application.Current.Dispatcher.Invoke(() => Messages.RemoveAt(0));
                }
            }
        }
    }
}