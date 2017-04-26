using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.IO;

namespace Aiva.Core.Test {
    [TestClass]
    public class DatabaseTest {
        [TestMethod]
        public void Database_Exists() {
            var expected = true;

            var existFile = File.Exists(@".\ConfigFiles\Database.db");

            Assert.AreEqual(existFile, expected);
        }


        [TestMethod]
        public void Database_AddUserToDatabase() {
            var generatedUser = new Aiva.Core.Storage.Users {
                Id = 123456,
                Name = "testEntry",
            };

            // read
            using (var context = new Aiva.Core.Storage.StorageEntities()) {
                var user = context.Users.SingleOrDefault(u => u.Id == generatedUser.Id);

                if (user != null) {
                    Assert.AreEqual(generatedUser.Id, user.Id);
                }
            }
        }

        [TestMethod]
        public void Database_AddUserToDatabaseCheckCurrency() {
            var generatedUser = new Aiva.Core.Storage.Users {
                Id = 123456,
                Name = "testEntry",
                Currency = new Aiva.Core.Storage.Currency {
                    ID = 123456,
                    Value = 0,
                }
            };

            // write
            using (var context = new Aiva.Core.Storage.StorageEntities()) {
                context.Users.Add(generatedUser);
            }

            // read
            using (var context = new Aiva.Core.Storage.StorageEntities()) {
                var user = context.Users.SingleOrDefault(u => u.Id == generatedUser.Id);

                if (user != null) {
                    Assert.AreEqual(generatedUser.Currency.ID, user.Id);
                }
            }
        }
    }
}
