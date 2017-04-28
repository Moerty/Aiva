using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using TwitchLib.Events.Client;

namespace Aiva.Extensions.Chat {

    /*
     * Can show Messages for GUI
     * Store Messages in Database
     * Write Messages from GUI
     */
    [PropertyChanged.ImplementPropertyChanged]
    public class Chat {

        private static Chat _Instance;
        public static Chat Instance {
            get {
                if (_Instance == null)
                    _Instance = new Chat();

                return _Instance;
            }
            private set {
                _Instance = value;
            }
        }

        public ObservableCollection<Models.Chat.Messages> Messages { get; set; }
        public ObservableCollection<Models.Chat.Viewers> Viewers { get; set; }
        public Models.Chat.Viewers SelectedViewer { get; set; }

        public Chat() {
            if (_Instance != null) return;

            Messages = new ObservableCollection<Models.Chat.Messages>();
            Viewers = new ObservableCollection<Models.Chat.Viewers>();
            Messages.CollectionChanged += MessagesCountCheck;
            Core.AivaClient.Instance.AivaTwitchClient.OnMessageReceived += ChatMessageReceived;
            Core.AivaClient.Instance.AivaTwitchClient.OnModeratorsReceived += ModeratorsReceived;
            Core.AivaClient.Instance.AivaTwitchClient.OnUserJoined += UserJoined;
            Core.AivaClient.Instance.AivaTwitchClient.OnExistingUsersDetected += ExistingUsers;
            Core.AivaClient.Instance.AivaTwitchClient.OnUserLeft += RemoveViewerFromViewers;

            _Instance = this;
        }

        /// <summary>
        /// Check if a joined
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModeratorsReceived(object sender, OnModeratorsReceivedArgs e) {
            var matches = Viewers.Where(v => e.Moderators.Contains(v.Name)).ToList();

            foreach (var match in matches) {
                match.IsMod = true;
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
        /// Fires when existing Users detected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExistingUsers(object sender, OnExistingUsersDetectedArgs e) {
            foreach (var user in e.Users) {
                var twitchUser = TwitchLib.TwitchApi.Subscriptions.ChannelHasUserSubscribed(user, Core.AivaClient.Instance.Channel);

                Core.AivaClient.Instance.AivaTwitchClient.GetChannelModerators(Core.AivaClient.Instance.Channel);

                Application.Current.Dispatcher.Invoke(() => {
                    Viewers.Add(
                        new Models.Chat.Viewers {
                            Name = user,
                            IsSub = twitchUser != null ? true : false,
                            // UserType >= 1 -> Mod or greater
                        });
                });
            }
        }

        /// <summary>
        /// Fires when User joined
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserJoined(object sender, OnUserJoinedArgs e) {
            var twitchUser = TwitchLib.TwitchApi.Subscriptions.ChannelHasUserSubscribed(e.Username, Core.AivaClient.Instance.Channel);

            Core.AivaClient.Instance.AivaTwitchClient.GetChannelModerators(Core.AivaClient.Instance.Channel);

            Application.Current.Dispatcher.Invoke(() => {
                Viewers.Add(
                    new Models.Chat.Viewers {
                        Name = e.Username,
                        IsSub = twitchUser != null ? true : false,
                    });
            });
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
            var AddModel = new Models.Chat.Messages {
                IsUserMod = e.ChatMessage.IsModerator,
                IsUserSub = e.ChatMessage.IsSubscriber,
                TwitchID = Convert.ToInt64(e.ChatMessage.UserId),
                Username = e.ChatMessage.Username,
                Message = e.ChatMessage.Message,
                TimeStamp = DateTime.Now
            };

            Application.Current.Dispatcher.Invoke(() => {
                Messages.Add(AddModel);
            });


            // Save in Database
            StoreIndatabase(AddModel);
        }

        /// <summary>
        /// Store Message in Database
        /// </summary>
        /// <param name="AddModel"></param>
        private static void StoreIndatabase(Models.Chat.Messages AddModel) {
            using (var context = new Core.Storage.StorageEntities()) {
                context.Chat.Add(
                    new Core.Storage.Chat {
                        TwitchID = AddModel.TwitchID,
                        ChatMessage = AddModel.Message,
                        Timestamp = AddModel.TimeStamp,
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
            StoreIndatabase(new Models.Chat.Messages {
                TimeStamp = DateTime.Now,
                TwitchID = Core.AivaClient.Instance.TwitchID,
                IsUserMod = true,
                IsUserSub = false,
                Message = message,
                Username = Core.AivaClient.Instance.Username
            });
        }

        /// <summary>
        /// Put the self written Message into the Messageslist
        /// </summary>
        /// <param name="message"></param>
        private void PutMessageInList(string message) {
            Messages.Add(
                            new Models.Chat.Messages {
                                IsUserMod = true,
                                IsUserSub = false,
                                TimeStamp = DateTime.Now,
                                Message = message,
                                TwitchID = Core.AivaClient.Instance.TwitchID,
                                Username = Core.AivaClient.Instance.Username
                            });
        }
    }
}
