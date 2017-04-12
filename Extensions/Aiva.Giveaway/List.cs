using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TwitchLib.Events.Client;

namespace Giveaway {
    [PropertyChanged.ImplementPropertyChanged]
    public class List {
        public ObservableCollection<Models.UsersModel> UserList { get; protected set; }
        private Models.GiveawayModel Model;

        public List(Models.GiveawayModel _model) {
            UserList = new ObservableCollection<Models.UsersModel>();
            Model = _model;
        }

        public Models.ReturnModel AddUserToList(OnChatCommandReceivedArgs message) {
            // Check if Command is from Sub and add that;
            if (message.Command.ChatMessage.Subscriber && Model.Sub && Model.Admin) {
                AddUserToList(message.Command.ChatMessage.Username);

                return Models.ReturnModel.Successfull;
            }

            // Check other posibilities
            switch (message.Command.ChatMessage.UserType) {
                case TwitchLib.Enums.UserType.Broadcaster:
                    AddUserToList(message.Command.ChatMessage.Username);
                    return Models.ReturnModel.Successfull;
                case TwitchLib.Enums.UserType.Admin:
                    if (Model.Admin) {
                        AddUserToList(message.Command.ChatMessage.Username);
                        return Models.ReturnModel.Successfull;
                    }
                    break;
                case TwitchLib.Enums.UserType.GlobalModerator:
                case TwitchLib.Enums.UserType.Moderator:
                    if (Model.Mod) {
                        AddUserToList(message.Command.ChatMessage.Username);
                        return Models.ReturnModel.Successfull;
                    }
                    break;
                case TwitchLib.Enums.UserType.Viewer:
                    if (Model.User) {
                        AddUserToList(message.Command.ChatMessage.Username);
                        return Models.ReturnModel.Successfull;
                    }
                    break;
            }
            return Models.ReturnModel.OtherError;
        }

        private void AddUserToList(string username) {
            if (UserList.SingleOrDefault(x => string.Compare(x.Username, username, true) == 0) == null) {
                Application.Current.Dispatcher.Invoke(() => {
                    UserList.Add(
                        new Models.UsersModel {
                            Username = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(username),
                            IsSub = IsUserSub(username),
                        }
                    );
                });
            }
        }

        /// <summary>
        /// Generate Int and get User with generated int in list
        /// </summary>
        /// <returns>Winenr UserModel</returns>
        public Models.UsersModel GetWinner() {
            List<string> nameList = new List<string>();

            foreach (var user in UserList) {
                nameList.Add(user.Username);
                if (user.IsSub) {
                    for (int i = 0; i < Model.SubLuck; i++) {
                        nameList.Add(user.Username);
                    }
                }
            }

            string Winner = nameList.ElementAt(new Random().Next(nameList.Count));
            return UserList.SingleOrDefault(x => string.Compare(x.Username, Winner) == 0);
        }

        private bool IsUserSub(string username) {
            return TwitchLib.TwitchApi.Subscriptions.ChannelHasUserSubscribed(username, Client.Client.ClientBBB.Channel) != null;
        }
    }
}