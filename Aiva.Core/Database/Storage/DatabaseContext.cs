using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Aiva.Core.Database.Storage {
    public class DatabaseContext : DbContext {
        public virtual DbSet<ActiveUsers> ActiveUsers { get; set; }
        public virtual DbSet<BlacklistedWords> BlacklistedWords { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<TimeWatched> TimeWatched { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<Timers> Timers { get; set; }
        public virtual DbSet<Commands> Commands { get; set; }

        /// <summary>
        /// This method connects the context with the database
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = Path.Combine(AppContext.BaseDirectory, "Config", "Database.db") };

            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>()
                .HasKey(k => k.UsersId);

            modelBuilder.Entity<BlacklistedWords>().HasKey(k => k.BlacklistedWordsId);
            modelBuilder.Entity<Chat>().HasKey(k => k.ChatId);
            modelBuilder.Entity<Commands>().HasKey(k => k.CommandsId);
            modelBuilder.Entity<Timers>().HasKey(k => k.TimersId);
        }
    }
}