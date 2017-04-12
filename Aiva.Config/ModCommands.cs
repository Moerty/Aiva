using IniParser;
using IniParser.Model;
using System.IO;
using System.Text;

namespace Config {
    public class ModCommands {
        public static IniData Config { get; private set; } = new IniData(new FileIniDataParser().ReadFile(".\\Configs\\modcommands.ini"));

        public static void WriteConfig() {
            new FileIniDataParser().WriteFile(".\\Configs\\modcommands.ini", Config, Encoding.UTF8);
        }

        public static void WriteInitialConfig() {
            IniData Config = new FileIniDataParser().ReadFile("Configs\\modcommands.default");

            if (File.Exists("Configs\\modcommands.ini")) {
                new FileIniDataParser().WriteFile("Configs\\modcommands.ini", Config, Encoding.UTF8);
                ModCommands.Config = new IniData(new FileIniDataParser().ReadFile(".\\Configs\\modcommands.ini"));
            }
            else {
                File.Create("Configs\\modcommands.ini").Dispose();
                WriteInitialConfig();
            }
        }
    }
}
