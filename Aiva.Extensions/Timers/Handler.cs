using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Aiva.Extensions.Timers {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Handler {

        #region Models

        public ObservableCollection<Core.Storage.Timers> Timers { get; set; }
        public Core.Storage.Timers SelectedTimer { get; set; }

        private Core.DatabaseHandlers.Timers _databaseHandler;
        private Timer _checker;

        #endregion Models

        #region Constructor
        public Handler() {
            _databaseHandler = new Core.DatabaseHandlers.Timers();
            LoadTimers();
            ActivateTimers();
        }
        #endregion Constructor

        #region Methods
        /// <summary>
        /// Load timer properties
        /// </summary>
        private void ActivateTimers() {
            _checker = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds) {
                Enabled = true,
                AutoReset = true,
            };
            _checker.Elapsed += _checker_Elapsed;
            _checker.Start();
        }

        /// <summary>
        /// Fires when "checker" elapsed (after 1 minute)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _checker_Elapsed(object sender, ElapsedEventArgs e) {
            bool refreshTimers = true;
            var timersToStart = _databaseHandler.GetStartTimers(refreshTimers: refreshTimers);

            if (timersToStart != null && timersToStart.Any()) {
                foreach (var timer in timersToStart) {
                    Core.AivaClient.Instance.AivaTwitchClient.SendMessage(timer.Text);
                }

                if (refreshTimers) {
                    LoadTimers();
                }
            }
        }

        /// <summary>
        /// Remove timer
        /// </summary>
        public void RemoveTimer() {
            _databaseHandler.RemoveTimer(SelectedTimer);
            SelectedTimer = null;
            LoadTimers();
        }


        /// <summary>
        /// Load timers from Database
        /// </summary>
        private void LoadTimers()
            => Timers = new ObservableCollection<Core.Storage.Timers>(_databaseHandler.GetTimers());


        /// <summary>
        /// Add timer to database
        /// Do it this way, cause i dont want to make "_databaseHandler" public
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="interval"></param>
        /// <param name="lines"></param>
        public bool AddTimerToDatabase(string name, string text, int interval, int lines) {

            var timer = new Core.Storage.Timers {
                Name = name.Replace(" ", ""),
                Text = text,
                Interval = interval,
                CreatedAt = DateTime.Now,
                NextExecution = DateTime.Now.AddMinutes(interval),
            };

            var result = _databaseHandler.AddTimer(timer);
            Timers.Add(timer);

            return result;
        }

        /// <summary>
        /// Edit timer
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="interval"></param>
        /// <param name="lines"></param>
        /// <param name="id"></param>
        public void EditTimer(string name, string text, int interval, int lines, long id) {
            _databaseHandler.EditTimer(name, text, interval, lines, id);
            LoadTimers();
        }
        #endregion Methods
    }
}