using System;
using System.Linq;

namespace Aiva.Bot.ViewModels.Flyouts {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class UsersInfoVM {
        public string Username { get; set; }
        public string ID { get; set; }
        public string Currency { get; set; }

        public UsersInfoVM(string name = null, string id = null) {
            if (name == null && id == null)
                return;
            if (id == null) {
                using (var context = new Core.Storage.StorageEntities()) {
                    var user = context.Users.SingleOrDefault(x => (String.Compare(x.Name, name, true)) == 0);

                    if (user != null) {
                        Username = user.DisplayName;
                        ID = user.Id;
                        Currency = user.Currency.Value.ToString();
                    }
                }
            }
        }
    }
}