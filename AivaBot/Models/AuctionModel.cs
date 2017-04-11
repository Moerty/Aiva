using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivaBot.Models
{
    
    public class AuctionModel
    {
        public Text Text { get; set; }
    }


    public class Text
    {
        public string NameWatermark { get; set; }
        public string CommandWatermark { get; set; }
        public string TicketExpanderName { get; set; }
        public string ButtonStartName { get; set; }
        public string ButtonStopName { get; set; }
    }
}