using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Core.DatabaseHandlers {
    public class Timers {

        /// <summary>
        /// Returns the existing Timers
        /// </summary>
        /// <returns></returns>
        public List<Storage.Timers> GetExistingTimers() {
            using(var context = new Storage.StorageEntities()) {
                return context.Timers.ToList();
            }
        }

        /// <summary>
        /// Remove the selected timer from database
        /// </summary>
        public void RemoveTimer(Storage.Timers timer) {
            using (var context = new Core.Storage.StorageEntities()) {
                context.Timers.Remove(timer);

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Add a timer to the database
        /// </summary>
        /// <param name="timer"></param>
        public void AddTimerToDatabase(Storage.Timers timer) {
            using(var context = new Storage.StorageEntities()) {
                context.Timers.Add(timer);

                context.SaveChanges();
            }
        }
    }
}
