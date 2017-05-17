using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Aiva.Extensions.Models;

namespace Aiva.Extensions.Timers {
    public class TimersHandler {
        public ObservableCollection<Core.Storage.Timers> Timers { get; set; }
        public Core.Storage.Timers SelectedTimer { get; set; }
        public List<Timer> TimersList { get; set; }

        #region Constuctor
        public TimersHandler() {
            LoadTimers();
        }

        private void LoadTimers() {
            GetTimers();
            SetupTimers();
        }

        private void GetTimers() {
            var timersDatabase = Core.Database.Timers.GetExistingTimers();

            Timers = new ObservableCollection<Core.Storage.Timers>(timersDatabase);
        }

        private void SetupTimers() {
            foreach(var timer in Timers) {
                Timer timerObject = new Timer();
                timerObject.Interval = new TimeSpan(0, (int)timer.Interval, 0).TotalMilliseconds;
                timerObject.Elapsed += (sender, e) => WriteInChat(sender, e, timer.Text);
                timerObject.AutoReset = timer.Autoreset;
                if (timer.Active)
                    timerObject.Start();

                TimersList.Add(timerObject);
            }
        }

        #endregion Constructor

        #region Functions
        private void WriteInChat(object sender, ElapsedEventArgs e, string message) {
            Core.AivaClient.Instance.AivaTwitchClient.SendMessage(message);
        }

        public void RemoveTimer() => Core.Database.Timers.RemoveTimer(SelectedTimer);

        public async void AddTimerAsync(Models.Timers.AddModel model) {
            var timer = new Core.Storage.Timers {
                Active = model.Active,
                Autoreset = model.Autoreset,
                CreatedAt = DateTime.Now,
                Interval = model.Interval,
                Text = model.Text,
                Timer = model.Name
            };

            using (var context = new Core.Storage.StorageEntities()) {
                context.Timers.Add(timer);

                await context.SaveChangesAsync();
            }
        }
        #endregion Funtions
    }
}
