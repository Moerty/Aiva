using IniParser;
using IniParser.Model;
using System.Text;

namespace Aiva.Core.Config {
    public class General {
        public static IniData Config { get; private set; } = new IniData(new FileIniDataParser().ReadFile("ConfigFiles\\general.ini"));

        public General() { }

        public static void WriteConfig() {
            new FileIniDataParser().WriteFile("ConfigFiles\\general.ini", Config, Encoding.UTF8);
        }

        public static void WriteConfig(IniData Config) {
            new FileIniDataParser().WriteFile("ConfigFiles\\general.ini", Config, Encoding.UTF8);
            General.Config = new IniData(new FileIniDataParser().ReadFile("ConfigFiles\\general.ini"));
        }
    }
}