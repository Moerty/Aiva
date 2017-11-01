using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Core.DatabaseHandlers {
    public class Timers {
        public bool AddTimer(string name, string text, int interval, int lines) {
            var entry = new Storage.Timers {
                CreatedAt = DateTime.Now,
                Interval = interval,
                Text = text,
                Name = name
            };

            using (var context = new Storage.StorageEntities()) {
                var searchEntry = context.Timers.SingleOrDefault(t => String.Compare(t.Name, entry.Name, true) == 0);

                if (searchEntry == null) {
                    context.Timers.Add(entry);
                    context.SaveChanges();
                    return true;
                } else {
                    return false;
                }
            }
        }
    }
}
