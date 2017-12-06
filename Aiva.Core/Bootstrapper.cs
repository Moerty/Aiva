namespace Aiva.Core {
    public class Bootstrapper {
        public void Start() {
            CheckIfDatabaseExists();
        }

        /// <summary>
        /// Check if the database exists and if not create it
        /// </summary>
        private void CheckIfDatabaseExists() {
            using (var context = new Database.Storage.DatabaseContext()) {
                context.Database.EnsureCreated();
            }
        }
    }
}