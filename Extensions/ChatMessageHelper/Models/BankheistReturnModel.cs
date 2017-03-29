using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Events.Client;

namespace ChatMessageHelper.Models
{
	public class BankheistReturnModel
	{
		public bool Accept { get; set; }
		public OnChatCommandReceivedArgs args { get; set; }
	}
}
