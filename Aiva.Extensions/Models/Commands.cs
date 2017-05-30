using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Models {

    public class Commands : INotifyPropertyChanged {

        public class AddModel : INotifyPropertyChanged {
            public string Command { get; set; }
            public string Text { get; set; }
            public int Cooldown { get; set; }
            public UserRights SelectedUserRight { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public enum UserRights {
            Viewer = 0,
            Mod = 1,
            Admin = 2
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
