using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Core.Models {
    public class DatabaseCurrencyModel {
        public class ListCurrencyUpdate {
            public string Name { get; set; }
            public long TwitchID { get; set; }
            public int Value { get; set; }
        }
    }
}
