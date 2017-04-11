using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Events.Client;

namespace Giveaway.Models
{
    [PropertyChanged.ImplementPropertyChanged]
    public class UsersModel
	{
		public string Username { get; set; }
        public bool IsSub { get; set; }
	}
}
