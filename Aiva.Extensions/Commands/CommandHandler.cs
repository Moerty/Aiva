using System.Collections.Generic;
using System.Linq;

namespace Aiva.Extensions.Commands {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class CommandHandler {
        public List<Core.Storage.Commands> CommandList { get; private set; }
        public Core.Storage.Commands SelectedCommand { get; set; }

        public CommandReceiver Receiver { get; set; }

        public CommandHandler() {
            LoadCommandList();

            Receiver = new CommandReceiver();
        }

        /// <summary>
        /// Load the list from the Database
        /// </summary>
        private void LoadCommandList() {
            using (var context = new Core.Storage.StorageEntities()) {
                CommandList = context.Commands.ToList();
            }
        }

        /// <summary>
        /// Remove the selected command command from list and database
        /// </summary>
        public void RemoveCommand() {
            using (var context = new Core.Storage.StorageEntities()) {
                context.Commands.Remove(SelectedCommand);

                context.SaveChanges();
            }

            LoadCommandList();
        }
    }
}