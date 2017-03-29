using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
	public class General
	{
		public static IniData Config { get; } = new IniData(new FileIniDataParser().ReadFile("Configs\\general.ini"));

		public General() { }

		public static void WriteConfig()
		{
			new FileIniDataParser().WriteFile("Configs\\general.ini", Config, Encoding.UTF8);
		}
	}
}


