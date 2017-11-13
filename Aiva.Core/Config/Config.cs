using Aiva.Core.Models;
using Newtonsoft.Json;
using System.IO;

namespace Aiva.Core.Config {
    public class Config {
        private static Config _Instance;

        public static Config Instance {
            get {
                return _Instance ?? (_Instance = new Config());
            }
            private set {
                _Instance = value;
            }
        }

        public ConfigStorage.Root Storage;

        public Config() {
            if (File.Exists("ConfigFiles\\config.json")) {
                LoadConfig();
            }
        }

        private void LoadConfig() {
            Storage = ConfigStorage.Root.FromJson(File.ReadAllText("ConfigFiles\\config.json"));
        }

        public void LoadDefaultConfigFile() {
            CreateConfig();
            LoadConfig();
        }

        public void CreateConfig() {
            File.Move("ConfigFiles\\config.json.default", "ConfigFiles\\config.json");
        }

        public void WriteConfig() {
            var json = Storage.ToJson();
            File.WriteAllText("ConfigFiles\\config.json", json);
        }
    }

    public static class Serialize {
        public static string ToJson(this ConfigStorage.Root self) => JsonConvert.SerializeObject(self, ConfigStorage.Converter.Settings);
    }
}