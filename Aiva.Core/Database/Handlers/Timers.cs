using Aiva.Core.Database.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aiva.Core.Database.Handlers {
    public class Timers {
        public void AddTimer(Storage.Timers timer) {
            using (var context = new DatabaseContext()) {
                context.Timers.Add(timer);
                context.SaveChanges();
            }
        }

        public void RemoveTimer(Storage.Timers timer) {
            using (var context = new DatabaseContext()) {
                var dbTimer = context.Timers.SingleOrDefault(t => t.TimersId == timer.TimersId);

                if (dbTimer != null) {
                    context.Timers.Remove(dbTimer);
                    context.SaveChanges();
                }
            }
        }

        public List<Storage.Timers> GetTimers() {
            using (var context = new DatabaseContext()) {
                return context.Timers.ToList();
            }
        }

        public List<Storage.Timers> GetTimersToStart(bool refreshTimers) {
            List<Storage.Timers> timersToStart;
            using (var context = new DatabaseContext()) {
                var substractedTime = DateTime.Now.AddMinutes(-1);
                timersToStart = context.Timers
                    .Where(t => t.NextExecution.HasValue && t.NextExecution.Value <= substractedTime)
                    .ToList();

                if (refreshTimers) {
                    RefreshTimers(timersToStart.Select(t => t.TimersId));
                }
            }

            return timersToStart;
        }

        public void RefreshTimers(IEnumerable<int> timerIds) {
            using (var context = new DatabaseContext()) {
                var timers = context.Timers.Where(t => timerIds.Contains(t.TimersId));

                foreach (var timer in timers) {
                    timer.NextExecution = DateTime.Now.AddMinutes(timer.Interval);
                }

                context.SaveChanges();
            }
        }

        public void RefreshTimersOnStartup() {
            using (var context = new DatabaseContext()) {
                var timers = context.Timers.AsEnumerable();

                foreach (var timer in timers) {
                    timer.NextExecution = DateTime.Now.AddMinutes(timer.Interval);
                }

                context.SaveChanges();
            }
        }

        public void EditTimer(string name, string text, int interval, int id) {
            using (var context = new DatabaseContext()) {
                var timer = context.Timers.SingleOrDefault(t => t.TimersId == id);

                if (timer != null) {
                    timer.Name = name;
                    timer.Text = text;
                    timer.Interval = interval;
                    context.SaveChanges();
                }
            }
        }
    }
}