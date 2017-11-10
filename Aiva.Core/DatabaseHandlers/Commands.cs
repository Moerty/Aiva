using System;
using System.Linq;

namespace Aiva.Core.DatabaseHandlers {

    public class Commands {

        /// <summary>
        /// Increate the stack from the command
        /// </summary>
        /// <param name="commandName"></param>
        public void IncreaseCommandCount(string commandName) {
            using (var context = new Core.Storage.StorageEntities()) {
                var command = context.Commands.SingleOrDefault(c => String.Compare(commandName, c.Name, true) == 0);

                if (command != null) {
                    command.Count++;

                    context.SaveChanges();
                }
            }
        }
    }
}