﻿using System.IO;

namespace Aiva.Core {
    public static class Bootstrapper {
        public static void StartBootstrapper() {
            CheckNeededFiles();
        }

        /// <summary>
        /// Checks if the database is there
        /// </summary>
        internal static void CheckNeededFiles() {
            if (!File.Exists("ConfigFiles\\Database.db")) {
                DatabaseHandlers.Creator.CreateDatabaseIfNotExist();
            }
        }
    }
}