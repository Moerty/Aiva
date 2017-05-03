using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class Commands {
        [PropertyChanged.ImplementPropertyChanged]
        public class AddModel {
            public string Command { get; set; }
            public string Text { get; set; }
            public int Cooldown { get; set; }
            public UserRights SelectedUserRight { get; set; }
        }

        public enum UserRights {
            Viewer = 0,
            Mod = 1,
            Admin = 2
        }
    }
}
