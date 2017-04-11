using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivaBot.Models {
    [PropertyChanged.ImplementPropertyChanged]
    class HomeModel {
        public string Header { get; set; }
        public MahApps.Metro.Controls.MetroContentControl Content { get; set; }
    }
}
