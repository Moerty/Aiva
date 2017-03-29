using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config {
    public class Currency {
        public static IniData Config { get; } = new IniData(new FileIniDataParser().ReadFile("Configs\\currency.ini"));

        public static void WriteConfig() {
            new FileIniDataParser().WriteFile("Configs\\currency.ini", Config, Encoding.UTF8);
        }
    }
}
