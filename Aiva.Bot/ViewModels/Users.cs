using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Bot.ViewModels {
    public class Users : INotifyPropertyChanged {
        public ObservableCollection<Core.Storage.Users> UsersList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Users() {
            using (var context = new Core.Storage.StorageEntities()) {
                UsersList = new ObservableCollection<Core.Storage.Users>(
                    context.Users.ToList());
            }
        }
    }
}
