using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackBoxBot.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class FirstStartModel {
        public MahApps.Metro.Controls.MetroContentControl Content { get; set; }
    }
}
