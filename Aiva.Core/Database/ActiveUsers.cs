using System;
using System.Collections.Generic;
using System.Linq;

namespace Aiva.Core.Database {
    public class ActiveUsersHandler {
        public static void AddUserToList(string name) {
            using (var context = new StorageEntities()) {
                context.ActiveUsers.Add(
                    new ActiveUsers {
                        Name = name,
                        Joined = DateTime.Now.ToString(),
                    });

                context.SaveChanges();
            }
        }

        public static void AddUserToList(List<string> list) {
            using (var context = new StorageEntities()) {
                list.ForEach(x => {
                    context.ActiveUsers.Add(
                        new ActiveUsers {
                            Name = x,
                            Joined = DateTime.Now.ToString(),
                        });
                });

                context.SaveChanges();
            }
        }

        public static void RemoveUserFromList(string name) {
            using (var context = new StorageEntities()) {
                var user = context.ActiveUsers.SingleOrDefault(x => (String.Compare(x.Name, name) == 0));
                context.ActiveUsers.Remove(user);

                context.SaveChanges();
            }
        }

        public static DateTime GetJoinedTime(string name) {
            using (var context = new StorageEntities()) {
                var user = context.ActiveUsers.SingleOrDefault(x => (String.Compare(x.Name, name) == 0));
                return DateTime.Parse(user.Joined);
            }
        }

        public static void AddTimeWatchedToDatabase(string name, DateTime Ticks) {
            using (var context = new StorageEntities()) {
                var user = context.Users.SingleOrDefault(x => String.Compare(x.Name, name) == 0);

                if (user != null) {
                    var timeWatched = long.Parse(user.TimeWatched);
                    timeWatched += Ticks.Ticks;
                    user.TimeWatched = timeWatched.ToString();

                    context.SaveChanges();
                }
            }
        }

        public static void OnExistProgram(object sender, EventArgs e) {
            using (var context = new StorageEntities()) {
                foreach (var entry in context.ActiveUsers.ToList()) {
                    var user = context.Users.SingleOrDefault(x => String.Compare(x.Name, entry.Name, StringComparison.OrdinalIgnoreCase) == 0);

                    if (user != null) {
                        long parsedTimeWatched;
                        if (long.TryParse(user.TimeWatched, out parsedTimeWatched)) {
                            parsedTimeWatched += DateTime.Parse(entry.Joined).Ticks;
                        } else {
                            user.TimeWatched = "1";
                        }
                        user.TimeWatched = parsedTimeWatched.ToString();
                    }
                }
                context.ActiveUsers.RemoveRange(context.ActiveUsers);

                context.SaveChanges();
            }
        }
    }
}
