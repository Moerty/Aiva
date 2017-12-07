using Aiva.Gui.Views.ChildWindows.Songrequest;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.Tabs {
    [AddINotifyPropertyChangedInterface]
    public class Songrequest {
        private bool _IsListining;
        public bool IsListining {
            get {
                return _IsListining;
            }
            set {
                _IsListining = !string.IsNullOrEmpty(_handler.Properties?.Command);
            }
        }

        public Aiva.Models.Songrequest.Song CurrentSong { get; set; }
        public Aiva.Models.Songrequest.Song SelectedSong { get; set; }
        public ObservableCollection<Aiva.Models.Songrequest.Song> SongList { get; set; }
        public ICommand StartListenCommand { get; set; }
        public ICommand StopListenCommand { get; set; }
        public ICommand AddSongCommand { get; set; }
        public ICommand NextSongCommand { get; set; }
        public ICommand OpenUrlCommand { get; set; }
        public ICommand PlaySelectedSongCommand { get; set; }
        public ICommand ChangeSettingsCommand { get; set; }
        public ICommand HonorRequesterCommand { get; set; }

        private readonly Extensions.Songrequest.Handler _handler;

        public Songrequest() {
            _handler = new Extensions.Songrequest.Handler();
            _handler.OnNewSong += AddNewSong;
            SongList = new ObservableCollection<Aiva.Models.Songrequest.Song>();

            StartListenCommand = new Internal.RelayCommand(
                start => StartListen(),
                start => !IsListining && !string.IsNullOrEmpty(_handler.Properties?.Command));

            StopListenCommand = new Internal.RelayCommand(
                stop => StopListen(),
                stop => IsListining);

            AddSongCommand = new Internal.RelayCommand(
                add => AddSong());

            NextSongCommand = new Internal.RelayCommand(
                next => PlayNextSong(),
                next => SongList.Count > 1);

            OpenUrlCommand = new Internal.RelayCommand(
                open => Process.Start(new ProcessStartInfo(CurrentSong.Url)),
                open => CurrentSong != null);

            PlaySelectedSongCommand = new Internal.RelayCommand(
                play => PlaySelectedSong(),
                play => SelectedSong != null);

            ChangeSettingsCommand = new Internal.RelayCommand(
                edit => EditSongrequestSettings());

            HonorRequesterCommand = new Internal.RelayCommand(
                honor => HonorRequester(),
                honor => CurrentSong != null);
        }

        private void HonorRequester() {
            var dataContextMainWindow = (ViewModels.Windows.MainWindow)Application.Current.MainWindow.DataContext;
            var dataContextHonorRequester = new Flyouts.HonorSongrequester(
                userId: CurrentSong.RequesterID,
                username: CurrentSong.Requester);

            // close the flyout
            dataContextHonorRequester.OnClose
                += (sender, EventArgs)
                => CloseHonorRequesterCommand();

            dataContextMainWindow.SelectedTab.Flyouts[0].DataContext = dataContextHonorRequester;

            dataContextMainWindow.SelectedTab.Flyouts[0].IsOpen = true;
        }

        private void CloseHonorRequesterCommand() {
            var dataContextMainWindow = (ViewModels.Windows.MainWindow)Application.Current.MainWindow.DataContext;
            dataContextMainWindow.SelectedTab.Flyouts[0].IsOpen = false;
        }

        private async void EditSongrequestSettings() {
            var options = new Edit();

            ((ChildWindows.Songrequest.Edit)options.DataContext).CloseEvent
                += (sender, EventArgs) => CloseEditWindow(options, true);

            options.Closing += (sender, EventArgs) => CloseEditWindow(options);

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(options).ConfigureAwait(false);
        }

        private void CloseEditWindow(Edit options, bool fromDatacontext = false) {
            var dataContext = Internal.ChildWindow.GetDatacontext
                <ChildWindows.Songrequest.Edit>(options.DataContext);

            if (dataContext?.IsCompleted() == true && fromDatacontext) {
                _handler.ChangeProperties(dataContext.Properties);
                options.Close();
            }
        }

        private void StopListen() {
            _handler.StopRegistration();
            IsListining = false;
        }

        private void StartListen() {
            _handler.StartRegistration();
            IsListining = true;
        }

        private void PlaySelectedSong() {
            CurrentSong = SelectedSong;
            SongList.Remove(SelectedSong);
        }

        /// <summary>
        /// Play next song
        /// </summary>
        private void PlayNextSong() {
            CurrentSong = SongList[0];
            SongList.RemoveAt(0);
        }

        /// <summary>
        /// Fires when a new song have to add to the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewSong(object sender, Aiva.Models.Songrequest.Song e) {
            Application.Current.Dispatcher.Invoke(() => SongList.Add(e));
        }

        /// <summary>
        /// Starts the add song child window
        /// </summary>
        private async void AddSong() {
            var addSongChildWindow = new Views.ChildWindows.Songrequest.Add {
                IsModal = true,
                AllowMove = false
            };

            ((ChildWindows.Songrequest.Add)addSongChildWindow.DataContext).CloseEvent
                += (sender, EventArgs) => CloseAddWindow(addSongChildWindow);

            await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(addSongChildWindow).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes when the add song child window close
        /// </summary>
        /// <param name="addSongChildWindow"></param>
        private void CloseAddWindow(Add addSongChildWindow) {
            var dataContext = Internal.ChildWindow.GetDatacontext
                <ChildWindows.Songrequest.Add>(addSongChildWindow.DataContext);

            if (dataContext != null) {
                _handler.AddSong(dataContext.Video);

                addSongChildWindow.Close();
            }
        }
    }
}