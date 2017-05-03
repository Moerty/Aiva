using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TwitchLib.Events.Client;

namespace Aiva.Extensions.Songrequest {
    [PropertyChanged.ImplementPropertyChanged]
    public class SongrequestHandler {
        /*
         * 
         * - Setting who can request songs
            - max song duration
            - max quene size
    
            - cost sttings
    
            - add playlist
            - add video
    */

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
            //CefSharp.Cef.Initialize();
            Player = new Player();
            Player.Autoplay = Autoplay;
        }

        public void EnableSongrequest() {
            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived += OnSongrequestCommandReceived;
        }

        public void DisableSongrequest() {
            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived -= OnSongrequestCommandReceived;
        }

        private void OnSongrequestCommandReceived(object sender, OnChatCommandReceivedArgs e) {
            if (String.Compare(e.Command.Command, Command, true) == 0 || String.Compare(e.Command.Command, Command.TrimStart('!'), true) == 0) {
                AddSong(e.Command.ArgumentsAsString, e.Command.ChatMessage.Username, Convert.ToInt64(e.Command.ChatMessage.UserId));
            }
        }

        public void AddSong(string argument, string username, long userid) {

            var song = new Song(argument, username) {
                TwitchID = userid
            };

            Application.Current.Dispatcher.Invoke(() => {
                Player.SongList.Add(song);
            });
        }

        public void AddPlaylist(string addPlaylistUrl, string username, long twitchID) {
            var SongList = new Playlist(addPlaylistUrl).GetSongListFromPlaylist();

            foreach (var song in SongList) {
                Application.Current.Dispatcher.Invoke(() => {
                    Player.SongList.Add(song);
                });
            }
        }

        public void StartSong(Song song) {
            Player.ChangeSong(song, Autoplay);
        }

        public void StopSong() {
            Player.StartStopMusic();
        }

        public void AddPlaylist(string userinput) {

        }

        /// <summary>
        /// Send Message to Chat
        /// </summary>
        /// <param name="text"></param>
        public void SendStartSongMessage(string text) {
            Core.AivaClient.Instance.AivaTwitchClient.SendMessage(text);
        }
    }
}
