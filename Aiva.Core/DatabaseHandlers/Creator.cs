using System.IO;

namespace Aiva.Core.DatabaseHandlers {
    public class Creator {
        /// <summary>
        /// Creates the database
        /// </summary>
        public static void CreateDatabaseIfNotExist() {
            using (var context = new Storage.StorageEntities()) {
                if (!File.Exists(context.Database.Connection.ConnectionString.Replace(@"data source=", ""))) {
                    context.Database.CreateIfNotExists();

                    context.Database.ExecuteSqlCommand(Properties.Resources.Users);
                    context.Database.ExecuteSqlCommand(Properties.Resources.ActiveUsers);
                    context.Database.ExecuteSqlCommand(Properties.Resources.Chat);
                    context.Database.ExecuteSqlCommand(Properties.Resources.Commands);
                    context.Database.ExecuteSqlCommand(Properties.Resources.Currency);
                    context.Database.ExecuteSqlCommand(Properties.Resources.TimeWatched);
                    context.Database.ExecuteSqlCommand(Properties.Resources.Timers);
                    context.Database.ExecuteSqlCommand(Properties.Resources.BlacklistedWords);

                    context.SaveChanges();
                }
            }
        }
    }
}