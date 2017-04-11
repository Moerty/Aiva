using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivaBot.Auction.Models {
    public class InitModel {
        public string Name { get; set; }
        public string Command { get; set; }
        public bool WithTickets { get; set; }
        public int Tickets { get; set; }

        public bool WriteStartInChat { get; set; }
    }
}
