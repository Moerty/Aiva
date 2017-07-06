using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Models {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Timers {

        [PropertyChanged.AddINotifyPropertyChangedInterface]
        public class AddModel {
            public string Name { get; set; }
            public string Text { get; set; }
            public System.DateTime CreatedAt { get; set; }
            public long Interval { get; set; }
            public bool Autoreset { get; set; }
            public bool Active { get; set; }
        }
    }
}
