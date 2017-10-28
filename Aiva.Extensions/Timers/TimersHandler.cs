using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Aiva.Extensions.Models;

namespace Aiva.Extensions.Timers {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TimersHandler {
        public ObservableCollection<Core.Storage.Timers> Timers { get; set; }
        public Core.Storage.Timers SelectedTimer { get; set; }
        public List<Timer> TimersList { get; set; }

        #region Constuctor
        public TimersHandler() {
            TimersList = new List<Timer>();
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
                AddTimerToInternalList(new Models.Timers.AddModel {
                    Active = timer.Active,
                    Autoreset = timer.Autoreset,
                    Timer = timer.Timer,
                    CreatedAt = timer.CreatedAt,
                    Interval = timer.Interval,
                    Text = timer.Text
                });
            }
        }

        #endregion Constructor

        #region Functions
        private void WriteInChat(object sender, ElapsedEventArgs e, string message) {
            Core.AivaClient.Instance.AivaTwitchClient.SendMessage(message);
        }

        public void RemoveTimer() => Core.Database.Timers.RemoveTimer(SelectedTimer);

        public async void AddTimerAsync(Models.Timers.AddModel model) {
            AddTimerToDatabase(model);
            AddTimerToInternalList(model);

            // refresh timers list
            await Task.Run(() => GetTimers()).ConfigureAwait(false);
        }

        private void AddTimerToInternalList(Models.Timers.AddModel model) {
            Timer timerObject = new Timer {
                Interval = new TimeSpan(0, (int)model.Interval, 0).TotalMilliseconds
            };
            timerObject.Elapsed += (sender, e) => WriteInChat(sender, e, model.Text);
            timerObject.AutoReset = model.Autoreset;
            if (model.Active)
                timerObject.Start();

            TimersList.Add(timerObject);
        }


        private static void AddTimerToDatabase(Models.Timers.AddModel model) {
            var timer = new Core.Storage.Timers {
                Active = model.Active,
                Autoreset = model.Autoreset,
                CreatedAt = DateTime.Now,
                Interval = model.Interval,
                Text = model.Text,
                Timer = model.Timer
            };

            Core.Database.Timers.AddTimerToDatabase(timer);
        }
        #endregion Funtions
    }
}
