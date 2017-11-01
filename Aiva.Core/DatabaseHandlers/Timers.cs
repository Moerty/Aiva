using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiva.Core.Storage;

namespace Aiva.Core.DatabaseHandlers {

    /// <summary>
    /// Database Handler for timers
    /// </summary>
    public class Timers {

        /// <summary>
        /// Add a timer to the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="interval"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public bool AddTimer(string name, string text, int interval, int lines) {
            var entry = new Storage.Timers {
                CreatedAt = DateTime.Now,
                Interval = interval,
                Text = text,
                Name = name
            };

            return AddTimer(entry);
        }

        /// <summary>
        /// Add a timer to the database
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public bool AddTimer(Storage.Timers timer) {
            if (!String.IsNullOrEmpty(timer.Name) &&
                !String.IsNullOrEmpty(timer.Text)) {

                using (var context = new StorageEntities()) {
                    var searchEntry = context.Timers.SingleOrDefault(t => String.Compare(t.Name, timer.Name, true) == 0);

                    if (searchEntry == null) {
                        context.Timers.Add(timer);
                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Remove a timer from the database
        /// </summary>
        /// <param name="selectedTimer"></param>
        public void RemoveTimer(Storage.Timers selectedTimer) {
            using (var context = new StorageEntities()) {
                var timer = context.Timers.SingleOrDefault(t => t.ID == selectedTimer.ID);

                if (timer != null) {
                    context.Timers.Remove(timer);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Get timers to start
        /// </summary>
        /// <returns></returns>
        public List<Storage.Timers> GetStartTimers(bool refreshTimers = false) {
            List<Storage.Timers> timersToStart;

            using (var context = new StorageEntities()) {
                var substracted = DateTime.Now.AddMinutes(-1);

                timersToStart = context.Timers.Where(t => t.NextExecution.HasValue && t.NextExecution <= substracted).ToList();

                if (timersToStart.Any() && refreshTimers) {
                    RefreshTimers(substracted);
                }

                return timersToStart;
            }
        }

        /// <summary>
        /// Refresh timers (next execution)
        /// </summary>
        /// <param name="substracted"></param>
        public void RefreshTimers(DateTime substracted) {
            using (var context = new StorageEntities()) {
                var timers = context.Timers.Where(t => t.NextExecution.HasValue && t.NextExecution <= substracted);

                foreach (var timer in timers) {
                    timer.NextExecution = DateTime.Now.AddMinutes(timer.Interval);
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Returns all Timers
        /// </summary>
        /// <returns></returns>
        public List<Storage.Timers> GetTimers() {
            using (var context = new StorageEntities()) {
                return context.Timers.ToList();
            }
        }

        /// <summary>
        /// Edit one timer
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="interval"></param>
        /// <param name="lines"></param>
        /// <param name="id"></param>
        public void EditTimer(string name, string text, int interval, int lines, long id) {
            using (var context = new StorageEntities()) {
                var timer = context.Timers.SingleOrDefault(t => t.ID == id);

                if (timer != null) {
                    timer.Name = name;
                    timer.Text = text;
                    timer.Interval = interval;
                    timer.NextExecution = DateTime.Now.AddMinutes(interval);

                    context.SaveChanges();
                }
            }
        }
    }
}
