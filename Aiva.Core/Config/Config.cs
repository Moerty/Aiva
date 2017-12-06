using System;
using System.IO;

namespace Aiva.Core.Config {
    public class Config {
        private static readonly Lazy<Config> lazyConfig = new Lazy<Config>();
        public static Config Instance = lazyConfig.Value;

        public Storage.Root Storage;

        public Config() {
            var path = Path.Combine(AppContext.BaseDirectory, "Config", "config.json");

            if (File.Exists(path)) {
                Storage = Core.Config.Storage.Root.FromJson(File.ReadAllText(path));
            } else {
                LoadDefaultConfigFile(path);
                Storage = Core.Config.Storage.Root.FromJson(File.ReadAllText(path));
            }
        }

        private void LoadDefaultConfigFile(string pathJsonFile) {
            var sampleConfig = Path.Combine(
                AppContext.BaseDirectory,
                "Config",
                "config.json.default");

            File.Move(sampleConfig, pathJsonFile);
        }

        /// <summary>
        /// Save config to disc
        /// </summary>
        public void SaveConfig() {
            var json = Storage.ToJson();
            var path = Path.Combine(AppContext.BaseDirectory, "Config", "config.json");
            File.WriteAllText(path, json);
        }
    }
}