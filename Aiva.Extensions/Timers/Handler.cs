﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Aiva.Core.Storage;

namespace Aiva.Extensions.Timers {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Handler {

        #region Models

        public ObservableCollection<Core.Storage.Timers> Timers { get; set; }
        public Core.Storage.Timers SelectedTimer { get; set; }

        private Core.DatabaseHandlers.Timers _databaseHandler;
        private Timer _checker;
        private Dictionary<string,Task> _internalTimersList;

        #endregion Models

        #region Constructor
        public Handler() {
            _databaseHandler = new Core.DatabaseHandlers.Timers();
            _internalTimersList = new Dictionary<string, Task>();
            LoadTimers();
            ResetTimers();
            ActivateTimers();
        }

        private void ResetTimers()
            => _databaseHandler.RefreshTimers();
        #endregion Constructor

        #region Methods
        /// <summary>
        /// Load timer properties
        /// </summary>
        private void ActivateTimers() {
            foreach (var timer in Timers) {
                SetTimer(timer);
            }
        }

        private void SetTimer(Core.Storage.Timers timer) {
            var task = Task.Run(async () => {
                await Task.Delay(TimeSpan.FromMinutes(timer.Interval).Milliseconds);
                StartTimer(timer);
            });

            //_internalTimersList.Add(timer.Name, task);
        }

        private void StartTimer(Core.Storage.Timers timer) {
            Core.AivaClient.Instance.AivaTwitchClient.SendMessage(timer.Text);

            SetTimer(timer);

            //var timerDatabase = _internalTimersList.SingleOrDefault(t => String.Compare(t.Key, timer.Name) == 0);

            //if(timerDatabase.Value != null) {
            //    timerDatabase.Value.
            //}
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
        private void LoadTimers() {
            var timers = _databaseHandler.GetTimers();

            Timers = new ObservableCollection<Core.Storage.Timers>(timers);

            timers.ForEach(t => SetTimer(t));
        }
            //=> Timers = new ObservableCollection<Core.Storage.Timers>(_databaseHandler.GetTimers());


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