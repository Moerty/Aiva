using Aiva.Extensions.Songrequest;
using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class Songrequest {

        public ICommand AddCommand { get; set; }
        public ICommand PlaySongCommand { get; set; }
        public ICommand HonorRequesterCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public string AddYoutubeUrl { get; set; }
        public string AddPlaylistUrl { get; set; }

        public SongrequestHandler Handler { get; set; }

        public Songrequest() {
            Handler = new SongrequestHandler();

            var type = new MetroContentControl().GetType();
            AddCommand = new Internal.RelayCommand(add => AddSongToPlaylist(), add => !String.IsNullOrEmpty(AddYoutubeUrl) || !String.IsNullOrEmpty(AddPlaylistUrl));
            PlaySongCommand = new Internal.RelayCommand(p => PlaySong(), p => Handler.Player.SongList.Any());
            HonorRequesterCommand = new Internal.RelayCommand(h => HonorRequester(), p => Handler.Player.SelectedSong != null);
            DeleteCommand = new Internal.RelayCommand(d => DeleteSong(), d => Handler.Player.SelectedSong != null);
        }

        /// <summary>
        /// Delete selected Song
        /// </summary>
        private void DeleteSong() => Handler.Player.DeleteSongFromPlaylist();

        /// <summary>
        /// Open HonorRequester Flyout
        /// </summary>
        private void HonorRequester() {

            MainWindow.Instance.SelectedTab.Flyouts[0].DataContext = new Flyouts.HonorSongrequester(Handler.Player.SelectedSong.Requester, Handler.Player.SelectedSong.TwitchID);
            MainWindow.Instance.SelectedTab.Flyouts[0].IsOpen = true;
        }

        /// <summary>
        /// Add Song to Playlist
        /// </summary>
        private void AddSongToPlaylist() {
            if (!String.IsNullOrEmpty(AddYoutubeUrl)) {
                Handler.AddSong(AddYoutubeUrl, Core.AivaClient.Instance.Username, Core.AivaClient.Instance.TwitchID);
                AddYoutubeUrl = String.Empty;
            } else {
                Handler.AddPlaylist(AddPlaylistUrl);
                AddPlaylistUrl = String.Empty;
            }
        }

        /// <summary>
        /// Play clicked song
        /// </summary>
        private void PlaySong() {

            Handler.Player.PlayedSong = Handler.Player.SelectedSong;

            if (Handler.Player.PlayedSong != null) {

                // Inform User
                SongrequestHandler.SendStartSongMessage
                    ($"Start Song \"{Handler.Player.PlayedSong.Title}\". Desired by @{Handler.Player.PlayedSong.Requester}. Link: {Handler.Player.PlayedSong.Url}");

                //SongList.ToList().ForEach(x => x.IsPlaying = false);
                Handler.Player.SongList.ToList().ForEach(x => x.IsPlaying = false);

                Handler.Player.PlayedSong.IsPlaying = true;

            }
        }
    }
}
