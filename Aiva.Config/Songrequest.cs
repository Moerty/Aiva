using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config {
    public class Songrequest {
        public static IniData Config { get; private set; } = new IniData(new FileIniDataParser().ReadFile("Configs\\songrequest.ini"));

        public Songrequest() { }

        public static void WriteConfig() {
            new FileIniDataParser().WriteFile("Configs\\songrequest.ini", Config, Encoding.UTF8);
        }

        public static void WriteInitialConfig() {
            IniData Config = new FileIniDataParser().ReadFile("Configs\\songrequest.default");

            if (File.Exists("Configs\\songrequest.ini")) {
                new FileIniDataParser().WriteFile("Configs\\songrequest.ini", Config, Encoding.UTF8);
                Songrequest.Config = new IniData(new FileIniDataParser().ReadFile("Configs\\songrequest.ini"));
            }
            else {
                File.Create("Configs\\songrequest.ini").Dispose();
                WriteInitialConfig();
            }
        }
    }
}
