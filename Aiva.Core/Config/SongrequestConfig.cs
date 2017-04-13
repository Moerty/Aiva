using IniParser;
using IniParser.Model;
using System.IO;
using System.Text;

namespace Aiva.Core.Config {
    public class SongrequestConfig {
        public static IniData Config { get; private set; } = new IniData(new FileIniDataParser().ReadFile("ConfigFiles\\songrequest.ini"));

        public SongrequestConfig() { }

        public static void WriteConfig() {
            new FileIniDataParser().WriteFile("ConfigFiles\\songrequest.ini", Config, Encoding.UTF8);
        }

        public static void WriteInitialConfig() {
            var Config = new FileIniDataParser().ReadFile("ConfigFiles\\songrequest.default");

            if (File.Exists("ConfigFiles\\songrequest.ini")) {
                new FileIniDataParser().WriteFile("ConfigFiles\\songrequest.ini", Config, Encoding.UTF8);
                SongrequestConfig.Config = new IniData(new FileIniDataParser().ReadFile("ConfigFiles\\songrequest.ini"));
            } else {
                File.Create("ConfigFiles\\songrequest.ini").Dispose();
                WriteInitialConfig();
            }
        }
    }
}
