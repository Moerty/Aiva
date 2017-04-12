using IniParser;
using IniParser.Model;
using System.IO;
using System.Text;

namespace Config {
    public class Bankheist {
        public static IniData Config { get; private set; } = new IniData(new FileIniDataParser().ReadFile("Configs\\Games\\bankheist.ini"));

        public Bankheist() { }

        public static void WriteConfig() {
            new FileIniDataParser().WriteFile("Configs\\Games\\bankheist.ini", Config, Encoding.UTF8);
        }

        public static void WriteInitialConfig() {
            IniData Config = new FileIniDataParser().ReadFile("Configs\\Games\\bankheist.default");

            if (File.Exists("Configs\\Games\\bankheist.ini")) {
                new FileIniDataParser().WriteFile("Configs\\Games\\bankheist.ini", Config, Encoding.UTF8);
                Bankheist.Config = new IniData(new FileIniDataParser().ReadFile("Configs\\Games\\bankheist.ini"));
            }
            else {
                File.Create("Configs\\Games\\bankheist.ini").Dispose();
                WriteInitialConfig();
            }
        }
    }
}
