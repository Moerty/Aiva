using System;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Enums;
using TwitchLib.Events.Client;

namespace Aiva.Extensions.Commands {
    public class CommandReceiver {

        public CommandReceiver() {
            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived += CommandReceived;
        }

        /// <summary>
        /// Fires when a chat command is received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandReceived(object sender, OnChatCommandReceivedArgs e) {

            using (var context = new Core.Storage.StorageEntities()) {
                var command = context.Commands.SingleOrDefault(c => String.Compare(e.Command.Command, c.Command, true) == 0);

                if (command != null) {
                    // can user execute command?
                    if (CanUserExecuteCommand(e.Command.ChatMessage.UserType, command.ExecutionRight)) {
                        // check if cooldown is over 0 
                        if (command.Cooldown.HasValue && command.Cooldown > 0) {
                            if (command.LastExecution.HasValue) {
                                // last execution is not empty 
                                var nextTime = command.LastExecution.Value.AddMinutes(command.Cooldown.Value);

                                // if next execution time <= now
                                if (nextTime <= DateTime.Now) {
                                    ExecuteCommand(command, e);
                                    command.LastExecution = DateTime.Now;

                                    context.SaveChanges();
                                }

                            } else {
                                // last execution is empty - start command
                                ExecuteCommand(command, e);
                                command.LastExecution = DateTime.Now;
                            }
                        } else {
                            // cooldown has no value or is < 0
                            ExecuteCommand(command, e);
                            command.LastExecution = DateTime.Now;
                        }
                        // check if last execution has a value and cooldown is > 0
                        if (command.LastExecution.HasValue && command.Cooldown.Value > 0) {

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the user can execute the command
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="executionRight"></param>
        /// <returns></returns>
        private bool CanUserExecuteCommand(UserType userType, long executionRight) {
            switch(executionRight) {
                case (1): // TODO: Difference between Mod and gloabel Mod
                    return (int)userType == 1 ? true : (int)userType == 2 ? true : false; // Mod&GloabelMod:
                case (2): // return true if it is the broeadcaster or admin; staff will be ignored
                    return (int)userType > 3 ? true : false;
                default:
                    return true;
            }
        }
       
        /// <summary>
        /// Replace all values
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        private void ExecuteCommand(Core.Storage.Commands command, OnChatCommandReceivedArgs args) {

            /*
            * User
            * Stack (Count)
            * Channel
            * Time
            * Twitch Info for user
            * Userlevel
            * currency from user
            * */

        // user
        var stringToSend = command.Text.Replace("%user%", args.Command.ChatMessage.Username);

            // currency
            if (stringToSend.Contains("%currency%")) {
                var currency = Core.Database.Currency.GetCurrencyFromUser(args.Command.ChatMessage.UserId);
                if (!currency.HasValue) {
                    return; // return if this is a currency command and we dont have a currency value
                } else {
                    stringToSend = stringToSend.Replace("%currency%", currency.Value.ToString());
                }
            }

            // stack
            if (stringToSend.Contains("%stack%")) {
                stringToSend.Replace("%stack%", (command.Count + 1).ToString());
                Task.Run(() => Core.Database.Commands.IncreaseCommandCount(command.Command));
            }

            // time
            if (stringToSend.Contains("%time%"))
                stringToSend.Replace("%time%", DateTime.Now.ToString());

            // Twitch related Details
            stringToSend = ReplaceTwitchInformation(stringToSend, args);

            // channel
            if (stringToSend.Contains("%channel%"))
                stringToSend = stringToSend.Replace("%channel%", Core.AivaClient.Instance.Channel);

            // botname
            if (stringToSend.Contains("%botname%"))
                stringToSend = stringToSend.Replace("%botname%", Core.AivaClient.Instance.Username);


            Core.AivaClient.Instance.AivaTwitchClient.SendMessage(stringToSend);
        }

        /// <summary>
        /// Replace Twitch Informations
        /// </summary>
        /// <param name="stringToSend"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private string ReplaceTwitchInformation(string stringToSend, OnChatCommandReceivedArgs args) {
            /*
             * userid
             * username
             * usertype
             * issubscriber
             * scubscribermonthcount
             * isturbo
             * ismoderator
             * isbroadcaster
             * ispartnered
             * spendOnBits (USD)
             * */

            if (stringToSend.Contains("%userid%"))
                stringToSend = stringToSend.Replace("%userid%", args.Command.ChatMessage.UserId);

            if (stringToSend.Contains("%username%"))
                stringToSend = stringToSend.Replace("%username%", args.Command.ChatMessage.Username);

            if (stringToSend.Contains("%usertype%"))
                stringToSend = stringToSend.Replace("%usertype%", args.Command.ChatMessage.UserType.ToString());

            if (stringToSend.Contains("%issubscriber%"))
                stringToSend = stringToSend.Replace("%issubscriber%", args.Command.ChatMessage.IsSubscriber.ToString());

            if (stringToSend.Contains("%scubscribermonthcount%"))
                stringToSend = stringToSend.Replace("%scubscribermonthcount%", args.Command.ChatMessage.SubscribedMonthCount.ToString());

            if (stringToSend.Contains("%isturbo%"))
                stringToSend = stringToSend.Replace("%isturbo%", args.Command.ChatMessage.IsTurbo.ToString());

            if (stringToSend.Contains("%ismoderator%"))
                stringToSend = stringToSend.Replace("%ismoderator%", args.Command.ChatMessage.IsModerator.ToString());

            if (stringToSend.Contains("%isbroadcaster%"))
                stringToSend = stringToSend.Replace("%isbroadcaster%", args.Command.ChatMessage.IsBroadcaster.ToString());

            if (stringToSend.Contains("%ispartnered%"))
                stringToSend = stringToSend.Replace("%ispartnered%", args.Command.ChatMessage.IsPartnered.ToString());

            if (stringToSend.Contains("%spendonbits%"))
                stringToSend = stringToSend.Replace("%spendonbits%", args.Command.ChatMessage.BitsInDollars.ToString());


            return stringToSend;
        }
    }
}
