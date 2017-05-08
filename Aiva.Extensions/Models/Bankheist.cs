using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Models {
    public class Bankheist {

        public class BankheistInitModel {
            public bool IsEnabled { get; set; }
            public string Command { get; set; }
        }

        public class BankheistUserModel {
            public string Name { get; set; }
            public string TwitchID { get; set; }
            public int Bet { get; set; }
        }

        public enum Bank {
            Bank1 = 1,
            Bank2 = 2,
            Bank3 = 3,
            Bank4 = 4,
            Bank5 = 5,
        }
    }
}
