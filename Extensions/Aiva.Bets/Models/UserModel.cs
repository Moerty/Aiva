using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivaBot.Bets.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class UserModel {
        public string Name { get; set; }
        public int Value { get; set; }
        public char Team { get; set; }
    }
}
