using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Songrequest;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Timers;

namespace AivaBot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    class SongrequestViewModel {
        public bool Active { get; set; }
        public string Command { get; set; }
        public bool RepeatInChat { get; set; } = false;
        public TimeSpan RepeatTime { get; set; } = new TimeSpan(0, 5, 0);

        public ICommand StartCommand { get; set; } = new RoutedCommand();
        public ICommand StopCommand { get; set; } = new RoutedCommand();
        public ICommand PlaySong { get; set; } = new RoutedCommand();
        public ICommand StopSong { get; set; } = new RoutedCommand();
        public ICommand DeleteCommand { get; set; } = new RoutedCommand();
        public ICommand CopyLinkCommand { get; set; } = new RoutedCommand();
        public ICommand HonorCommand { get; set; } = new RoutedCommand();

        public Models.SongrequestModel Model;
        public PlaylistHandler Playlist = new Songrequest.PlaylistHandler(Properties.Settings.Default.GoogleClientID, Properties.Settings.Default.GoogleClientKey);
        public Models.AsyncObservableCollection<Songrequest.Song> SongList { get; set; } = new Models.AsyncObservableCollection<Song>();
        public Player Player { get; set; }

        private Timer RepeatTimer;

        public SongrequestViewModel(FrameworkElement control) {
            // CreateModels
            CreateModels();

            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(StartCommand, StartSongrequest));
            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(StopCommand, StopSongrequest));

            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(DeleteCommand, DeleteSong));
            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(CopyLinkCommand, ToClipboard));
            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(HonorCommand, StopSongrequest));

            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(PlaySong, playSong));
            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(StopSong, stopSong));
        }

        private void CreateModels() {
            Model = new Models.SongrequestModel {
                Text = new Models.SongrequestModel.TextModel {
                    SongrequestCommandWatermarkText = Config.Language.Instance.GetString("SongrequestCommandWatermarkText"),
                    SongrequestExpanderRepeatText = Config.Language.Instance.GetString("SongrequestExpanderRepeatText"),
                    SongrequestButtonStartText = Config.Language.Instance.GetString("SongrequestButtonStartText"),
                    SongrequestButtonStopText = Config.Language.Instance.GetString("SongrequestButtonStopText"),
                    SongrequestButtonStopMusicText = Config.Language.Instance.GetString("SongrequestButtonStopMusicText"),
                }
            };
        }

        private void StartSongrequest(object sender, EventArgs e) {
            Client.Client.ClientBBB.TwitchClientBBB.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;

            if (RepeatInChat) {
                if (RepeatTimer == null) {
                    RepeatTimer = new Timer();
                }
                RepeatTimer.Interval = RepeatTime.TotalMilliseconds;
                RepeatTimer.Elapsed += RepeatTimer_Elapsed;
                RepeatTimer.Start();
            }
        }

        private void RepeatTimer_Elapsed(object sender, ElapsedEventArgs e) {
            Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("SongrequestRepeatText")
                                    .Replace("@COMMAND@", Command));
        }

        private void StopSongrequest(object sender, EventArgs e) {
            Client.Client.ClientBBB.TwitchClientBBB.OnChatCommandReceived -= TwitchClient_OnChatCommandReceived;
            RepeatTimer.Stop();
        }

        private void DeleteSong(object sender, RoutedEventArgs e) {
            var song = (e.OriginalSource as ListViewItem).DataContext as Songrequest.Song;

            SongList.Remove(song);
        }

        private void ToClipboard(object sender, RoutedEventArgs e) {
            var song = (e.OriginalSource as ListViewItem).DataContext as Songrequest.Song;

            System.Windows.Forms.Clipboard.SetDataObject(song.VideoID, true);
        }

        private void Honor(object sender, RoutedEventArgs e) {
            var song = (e.OriginalSource as ListViewItem).DataContext as Songrequest.Song;

            Database.CurrencyHandler.AddCurrencyAsync(song.Username, 100);
            Client.Client.ClientBBB.TwitchClientBBB.SendMessage(song.Username + Config.Language.Instance.GetString("SongrequestHonorText"));

        }

        private void TwitchClient_OnChatCommandReceived(object sender, TwitchLib.Events.Client.OnChatCommandReceivedArgs e) {
            // Add Song
            if (String.Compare(e.Command.Command, Command, StringComparison.OrdinalIgnoreCase) == 0) {

                var Song = new Songrequest.Song(e.Command.ArgumentsAsString, e.Command.ChatMessage.Username,
                                                Properties.Settings.Default.GoogleClientID,
                                                    Properties.Settings.Default.GoogleClientKey);



                if (Song.VideoModel.VideoStatus == Songrequest.Models.ReturnModel.Completed) {
                    var entry = SongList.Where(x => x.VideoID == Song.VideoID).ToList();
                    if (entry.Count == 0) {
                        SongList.Add(Song);
                        InformUser(Models.SongrequestModel.InformUser.AddedSuccessfully, Song);
                    }
                    else {
                        Song.VideoModel.Title = entry[0].VideoModel.Title;
                        InformUser(Models.SongrequestModel.InformUser.VideoDuplicate, Song);
                    }
                }
                else {
                    InformUser(Models.SongrequestModel.InformUser.VideoNotFound, null, e.Command.ChatMessage.Username);
                }
            }

            // GetSong
            if (/*command.CommandType == ChatMessageHelper.Models.ReturnModel.returnModel.SongrequestInfo */  true) {
                if (Models.SongrequestModel.playedSong != null) {
                    Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("SongrequestGetVideoPlayed")
                                                                .Replace("@TITLE@", Models.SongrequestModel.playedSong.VideoModel.Title)
                                                                .Replace("@LINK@", Models.SongrequestModel.playedSong.VideoModel.Url));
                }
            }
        }


        /// <summary>
        /// Send ChatMessage to User if Song is successfully added to List
        /// </summary>
        /// <param name="type"></param>
        /// <param name="song"></param>
        /// <param name="username"></param>
        private void InformUser(Models.SongrequestModel.InformUser type, Songrequest.Song song = null, string username = null) {
            switch (type) {
                case Models.SongrequestModel.InformUser.AddedSuccessfully:
                    Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("SongrequestSongAddedToPlaylist")
                                                            .Replace("@TITLE@", song.VideoModel.Title)
                                                            .Replace("@USERNAME@", song.Username));
                    break;

                case Models.SongrequestModel.InformUser.VideoDuplicate:
                    Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("SongrequestVideoAlreadyExist")
                                                            .Replace("@TITLE@", song.VideoModel.Title)
                                                            .Replace("@USERNAME@", song.Username));
                    break;

                case Models.SongrequestModel.InformUser.VideoNotFound:
                    Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("SongrequestVideoNotExist")
                                                           .Replace("@USERNAME@", song.Username));
                    break;
                case Models.SongrequestModel.InformUser.NotActivated:
                    Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("SongrequestNotActivated")
                                                            .Replace("@USERNAME@", username));
                    break;
                default:
                    Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("SongrequestNotDefinedError")
                                                            .Replace("@USERNAME@", song.Username));
                    break;
            }
        }

        private void playSong(object sender, ExecutedRoutedEventArgs e) {
            var Song = (Songrequest.Song)(sender as Views.Songrequest).listView.SelectedItem;
            if (Player == null) Player = new Player(true);

            // Handle DoubleClick cause Pause
            var currentSong = SongList.SingleOrDefault(x => x.IsPlaying);
            if (currentSong != null) {
                if (Song.VideoID == currentSong.VideoID) {
                    currentSong.IsPlaying = false;
                    Player.MusicOnOff();
                    Models.SongrequestModel.playedSong = null;
                    return;
                }
            }

            if (Song != null) {
                if (Player.IsInit) {
                    Player.ChangeSong(Song.VideoID);

                    // Inform User
                    Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Config.Language.Instance.GetString("SongrequestSongPlayed")
                                                        .Replace("@USERNAME@", Song.Username)
                                                        .Replace("@TITLE@", Song.VideoModel.Title));

                    SongList.ToList().ForEach(x => x.IsPlaying = false);

                    Song.IsPlaying = true;
                    Models.SongrequestModel.playedSong = Song;
                }
            }
        }

        private void stopSong(object sender, ExecutedRoutedEventArgs e) {
            if (Player != null)
                Player.MusicOnOff();

            var Song = (Songrequest.Song)(sender as Views.Songrequest).listView.SelectedItem;
            if (Song != null) {
                Song.IsPlaying = false;
                Models.SongrequestModel.playedSong = null;
            }
        }
    }
}
