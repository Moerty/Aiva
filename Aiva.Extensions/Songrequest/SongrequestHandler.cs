using System;
using System.Windows;
using TwitchLib.Events.Client;

namespace Aiva.Extensions.Songrequest {
    [PropertyChanged.ImplementPropertyChanged]
    public class SongrequestHandler {

        private bool _IsEnabled;
        public bool IsEnabled {
            get {
                return _IsEnabled;
            }
            set {
                if (!String.IsNullOrEmpty(Command)) {
                    if (value) {
                        EnableSongrequest();
                    } else {
                        DisableSongrequest();
                    }
                    _IsEnabled = value;
                }
            }
        }

        public Player Player { get; private set; }
        public bool Autoplay { get; set; } = true;
        public string Command { get; set; }
        public TwitchLib.Enums.UserType UserType { get; set; }

        public SongrequestHandler() {
            Player = new Player();
        }

        /// <summary>
        /// Enabled Songrequest
        /// </summary>
        public void EnableSongrequest() {
            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived += OnSongrequestCommandReceived;
        }

        /// <summary>
        /// Disable Songrequest
        /// </summary>
        public void DisableSongrequest() {
            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived -= OnSongrequestCommandReceived;
        }

        /// <summary>
        /// Fires when Songrequest Command received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSongrequestCommandReceived(object sender, OnChatCommandReceivedArgs e) {
            if (String.Compare(e.Command.Command, Command, true) == 0 || String.Compare(e.Command.Command, Command.TrimStart('!'), true) == 0) {
                AddSong(e.Command.ArgumentsAsString, e.Command.ChatMessage.Username, Convert.ToInt64(e.Command.ChatMessage.UserId));
            }
        }

        /// <summary>
        /// Add a Song to the Playlist
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="username"></param>
        /// <param name="userid"></param>
        public void AddSong(string argument, string username, long userid) {

            var song = new Song(argument, username) {
                TwitchID = userid
            };

            Application.Current.Dispatcher.Invoke(() => {
                Player.SongList.Add(song);
            });
        }

        /// <summary>
        /// Add a Playlist from YouTube to the internal Playlist
        /// </summary>
        /// <param name="addPlaylistUrl"></param>
        public void AddPlaylist(string addPlaylistUrl) {
            var SongList = new Playlist(addPlaylistUrl).GetSongListFromPlaylist();

            foreach (var song in SongList) {
                Application.Current.Dispatcher.Invoke(() => {
                    Player.SongList.Add(song);
                });
            }
        }

        /// <summary>
        /// Send Message to Chat
        /// </summary>
        /// <param name="text"></param>
        public static void SendStartSongMessage(string text) {
            Core.AivaClient.Instance.AivaTwitchClient.SendMessage(text);
        }
    }
}
