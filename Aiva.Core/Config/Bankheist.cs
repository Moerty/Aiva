using IniParser;
using IniParser.Model;
using System.IO;
using System.Text;

namespace Aiva.Core.Config {
    public class Bankheist {
        public static IniData Config { get; private set; } = new IniData(new FileIniDataParser().ReadFile("ConfigFiles\\Games\\bankheist.ini"));

        public Bankheist() { }

        public static void WriteConfig() {
            new FileIniDataParser().WriteFile("ConfigFiles\\Games\\bankheist.ini", Config, Encoding.UTF8);
        }

        public static void WriteInitialConfig() {
            var Config = new FileIniDataParser().ReadFile("ConfigFiles\\Games\\bankheist.default");

            if (File.Exists("ConfigFiles\\Games\\bankheist.ini")) {
                new FileIniDataParser().WriteFile("ConfigFiles\\Games\\bankheist.ini", Config, Encoding.UTF8);
                Bankheist.Config = new IniData(new FileIniDataParser().ReadFile("ConfigFiles\\Games\\bankheist.ini"));
            } else {
                File.Create("ConfigFiles\\Games\\bankheist.ini").Dispose();
                WriteInitialConfig();
            }
        }
    }
}
