using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
	public class Bankheist
	{
		public static IniData Config { get; } = new IniData(new FileIniDataParser().ReadFile("Configs\\Games\\bankheist.ini"));

		public Bankheist() { }

		public static void WriteConfig()
		{
			new FileIniDataParser().WriteFile("Configs\\Games\\bankheist.ini", Config, Encoding.UTF8);
		}
	}
}
