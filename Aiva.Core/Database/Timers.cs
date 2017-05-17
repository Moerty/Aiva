using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Core.Database {
    public class Timers {

        /// <summary>
        /// Returns the existing Timers
        /// </summary>
        /// <returns></returns>
        public static List<Storage.Timers> GetExistingTimers() {
            using(var context = new Storage.StorageEntities()) {
                return context.Timers.ToList();
            }
        }

        /// <summary>
        /// Remove the selected command command from list and database
        /// </summary>
        public static void RemoveTimer(Storage.Timers timer) {
            using (var context = new Core.Storage.StorageEntities()) {
                context.Timers.Remove(timer);

                context.SaveChanges();
            }
        }
    }
}
