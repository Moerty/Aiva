using Aiva.Core.Twitch;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using TwitchLib.Client.Events;

namespace Aiva.Extensions.Songrequest {
    public class Handler {
        #region Models
        public Models.Songrequest.Properties Properties { get; set; }

        public EventHandler<Models.Songrequest.Song> OnNewSong;

        private readonly Core.Database.Handlers.Currency _currencyDatabaseHandler;
        private bool _isListning;

        #endregion Models

        #region Constructor

        public Handler() {
            _currencyDatabaseHandler = new Core.Database.Handlers.Currency();
        }

        #endregion Constructor

        #region Functions

        /// <summary>
        /// Stop the registration
        /// </summary>
        public void StopRegistration() {
            AivaClient.Instance.TwitchClient.OnChatCommandReceived -= AddSongCommandReceived;
            _isListning = false;
        }

        public void StartRegistration() {
            AivaClient.Instance.TwitchClient.OnChatCommandReceived += AddSongCommandReceived;
            _isListning = true;
        }

        /// <summary>
        /// Fires when a command was received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddSongCommandReceived(object sender, OnChatCommandReceivedArgs e) {
            if (String.Compare(Properties.Command.TrimStart('!'), e.Command.CommandText, true) == 0) {
                // price
                if (Properties.IsCostEnabled) {
                    if (!_currencyDatabaseHandler.HasUserEnoughCurrency(
                        twitchID: Convert.ToInt32(e.Command.ChatMessage.UserId),
                        currencyToCheck: Properties.Cost)) {
                        return;
                    }
                }

                // permissions
                if (Properties.JoinPermission != Models.Enums.JoinPermission.Everyone) {
                    if (Properties.JoinPermission == Models.Enums.JoinPermission.Subscriber) {
                        if (!e.Command.ChatMessage.IsSubscriber) {
                            return;
                        }
                    }

                    if (Properties.JoinPermission == Models.Enums.JoinPermission.Moderation) {
                        if (!e.Command.ChatMessage.IsModerator) {
                            return;
                        }
                    }
                }

                // follower
                if (Properties.BeFollower) {
                    var followerCheck = await AivaClient.Instance.TwitchApi.Users.v5.UserFollowsChannelAsync(e.Command.ChatMessage.UserId, AivaClient.Instance.ChannelId).ConfigureAwait(false);

                    if (!followerCheck) {
                        return;
                    }
                }

                // get song model
                var songModel = await GenerateSongModel(e.Command.ArgumentsAsString, e.Command.ChatMessage.DisplayName, Convert.ToInt32(e.Command.ChatMessage.UserId)).ConfigureAwait(false);

                // remove currency from user
                if (Properties.IsCostEnabled) {
                    _currencyDatabaseHandler.Remove.Remove(
                        Convert.ToInt32(e.Command.ChatMessage.UserId),
                        Properties.Cost);
                }

                // call song found
                OnNewSong?.Invoke(this, songModel);
            }
        }

        public void ChangeProperties(Models.Songrequest.Properties addModel) {
            if (_isListning) {
                StopRegistration();
                Properties = addModel;
                StartRegistration();
            } else {
                Properties = addModel;
            }
        }

        public async void AddSong(string url) {
            var songModel = await GenerateSongModel(
                url,
                Core.Config.Config.Instance.Storage.General.BotName,
                Convert.ToInt32(Core.Config.Config.Instance.Storage.General.BotUserID)).ConfigureAwait(false);

            if (songModel != null) {
                OnNewSong?.Invoke(this, songModel);
            }
        }

        private async Task<Models.Songrequest.Song> GenerateSongModel(string argumentsAsString, string displayName, int userId) {
            if (IsVideoId(argumentsAsString, out string videoid)) {
                var songModel = await new YouTubeInfo(videoid).GetVideoDetails().ConfigureAwait(false);

                if (songModel != null) {
                    songModel.Requester = displayName;
                    songModel.RequesterID = userId;
                    songModel.Url = $"https://www.youtube.com/watch?v={videoid}";

                    return songModel;
                } else {
                    return default(Models.Songrequest.Song);
                }
            } else {
                return null;
            }
        }

        /// <summary>
        /// Extract the VideoID
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="videoId"></param>
        /// <returns></returns>
        private bool IsVideoId(string userInput, out string videoId) {
            // https://www.youtube.com/watch?v=ARfqiQRSPFc
            if (userInput.StartsWith("https://www.youtube.com/watch")) {
                var query = HttpUtility.ParseQueryString(new Uri(userInput).Query);
                if (query?.AllKeys?.Contains("v") == true) {
                    videoId = query["v"];
                    return true;
                } else {
                    videoId = string.Empty;
                    return false;
                }
            }

            // https://youtu.be/5ZECzjIyUOg
            if (userInput.StartsWith("https://youtu.be/") && userInput.Length >= 28) {
                videoId = userInput.Substring(17, 11);
                return true;
            }

            if (userInput.StartsWith("/watch?v=") && userInput.Length >= 20) {
                videoId = userInput.Substring(9, userInput.Length - 9);
                return true;
            }

            // hole video id
            var result = CheckHeaderResponse(userInput);

            if (result) {
                videoId = userInput;
                return true;
            } else {
                videoId = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// check youtube headers
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        private bool CheckHeaderResponse(string userInput) {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create($"https://youtu.be/{userInput}");
            request.Method = "HEAD";
            try {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                    if (response.ResponseUri.ToString().Contains("youtube.com")) {
                        return true;
                    }
                }
            }
            catch {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Checks if the user has enough currency
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool UserHasEnoughCurrency(int id) {
            var currency = _currencyDatabaseHandler.GetCurrency(id);

            if (currency.HasValue) {
                if (currency.Value >= Properties.Cost) {
                    return true;
                }
            }

            return false;
        }

        #endregion Functions
    }
}