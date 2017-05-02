using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Bot.ViewModels.Flyouts
{
    [PropertyChanged.ImplementPropertyChanged]
    public class UsersInfoVM
    {
        public string Username { get; set; }
        public string ID { get; set; }
        public string Currency { get; set; }

        public UsersInfoVM(string name = null, string id = null)
        {
            if (name == null && id == null) return;
            if(id == null) {
                using (var context = new Core.Storage.StorageEntities()) {
                    var user = context.Users.SingleOrDefault(x => (String.Compare(x.Name, name, true)) == 0);

                    if(user != null) {
                        Username = user.DisplayName;
                        ID = user.Id.ToString();
                        Currency = user.Currency.Value.ToString();
                    }
                }
            }
        }
    }
}
