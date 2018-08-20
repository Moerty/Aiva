using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;

namespace Aiva.Core.Twitch.Tasks.Commands.ModCommands {
    public class EditCommands {

        private const string _command = "command";
        private const string _addPrefix = "add";
        private const string _editPrefix = "edit";
        private const string _removePrefix = "remove";
        private const string _regexQuotesPrefix = "\".*?\"";
        private readonly Database.Handlers.Commands _commandsDatabaseHandler;

        public EditCommands() {
            _commandsDatabaseHandler = new Database.Handlers.Commands();
        }

        public void CommandReceived(object sender, OnChatCommandReceivedArgs e) {
            var text = new Regex(_regexQuotesPrefix).Match(e.Command.ArgumentsAsString);
            if (string.Compare(e.Command.CommandText, _command) == 0) {
                // if two quotes are found 
                if (text.Success) {
                    // have user permissions?
                    if ((int)e.Command.ChatMessage.UserType >= 1 // >= 1 == >= Mod
                        && e.Command.ChatMessage.UserType != UserType.Staff) {
                        // check parameters
                        if (e.Command.ArgumentsAsList.Count >= 1) {
                            switch (e.Command.ArgumentsAsList[0]) {
                                case _addPrefix:
                                    AddCommand(e.Command.ArgumentsAsList[1], text.Value, e.Command.ChatMessage.DisplayName);
                                    break;
                                case _editPrefix:
                                    EditCommand(e.Command.ArgumentsAsList[1], text.Value, e.Command.ChatMessage.DisplayName);
                                    break;
                                case _removePrefix:
                                    RemoveCommand(e.Command.ArgumentsAsList[1], e.Command.ChatMessage.DisplayName);
                                    break;
                                default:
                                    WriteHelpMessage(e.Command.ChatMessage.DisplayName);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void WriteHelpMessage(string displayName) {
            AivaClient.Instance.TwitchClient.SendMessage(
                AivaClient.Instance.Channel,
                $"@{displayName} HowTo: https://aiva.it0.me/Commands/modcommands");
        }

        private void AddCommand(string commandName, string commandText, string username) {
            var result = _commandsDatabaseHandler.AddCommand(username, commandName, commandText);

            if(result) {
                AivaClient.Instance.TwitchClient.SendMessage(
                    AivaClient.Instance.Channel,
                    $"@{username} successfully added command {commandName}", AivaClient.DryRun);
            }
        }

        private void EditCommand(string commandName, string commandText, string username) {
            var result = _commandsDatabaseHandler.EditCommand(commandName, commandText);

            if(result) {
                AivaClient.Instance.TwitchClient.SendMessage(
                    AivaClient.Instance.Channel,
                    $"@{username} successfully edited command {commandName}", AivaClient.DryRun);
            }
        }

        private void RemoveCommand(string commandName, string username) {
            var result = _commandsDatabaseHandler.RemoveCommand(commandName);

            if(result) {
                AivaClient.Instance.TwitchClient.SendMessage(
                    AivaClient.Instance.Channel,
                    $"@{username} successfully removed command {commandName}", AivaClient.DryRun);
            }
        }
    }
}
