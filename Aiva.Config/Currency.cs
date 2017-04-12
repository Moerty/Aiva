using IniParser;
using IniParser.Model;
using System.IO;
using System.Text;

namespace Config {
    public class Currency {
        public static IniData Config { get; private set; } = new IniData(new FileIniDataParser().ReadFile("Configs\\currency.ini"));

        public static void WriteConfig() {
            new FileIniDataParser().WriteFile("Configs\\currency.ini", Config, Encoding.UTF8);
        }

        public static void WriteInitialConfig() {
            IniData Config = new FileIniDataParser().ReadFile("Configs\\currency.default");

            if (File.Exists("Configs\\currency.ini")) {
                new FileIniDataParser().WriteFile("Configs\\currency.ini", Config, Encoding.UTF8);
                Currency.Config = new IniData(new FileIniDataParser().ReadFile("Configs\\currency.ini"));
            }
            else {
                File.Create("Configs\\currency.ini").Dispose();
                WriteInitialConfig();
            }
        }
    }
}
