using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database {
    public class UserSettingsHandler {
        public static List<Database.UserSettings> GetConfig() {
            using (var context = new DatabaseEntities()) {
                return context.UserSettings.ToList();
            }
        }

        public static void WriteConfig(List<UserSettings> settings) {
            using (var context = new DatabaseEntities()) {
                context.UserSettings.RemoveRange(context.UserSettings);
                context.UserSettings.AddRange(settings);
                context.SaveChanges();
            }
        }

        //public Database.UserSettings GetConfig() {

        //}
    }
}
