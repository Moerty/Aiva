using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Gui.ViewModels.Windows {
    public class Users {
        public Core.Database.Handlers.Users _databaseUsersHandler;
        public List<Core.Database.Storage.Users> UserList { get; set; }

        public Users() {
            _databaseUsersHandler = new Core.Database.Handlers.Users();
            UserList = _databaseUsersHandler.GetUsers();
        }
    }
}
