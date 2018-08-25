using Aiva.Core.Twitch;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Aiva.Extensions.Timers
{
    public class Handler
    {
        #region Models
        private readonly Core.Database.Handlers.Timers _databaseTimersHandler;
        private List<Timer> _timerTasks;
        #endregion Models
        #region Cunstructor
        public Handler()
        {
            _databaseTimersHandler = new Core.Database.Handlers.Timers();
            RefreshTimersOnStartup();
            SetTimerTasks();
        }
        #endregion Cunstructor

        /// <summary>
        /// Method to reset all tasks (if task list is not null)
        /// </summary>
        private void SetTimerTasks()
        {
            ResetTimer();

            _timerTasks = new List<Timer>();
            foreach (var timer in _databaseTimersHandler.GetTimers())
            {
                SetTimerTask(timer);
            }
        }

        /// <summary>
        /// Reset all timers (cancel)
        /// </summary>
        private void ResetTimer()
        {
            _timerTasks?.ForEach(t => t.Change(Timeout.Infinite, Timeout.Infinite));
        }

        /// <summary>
        /// Create timer andadd to list
        /// </summary>
        /// <param name="timerDatabase"></param>
        private void SetTimerTask(Core.Database.Storage.Timers timerDatabase)
        {
            var timer = new Timer(
                e => ExecuteTimer(timerDatabase),
                null,
                TimeSpan.FromMinutes(timerDatabase.Interval),
                TimeSpan.FromMinutes(timerDatabase.Interval));

            _timerTasks.Add(timer);
        }

        /// <summary>
        /// Execute when timer elapse
        /// </summary>
        /// <param name="timer"></param>
        private void ExecuteTimer(Core.Database.Storage.Timers timer)
        {
            AivaClient.Instance.TwitchClient.SendMessage(
                channel: AivaClient.Instance.Channel,
                message: timer.Text,
                dryRun: AivaClient.DryRun);
        }

        /// <summary>
        /// Function to add a timer to database and refresh timers
        /// </summary>
        /// <param name="timer"></param>
        public void AddTimerToDatabase(Core.Database.Storage.Timers timer)
        {
            _databaseTimersHandler.AddTimer(timer);
            SetTimerTasks();
        }

        /// <summary>
        /// edit a timer and refresh task list
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="interval"></param>
        /// <param name="id"></param>
        public void EditTimer(string name, string text, int interval, int id)
        {
            _databaseTimersHandler.EditTimer(name, text, interval, id);
            SetTimerTasks();
        }

        /// <summary>
        /// Remove given timer from list
        /// </summary>
        /// <param name="selectedTimer"></param>
        public void RemoveTimerFromDatabase(Core.Database.Storage.Timers selectedTimer)
        {
            _databaseTimersHandler.RemoveTimer(selectedTimer);
            SetTimerTasks();
        }

        /// <summary>
        /// Get timers from database
        /// </summary>
        /// <returns></returns>
        public List<Core.Database.Storage.Timers> GetTimers()
        {
            return _databaseTimersHandler.GetTimers();
        }

        /// <summary>
        /// Refresh timers on startup (lastexecution = DateTime.Now + Interval)
        /// </summary>
        private void RefreshTimersOnStartup()
        {
            _databaseTimersHandler.RefreshTimersOnStartup();
        }
    }
}