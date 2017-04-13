using IniParser;
using IniParser.Model;
using System.IO;
using System.Text;

namespace Aiva.Core.Config {
    public class Currency {
        public static IniData Config { get; private set; } = new IniData(new FileIniDataParser().ReadFile("ConfigFiles\\currency.ini"));

        public static void WriteConfig() {
            new FileIniDataParser().WriteFile("ConfigFiles\\currency.ini", Config, Encoding.UTF8);
        }

        public static void WriteInitialConfig() {
            var Config = new FileIniDataParser().ReadFile("ConfigFiles\\currency.default");

            if (File.Exists("ConfigFiles\\currency.ini")) {
                new FileIniDataParser().WriteFile("ConfigFiles\\currency.ini", Config, Encoding.UTF8);
                Currency.Config = new IniData(new FileIniDataParser().ReadFile("ConfigFiles\\currency.ini"));
            } else {
                File.Create("ConfigFiles\\currency.ini").Dispose();
                WriteInitialConfig();
            }
        }
    }
}
