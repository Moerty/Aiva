using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class UserHandler
    {
        public class AddUser
        {
            public static void AddUserToDatabase(TwitchLib.Models.API.User.User user)
            {
                if (!CheckIfUserExist(user))
                {
                    // User doesnt exists
                    AddToUsersTableAsync(user);
                    AddToCurrencyTableAsync(user);
                }
            }
            private static bool CheckIfUserExist(TwitchLib.Models.API.User.User user)
            {
                using (var context = new DatabaseEntities())
                {
                    var result = context.Users.SingleOrDefault(x => x.TwitchID == user.Id);
                    return result != null;
                }
            }

            private static async void AddToUsersTableAsync(TwitchLib.Models.API.User.User user)
            {
                var userDb = new Users
                {
                    DisplayName = user.DisplayName,
                    Name = user.Name,
                    TwitchID = Convert.ToInt64(user.Id),
                    Type = user.Type,
                    CreatedAt = user.CreatedAt.ToString(),
                    LastSeen = DateTime.Now.ToString(),
                    Rank = 0,
                    IsViewing = 1
                };

                using (var context = new DatabaseEntities())
                {
                    context.Users.Add(userDb);
                    await context.SaveChangesAsync();
                }
            }
            private static async void AddToCurrencyTableAsync(TwitchLib.Models.API.User.User user)
            {
                var currency = new Currency
                {
                    Name = user.Name,
                    Value = 0
                };

                using (var context = new DatabaseEntities())
                {
                    context.Currency.Add(currency);
                    await context.SaveChangesAsync();
                }
            }
        }

        public class UpdateUser
        {
            public static async void UpdateLastSeenAsync(string name)
            {
                using (var context = new DatabaseEntities())
                {
                    var user = context.Users.FirstOrDefault(u => u.Name == name);

                    user.LastSeen = DateTime.Now.ToString();
                    user.IsViewing = 0;

                    //context.Users.Attach(user);
                    context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }

            public static async void SetIsViewingAsync(string name, bool value)
            {
                using (var context = new DatabaseEntities())
                {
                    var user = context.Users.SingleOrDefault(u => u.Name == name);

                    if (value)
                        user.IsViewing = 1;
                    else
                        user.IsViewing = 0;

                    await context.SaveChangesAsync();
                }
            }

            public static void OnExistProgramm(object sender, EventArgs e)
            {
                using (var context = new DatabaseEntities())
                {
                    var result = context.Users.Where(u => u.IsViewing == 1).ToList();

                    result.ForEach(u => u.IsViewing = 0);

                    context.SaveChanges();
                }
            }
        }
    }
}
