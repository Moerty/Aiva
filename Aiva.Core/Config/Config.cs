using System;
using System.IO;

namespace Aiva.Core.Config {
    public class Config {
        private static readonly Lazy<Config> lazyConfig = new Lazy<Config>();
        public static Config Instance = lazyConfig.Value;

        public Storage.Root Storage;

        public Config() {
            if (File.Exists(GetConfigPath())) {
                Storage = Core.Config.Storage.Root.FromJson(File.ReadAllText(GetConfigPath()));
            } else {
                LoadDefaultConfigFile();
                Storage = Core.Config.Storage.Root.FromJson(File.ReadAllText(GetConfigPath()));
            }
        }

        /// <summary>
        /// Move default config file to "real" file
        /// </summary>
        private void LoadDefaultConfigFile()
            => File.Move(GetSampleConfigPath(), GetConfigPath());

        /// <summary>
        /// Save config to disc
        /// </summary>
        public void SaveConfig() {
            var json = Storage.ToJson();
            File.WriteAllText(GetConfigPath(), json);
        }

        /// <summary>
        /// Get sample Config Path
        /// </summary>
        /// <returns></returns>
        public static string GetSampleConfigPath() {
            return Path.Combine(
                AppContext.BaseDirectory,
                "Config",
                "config.json.default");
        }

        /// <summary>
        /// Get Config Path
        /// </summary>
        /// <returns></returns>
        public static string GetConfigPath() {
            return Path.Combine(
                AppContext.BaseDirectory,
                "Config",
                "config.json");
        }
    }
}