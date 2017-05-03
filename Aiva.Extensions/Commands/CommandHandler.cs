using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Commands {
    [PropertyChanged.ImplementPropertyChanged]
    public class CommandHandler {
        public List<Core.Storage.Commands> CommandList { get; private set; }

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
        /// Add a Command to the Database
        /// </summary>
        /// <param name="AddModel"></param>
        public async void AddCommandAsync(Models.Commands.AddModel AddModel) {
            using (var context = new Core.Storage.StorageEntities()) {

                var commandEntry = context.Commands.SingleOrDefault(c => String.Compare(c.Command, AddModel.Command, true) == 0);

                // Command doesnt exist, create new
                if (commandEntry == null) {
                    commandEntry = new Core.Storage.Commands {
                        Command = AddModel.Command,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        ExecutionRight = (int)AddModel.SelectedUserRight,
                        Text = AddModel.Text,
                        Cooldown = AddModel.Cooldown,
                        Count = 0,
                    };

                    context.Commands.Add(commandEntry);
                } else {
                    // Command exists; update
                    commandEntry.ExecutionRight = (int)AddModel.SelectedUserRight;
                    commandEntry.ModifiedAt = DateTime.Now;
                    commandEntry.Text = AddModel.Text;
                    commandEntry.Cooldown = AddModel.Cooldown;
                }

                await context.SaveChangesAsync();
            }

            // Load new CommandList
            LoadCommandList();
        }
    }
}
