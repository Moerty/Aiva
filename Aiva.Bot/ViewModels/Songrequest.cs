using Aiva.Core;
using Aiva.Extensions.Songrequest;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class Songrequest {

        public ICommand AddCommand { get; set; }
        public ICommand PlaySongCommand { get; set; }
        public ICommand SetSongrequestInactiveCommand { get; set; }


        public string AddYoutubeUrl { get; set; }
        public string AddPlaylistUrl { get; set; }

        public SongrequestHandler Handler { get; set; }


        public Songrequest() {
            Handler = new SongrequestHandler();


            var type = new MetroContentControl().GetType();
            AddCommand = new Internal.RelayCommand(add => AddSongToPlaylist(), add => !String.IsNullOrEmpty(AddYoutubeUrl) || !String.IsNullOrEmpty(AddPlaylistUrl));
            //SetSongrequestActiveCommand = new Internal.RelayCommand(active => SetRequestActive(), active => true);
            //SetSongrequestActiveCommand = new Internal.RelayCommand(inactive => SetRequestInactive(), inactive => true);
            PlaySongCommand = new Internal.RelayCommand(p => PlaySong(), p => Handler.Player.SongList.Any());
        }

        /// <summary>
        /// Disable Songrequest
        /// </summary>
        private void SetRequestInactive() => Handler.DisableSongrequest();

        /// <summary>
        /// Enable Songrequest
        /// </summary>
        private void SetRequestActive() {
            if (!String.IsNullOrEmpty(Handler.Command))
                Handler.EnableSongrequest();
            else {
                Handler.IsEnabled = false;
            }
        }

        /// <summary>
        /// Add Song to Playlist
        /// </summary>
        private void AddSongToPlaylist() {
            if (!String.IsNullOrEmpty(AddYoutubeUrl)) {
                Handler.AddSong(AddYoutubeUrl, Core.AivaClient.Instance.Username, Core.AivaClient.Instance.TwitchID);
            } else {
                Handler.AddPlaylist(AddPlaylistUrl, Core.AivaClient.Instance.Username, Core.AivaClient.Instance.TwitchID);
            }
        }

        /// <summary>
        /// Play clicked song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaySong() {
            //var Song = (Extensions.Songrequest.Song)(sender as Views.Songrequest).listView.SelectedItem;
            var Song = Handler.Player.SelectedSong;


            // Handle DoubleClick cause Pause
            var currentSong = Handler.Player.SongList.SingleOrDefault(x => x.IsPlaying);
            if (currentSong != null) {
                if (Song.VideoID == currentSong.VideoID) {
                    currentSong.IsPlaying = false;
                    Handler.Player.StartStopMusic();


                    return;
                }
            }

            if (Song != null) {

                //Player.ChangeSong(Song.VideoID);
                Player.Instance.ChangeSong(Song, Handler.Autoplay);

                // Inform User
                Handler.SendStartSongMessage($"Start Song \"{Song.Title}\". Gewünscht von @{Song.Requester}. Link: {Song.Url}");

                //SongList.ToList().ForEach(x => x.IsPlaying = false);
                Handler.Player.SongList.ToList().ForEach(x => x.IsPlaying = false);

                Song.IsPlaying = true;

            }
        }
    }
}
