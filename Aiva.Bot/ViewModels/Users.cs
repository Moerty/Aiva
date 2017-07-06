using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Bot.ViewModels {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Users {
        public ObservableCollection<Core.Storage.Users> UsersList { get; set; }

        public Users() {
            using (var context = new Core.Storage.StorageEntities()) {
                UsersList = new ObservableCollection<Core.Storage.Users>(
                    context.Users.ToList());
            }
        }
    }
}
