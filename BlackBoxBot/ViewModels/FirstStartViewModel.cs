using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackBoxBot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class FirstStartViewModel {
        public Models.FirstStartModel Model { get; set; } = new Models.FirstStartModel();
    }
}
