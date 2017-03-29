using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackBoxBot.Auction.Models
{
    [PropertyChanged.ImplementPropertyChanged]
    public class AddModel
    {
        public string Username { get; set; }
        public int Tickets { get; set; }
    }
}
