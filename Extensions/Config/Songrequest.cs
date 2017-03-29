using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
	public class Songrequest
	{
		public static IniData Config { get; } = new IniData(new FileIniDataParser().ReadFile("Configs\\songrequest.ini"));

		public Songrequest() { }

		public static void WriteConfig()
		{
			new FileIniDataParser().WriteFile("Configs\\songrequest.ini", Config, Encoding.UTF8);
		}
	}
}
