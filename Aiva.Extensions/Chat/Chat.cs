using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using Aiva.Core.Models;
using TwitchLib.Events.Client;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Aiva.Core;

namespace Aiva.Extensions.Chat {

    /*
     * Can show Messages for GUI
     * Store Messages in Database
     * Write Messages from GUI
     */
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Chat {

        public ObservableCollection<Models.Chat.MessageModel> Messages { get; set; }
        public ObservableCollection<Models.Chat.Viewers> Viewers { get; set; }
        public Models.Chat.Viewers SelectedViewer { get; set; }

        public Chat() {
            Messages = new ObservableCollection<Models.Chat.MessageModel>();
            Viewers = new ObservableCollection<Models.Chat.Viewers>();
            Messages.CollectionChanged += MessagesCountCheck;
            Core.AivaClient.Instance.AivaTwitchClient.OnMessageReceived += ChatMessageReceived;
            Core.AivaClient.Instance.Tasks.OnModeratorsReceivedEvent += ModeratorsReceived;
            Core.AivaClient.Instance.AivaTwitchClient.OnUserLeft += RemoveViewerFromViewers;
            Core.AivaClient.Instance.AivaTwitchClient.OnUserJoined += AivaTwitchClient_OnUserJoined;
            Core.AivaClient.Instance.AivaTwitchClient.OnExistingUsersDetected += AivaTwitchClient_OnExistingUsersDetected;
        }

        private void AivaTwitchClient_OnExistingUsersDetected(object sender, OnExistingUsersDetectedArgs e) {
            foreach (var user in e.Users) {
                var twitchUser = AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(user).Result;

                if (twitchUser != null) {
                    OnNewUserFound(twitchUser.Matches[0].Name, twitchUser.Matches[0].Id);
                }
            }
        }

        private void AivaTwitchClient_OnUserJoined(object sender, OnUserJoinedArgs e) {
            var twitchUser = AivaClient.Instance.TwitchApi.Users.v5.GetUserByNameAsync(e.Username).Result;

            if (twitchUser != null) {
                OnNewUserFound(twitchUser.Matches[0].Name, twitchUser.Matches[0].Id);
            }
        }

        private void OnNewUserFound(string name, string id) {

            if (Viewers.Any(v => String.Compare(v.TwitchID, id) == 0)) // check if user is already in List
                return;

            var IsUserSubscriber = AivaClient.Instance.TwitchApi.Subscriptions.v3.ChannelHasUserSubscribedAsync(Core.AivaClient.Instance.Channel, name).Result;

            var rnd = new Random();
            var viewer = new Models.Chat.Viewers {
                Name = name,
                TwitchID = id,
                IsSub = IsUserSubscriber != null ? true : false,
                Type = IsUserSubscriber != null ? nameof(Models.Chat.SortDirectionListView.Subscriber)
                                            : nameof(Models.Chat.SortDirectionListView.Viewer),
                ChatNameColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256))
                //IsMod = will be filled from the event "ModeratoersReceived"
            };

            Application.Current.Dispatcher.Invoke(() => { Viewers.Add(viewer); });


            // Get Channel Moderators to fire "ModeratorsReceived"
            Core.AivaClient.Instance.AivaTwitchClient.GetChannelModerators(Core.AivaClient.Instance.Channel);
        }

        /// <summary>
        /// Check if a joined
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModeratorsReceived(object sender, OnModeratorsReceivedArgs e) {
            var matches = Viewers.Where(v => e.Moderators.Contains(v.Name));

            foreach (var match in matches) {
                match.IsMod = true;
                match.Type = nameof(Models.Chat.SortDirectionListView.Mod);
            }
        }

        /// <summary>
        /// Remove User from Viewers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveViewerFromViewers(object sender, OnUserLeftArgs e) {
            var viewer = Viewers.SingleOrDefault(u => u.Name == e.Username);

            if (viewer != null) {

                Application.Current.Dispatcher.Invoke(() => {
                    Viewers.Remove(viewer);
                });
            }
        }

        /// <summary>
        /// Remove Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessagesCountCheck(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                if (Messages.Count >= 1000) {
                    Application.Current.Dispatcher.Invoke(() => {
                        Messages.RemoveAt(0);
                    });
                }
            }
        }

        /// <summary>
        /// Fires when ChatMessage Received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatMessageReceived(object sender, OnMessageReceivedArgs e) {

            var message = new Models.Chat.MessageModel {
                Color = e.ChatMessage.Color.IsEmpty ? GetChatColor(e.ChatMessage.UserId) : e.ChatMessage.Color,
                DisplayName = e.ChatMessage.DisplayName,
                IsBroadcaster = e.ChatMessage.IsBroadcaster,
                IsMe = e.ChatMessage.IsMe,
                IsModerator = e.ChatMessage.IsModerator,
                IsSubscriber = e.ChatMessage.IsSubscriber,
                IsTurbo = e.ChatMessage.IsTurbo,
                Message = e.ChatMessage.Message,
                SubscribedMonthCount = e.ChatMessage.SubscribedMonthCount,
                UserId = e.ChatMessage.UserId,
                Username = e.ChatMessage.Username,
                UserType = e.ChatMessage.UserType,
            };

            Application.Current.Dispatcher.Invoke(() => {
                Messages.Add(message);
            });

            // Save in Database
            StoreIndatabase(e.ChatMessage.UserId, e.ChatMessage.Message, DateTime.Now);
        }

        private Color GetChatColor(string userId) {
            var viewer = Viewers.SingleOrDefault(v => String.Compare(v.TwitchID, userId, true) == 0);

            if (viewer != null) {
                return viewer.ChatNameColor;
            } else {
                var rnd = new Random();
                return Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
        }

        /// <summary>
        /// Store Message in Database
        /// </summary>
        /// <param name="AddModel"></param>
        private static void StoreIndatabase(string twitchId, string chatMessage, DateTime timeStamp) {
            using (var context = new Core.Storage.StorageEntities()) {
                context.Chat.Add(
                    new Core.Storage.Chat {
                        TwitchID = twitchId,
                        ChatMessage = chatMessage,
                        Timestamp = timeStamp,
                    });

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Send a Message to the Chat
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message) {
            Core.AivaClient.Instance.AivaTwitchClient.SendMessage(message);
            PutMessageInList(message);
            AddMessageToDatabase(message);
        }

        /// <summary>
        /// Put the self written Message into the Database
        /// </summary>
        /// <param name="message"></param>
        private static void AddMessageToDatabase(string message) {
            // Database
            StoreIndatabase(Core.AivaClient.Instance.TwitchID, message, DateTime.Now);
        }

        /// <summary>
        /// Put the self written Message into the Messageslist
        /// </summary>
        /// <param name="message"></param>
        private void PutMessageInList(string message) {
            Messages.Add(new Models.Chat.MessageModel {
                DisplayName = Core.AivaClient.Instance.Username,
                IsMe = true,
                IsBroadcaster = true,
                IsModerator = true,
                Message = message,
                Username = Core.AivaClient.Instance.Username,
                UserId = Core.AivaClient.Instance.TwitchID,
            });
        }
    }
}
