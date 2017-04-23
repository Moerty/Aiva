using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Timers;
using Aiva.Extensions.Songrequest;
using Aiva.Core.Config;
using Aiva.Core.Client;
using Aiva.Core.Database;
using Aiva.Extensions.Models.Songrequest;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    class SongrequestViewModel {
        public bool Active { get; set; }
        public string Command { get; set; }
        public bool RepeatInChat { get; set; } = false;
        public TimeSpan RepeatTime { get; set; } = new TimeSpan(0, 5, 0);

        public ICommand StartCommand { get; set; } = new RoutedCommand();
        public ICommand StopCommand { get; set; } = new RoutedCommand();
        public ICommand PlaySongCommand { get; set; } = new RoutedCommand();
        public ICommand StopSongCommand { get; set; } = new RoutedCommand();
        public ICommand DeleteCommand { get; set; } = new RoutedCommand();
        public ICommand CopyLinkCommand { get; set; } = new RoutedCommand();
        public ICommand HonorCommand { get; set; } = new RoutedCommand();

        public Models.SongrequestModel Model;
        public Models.AsyncObservableCollection<Song> SongList { get; set; } = new Models.AsyncObservableCollection<Song>();
        public Player Player { get; set; }
        private Timer RepeatTimer;

        /// <summary>
        /// Create ViewModel
        /// </summary>
        /// <param name="control"></param>
        public SongrequestViewModel(FrameworkElement control) {
            // CreateModels
            CreateModels();

            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(StartCommand, StartSongrequest));
            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(StopCommand, StopSongrequest));

            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(DeleteCommand, DeleteSong));
            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(CopyLinkCommand, ToClipboard));
            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(HonorCommand, Honor));

            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(PlaySongCommand, PlaySong));
            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(StopSongCommand, StopSong));
        }

        /// <summary>
        /// Create Models
        /// </summary>
        private void CreateModels() {
            Model = new Models.SongrequestModel {
                Text = new Models.SongrequestModel.TextModel {
                    SongrequestCommandWatermarkText = LanguageConfig.Instance.GetString("SongrequestCommandWatermarkText"),
                    SongrequestExpanderRepeatText = LanguageConfig.Instance.GetString("SongrequestExpanderRepeatText"),
                    SongrequestButtonStartText = LanguageConfig.Instance.GetString("SongrequestButtonStartText"),
                    SongrequestButtonStopText = LanguageConfig.Instance.GetString("SongrequestButtonStopText"),
                    SongrequestButtonStopMusicText = LanguageConfig.Instance.GetString("SongrequestButtonStopMusicText"),
                }
            };
        }

        /// <summary>
        /// Start Songrequest & timer if activated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSongrequest(object sender, EventArgs e) {
            AivaClient.Client.AivaTwitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;

            if (RepeatInChat) {
                if (RepeatTimer == null) {
                    RepeatTimer = new Timer();
                }
                RepeatTimer.Interval = RepeatTime.TotalMilliseconds;
                RepeatTimer.Elapsed += RepeatTimer_Elapsed;
                RepeatTimer.AutoReset = true;
                RepeatTimer.Start();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeatTimer_Elapsed(object sender, ElapsedEventArgs e) {
            AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("SongrequestRepeatText")
                                    .Replace("@COMMAND@", Command));
        }

        /// <summary>
        /// Stop the songrequest
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopSongrequest(object sender, EventArgs e) {
            AivaClient.Client.AivaTwitchClient.OnChatCommandReceived -= TwitchClient_OnChatCommandReceived;
            RepeatTimer.Stop();
        }

        /// <summary>
        /// Delete Song from list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSong(object sender, RoutedEventArgs e) {
            var song = (e.OriginalSource as ListViewItem).DataContext as Song;

            SongList.Remove(song);
        }

        /// <summary>
        /// Copy song to Clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToClipboard(object sender, RoutedEventArgs e) {
            var song = (e.OriginalSource as ListViewItem).DataContext as Song;

            System.Windows.Forms.Clipboard.SetDataObject(song.VideoID, true);
        }

        /// <summary>
        /// Honor requester with 100 currency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Honor(object sender, RoutedEventArgs e) {
            var song = (e.OriginalSource as ListViewItem).DataContext as Song;

            CurrencyHandler.AddCurrencyAsync(song.Username, 100);
            AivaClient.Client.AivaTwitchClient.SendMessage(song.Username + LanguageConfig.Instance.GetString("SongrequestHonorText"));
        }

        /// <summary>
        /// Add song when command received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwitchClient_OnChatCommandReceived(object sender, TwitchLib.Events.Client.OnChatCommandReceivedArgs e) {
            // Add Song
            if (String.Compare(e.Command.Command, Command, StringComparison.OrdinalIgnoreCase) == 0) {

                var Song = new Song(e.Command.ArgumentsAsString, e.Command.ChatMessage.Username,
                                                Properties.Settings.Default.GoogleClientID,
                                                    Properties.Settings.Default.GoogleClientKey);



                if (Song.VideoModel.VideoStatus == ReturnModel.Completed) {
                    var entry = SongList.Where(x => x.VideoID == Song.VideoID).ToList();
                    if (entry.Count == 0) {
                        SongList.Add(Song);
                        InformUser(Models.SongrequestModel.InformUser.AddedSuccessfully, Song);
                    } else {
                        Song.VideoModel.Title = entry[0].VideoModel.Title;
                        InformUser(Models.SongrequestModel.InformUser.VideoDuplicate, Song);
                    }
                } else {
                    InformUser(Models.SongrequestModel.InformUser.VideoNotFound, null, e.Command.ChatMessage.Username);
                }
            }

            // GetSong
            if (/*command.CommandType == ChatMessageHelper.Models.ReturnModel.returnModel.SongrequestInfo */  true) {
                if (Models.SongrequestModel.playedSong != null) {
                    AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("SongrequestGetVideoPlayed")
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
        private void InformUser(Models.SongrequestModel.InformUser type, Song song = null, string username = null) {
            switch (type) {
                case Models.SongrequestModel.InformUser.AddedSuccessfully:
                    AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("SongrequestSongAddedToPlaylist")
                                                            .Replace("@TITLE@", song.VideoModel.Title)
                                                            .Replace("@USERNAME@", song.Username));
                    break;

                case Models.SongrequestModel.InformUser.VideoDuplicate:
                    AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("SongrequestVideoAlreadyExist")
                                                            .Replace("@TITLE@", song.VideoModel.Title)
                                                            .Replace("@USERNAME@", song.Username));
                    break;

                case Models.SongrequestModel.InformUser.VideoNotFound:
                    AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("SongrequestVideoNotExist")
                                                           .Replace("@USERNAME@", song.Username));
                    break;
                case Models.SongrequestModel.InformUser.NotActivated:
                    AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("SongrequestNotActivated")
                                                            .Replace("@USERNAME@", username));
                    break;
                default:
                    AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("SongrequestNotDefinedError")
                                                            .Replace("@USERNAME@", song.Username));
                    break;
            }
        }

        /// <summary>
        /// Play clicked song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaySong(object sender, ExecutedRoutedEventArgs e) {
            var Song = (Song)(sender as Views.Songrequest).listView.SelectedItem;
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
                    AivaClient.Client.AivaTwitchClient.SendMessage(LanguageConfig.Instance.GetString("SongrequestSongPlayed")
                                                        .Replace("@USERNAME@", Song.Username)
                                                        .Replace("@TITLE@", Song.VideoModel.Title));

                    SongList.ToList().ForEach(x => x.IsPlaying = false);

                    Song.IsPlaying = true;
                    Models.SongrequestModel.playedSong = Song;
                }
            }
        }

        /// <summary>
        /// Stop song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopSong(object sender, ExecutedRoutedEventArgs e) {
            if (Player != null)
                Player.MusicOnOff();

            var Song = (Song)(sender as Views.Songrequest).listView.SelectedItem;
            if (Song != null) {
                Song.IsPlaying = false;
                Models.SongrequestModel.playedSong = null;
            }
        }
    }
}
