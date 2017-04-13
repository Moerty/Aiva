using IniParser;
using IniParser.Model;
using System.IO;
using System.Text;

namespace Aiva.Core.Config {
    public class ModCommands {
        public static IniData Config { get; private set; } = new IniData(new FileIniDataParser().ReadFile(".\\ConfigFiles\\modcommands.ini"));

        public static void WriteConfig() {
            new FileIniDataParser().WriteFile(".\\ConfigFiles\\modcommands.ini", Config, Encoding.UTF8);
        }

        public static void WriteInitialConfig() {
            var Config = new FileIniDataParser().ReadFile("ConfigFiles\\modcommands.default");

            if (File.Exists("ConfigFiles\\modcommands.ini")) {
                new FileIniDataParser().WriteFile("ConfigFiles\\modcommands.ini", Config, Encoding.UTF8);
                ModCommands.Config = new IniData(new FileIniDataParser().ReadFile(".\\ConfigFiles\\modcommands.ini"));
            } else {
                File.Create("ConfigFiles\\modcommands.ini").Dispose();
                WriteInitialConfig();
            }
        }
    }
}
