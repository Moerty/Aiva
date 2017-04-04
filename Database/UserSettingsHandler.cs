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

        //public Database.UserSettings GetConfig() {

        //}
    }
}
