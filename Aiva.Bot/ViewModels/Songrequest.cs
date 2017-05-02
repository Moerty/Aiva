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
        

        private bool _IsEnabled;
        public bool IsEnabled {
            get {
                return _IsEnabled;
            }
            set {
                _IsEnabled = value;

                if(value) {
                    //Handler.EnableSongrequest(Command);
                }
                else {
                    //Handler.DisableSongrequest();
                }
            }
        }

        public ICommand AddCommand { get; set; }

        
        public string Command { get; set; }
        public string AddYoutubeUrl { get; set; }
        public string AddPlaylistUrl { get; set; }

        Extensions.Songrequest.SongrequestHandler Handler { get; set; }


        public Songrequest() {



            var type = new MetroContentControl().GetType();
            AddCommand = new Internal.RelayCommand(add => AddSongToPlaylist(), add => !String.IsNullOrEmpty(AddYoutubeUrl) || !String.IsNullOrEmpty(AddPlaylistUrl));
        }

        private void AddSongToPlaylist()
        {
            if(!String.IsNullOrEmpty(AddYoutubeUrl)) {
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
        private void PlaySong(object sender, ExecutedRoutedEventArgs e)
        {
            var Song = (Extensions.Songrequest.Song)(sender as Views.Songrequest).listView.SelectedItem;
            

            // Handle DoubleClick cause Pause
            var currentSong = Handler.Player.SongList.SingleOrDefault(x => x.IsPlaying);
            if (currentSong != null) {
                if (Song.VideoID == currentSong.VideoID) {
                    currentSong.IsPlaying = false;
                    Handler.Player.StartStopMusic();
                    Models.SongrequestModel.playedSong = null;
                    return;
                }
            }

            if (Song != null) {
                if (Player.IsInit) {
                    Player.ChangeSong(Song.VideoID);

                    // Inform User
                    AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("SongrequestSongPlayed")
                                                        .Replace("@USERNAME@", Song.Username)
                                                        .Replace("@TITLE@", Song.VideoModel.Title));

                    SongList.ToList().ForEach(x => x.IsPlaying = false);

                    Song.IsPlaying = true;
                    Models.SongrequestModel.playedSong = Song;
                }
            }
        }
    }
}
