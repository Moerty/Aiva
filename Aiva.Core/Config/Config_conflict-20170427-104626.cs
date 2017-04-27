using IniParser.Model;
using System.Text;
using System.IO;
using System;

namespace Aiva.Core.Config {
    public class Config {

        private static IniData _Instance;
        public static IniData Instance {
            get {
                if (_Instance == null)
                    InitConfig();

                return _Instance;
            }
            private set {
                _Instance = value;
            }
        }

        /// <summary>
        /// Init Config
        /// </summary>
        private static void InitConfig() {
            if (!File.Exists("ConfigFiles\\config.ini"))
                throw new Exception("No config File found in \"ConfigFiles\\config.ini\"");

            Instance = new IniData(new IniParser.FileIniDataParser().ReadFile("ConfigFiles\\config.ini"));
        }

        /// <summary>
        /// Save Config
        /// </summary>
        public static void WriteConfig() {
            new IniParser.FileIniDataParser().WriteFile("ConfigFiles\\config.ini", Instance, Encoding.UTF8);
        }

        /// <summary>
        /// Create the default config for tests without Credentials
        /// </summary>
        public static void CreateDefaultConfig() {
            if (File.Exists("ConfigFiles\\config.ini.default")) {
                File.Copy("ConfigFiles\\config.ini.default", "ConfigFiles\\config.ini");
            }
        }
    }
}
