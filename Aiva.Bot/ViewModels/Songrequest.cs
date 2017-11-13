using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Songrequest {
        #region Models

        public ICommand StartSongrequestCommand { get; set; }
        public ICommand StopSongrequestCommand { get; set; }
        public ICommand AddSongCommand { get; set; }
        public ICommand ResetSongrequestCommand { get; set; }
        public ICommand NextSongCommand { get; set; }
        public ICommand PlaySongCommand { get; set; }

        public Extensions.Songrequest.Handler Handler { get; set; }

        #endregion Models

        #region Constructor

        public Songrequest() {
            SetCommands();
        }

        private void SetCommands() {
            StartSongrequestCommand = new Internal.RelayCommand(start => StartSongrequestSetup());
            StopSongrequestCommand = new Internal.RelayCommand(stop => StopRegisterSongrequest());
            AddSongCommand = new Internal.RelayCommand(add => AddSong(), add => Handler != null);
            ResetSongrequestCommand = new Internal.RelayCommand(reset => Handler = null);
            NextSongCommand = new Internal.RelayCommand(next => NextSong(), next => Handler?.SongList.Count > 0);
            PlaySongCommand = new Internal.RelayCommand(play => PlaySong());
        }

        private void PlaySong() => Handler.PlaySelectedSong();

        /// <summary>
        /// Plays the next song
        /// </summary>
        private void NextSong() => Handler.NextSong();

        #endregion Constructor

        #region Functions

        /// <summary>
        /// start the window to add a song
        /// </summary>
        private async void AddSong() {
            var startAddWindow = new Views.ChildWindows.Songrequest.AddSong() { IsModal = true, AllowMove = true };
            ((ChildWindows.Songrequest.AddSong)startAddWindow.DataContext).CloseEvent += (sender, EventArgs) => CloseAddWindow(startAddWindow);

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(startAddWindow).ConfigureAwait(false);
        }

        /// <summary>
        /// Fires then the "add song window" is closed
        /// </summary>
        /// <param name="startTimerWindow"></param>
        private void CloseAddWindow(Views.ChildWindows.Songrequest.AddSong startTimerWindow) {
            var dataContext = Internal.SimpleChildWindow.GetDataContext<Views.ChildWindows.Songrequest.AddSong, ChildWindows.Songrequest.AddSong>
                (startTimerWindow, startTimerWindow.DataContext);

            if(dataContext?.Item1 != null && dataContext?.Item2 != null) {
                Handler.AddSong(dataContext.Item2.Video, dataContext.Item2.InstantStart);

                dataContext.Item1.Close();
            }
        }

        /// <summary>
        /// Stop a viewer to register a new song
        /// </summary>
        private void StopRegisterSongrequest() => Handler.StopRegistration();

        /// <summary>
        /// Start giveaway form
        /// IMHO a giant hack against mvvm
        /// </summary>
        private async void StartSongrequestSetup() {
            var startSongrequestWindow = new Views.ChildWindows.Songrequest.StartSongrequest() { IsModal = true, AllowMove = true };
            ((ChildWindows.Songrequest.StartSongrequest)startSongrequestWindow.DataContext).CloseEvent += (sender, EventArgs) => CloseStartWindow(startSongrequestWindow);

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(startSongrequestWindow).ConfigureAwait(false);
        }

        /// <summary>
        /// Fires when the start giveaway form closed
        /// </summary>
        /// <param name="startTimerWindow"></param>
        private void CloseStartWindow(Views.ChildWindows.Songrequest.StartSongrequest startTimerWindow) {
            Views.ChildWindows.Songrequest.StartSongrequest window;
            ChildWindows.Songrequest.StartSongrequest dataContext;

            if ((window = startTimerWindow as Views.ChildWindows.Songrequest.StartSongrequest) != null) {
                if ((dataContext = window.DataContext as ChildWindows.Songrequest.StartSongrequest) != null) {
                    if (dataContext.IsCompleted) {
                        StartSongrequest(dataContext);
                    }
                }

                window.Close();
            }
        }

        /// <summary>
        /// Starts the handler
        /// </summary>
        /// <param name="dataContext"></param>
        private void StartSongrequest(ChildWindows.Songrequest.StartSongrequest dataContext) {
            Handler = new Extensions.Songrequest.Handler {
                Properties = dataContext.AddModel,
                IsStarted = true
            };
        }

        #endregion Functions
    }
}