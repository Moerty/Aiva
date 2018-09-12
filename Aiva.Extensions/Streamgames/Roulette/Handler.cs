using Aiva.Core.Twitch;
using Aiva.Models.Streamgames.Roulette;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using System.Linq;
using Aiva.Core.Config;

namespace Aiva.Extensions.Streamgames.Roulette {
    public class Handler {
        private readonly Core.Database.Handlers.Currency _databaseCurrencyHandler;
        private readonly Random _randomGenerator;
        private readonly TableLayout _tableLayout;
        private List<User> registeredUsers;
        private int _winningNumber;
        private bool _isRunning;
        private bool _shouldStop;

        private const int minNumber = 0;
        private const int maxNumber = 36;
        private const int timeActiveRoulette = 10000; //300000;
        private const int timeToWaitForWinner = 30000;

        public Handler() {
            _databaseCurrencyHandler = new Core.Database.Handlers.Currency();
            _randomGenerator = new Random();
            _tableLayout = new TableLayout();
        }

        public async void StartRoulette() {
            while (Config.Instance.Storage.StreamGames.Roulette.General.Active) {
                await Task.Delay(5000);
                _isRunning = true;

                AivaClient.Instance.TwitchClient
                    .SendMessage(
                        AivaClient.Instance.Channel,
                        "Roulette started",
                        AivaClient.DryRun);

                if (_shouldStop) {
                    _isRunning = false;
                    _shouldStop = false;
                    return;
                }

                registeredUsers = new List<User>();

                AivaClient.Instance.TwitchClient.OnChatCommandReceived
                    += ChatMessageReceived;

                if (_shouldStop) {
                    _isRunning = false;
                    _shouldStop = false;
                    return;
                }

                await Task.Delay(timeActiveRoulette);
                AivaClient.Instance.TwitchClient
                    .SendMessage(
                        AivaClient.Instance.Channel,
                        "Roulette closed",
                        AivaClient.DryRun);

                if (_shouldStop) {
                    _isRunning = false;
                    _shouldStop = false;
                    return;
                }

                await Task.Delay(timeToWaitForWinner);

                if (_shouldStop) {
                    _isRunning = false;
                    _shouldStop = false;
                    return;
                }

                SpinTheWheel();

                if (_shouldStop) {
                    _isRunning = false;
                    _shouldStop = false;
                    return;
                }

                var winners = registeredUsers
                    .Where(w => w.IsWon);

                var stringBuilder = new StringBuilder();

                if (_shouldStop) {
                    _isRunning = false;
                    _shouldStop = false;
                    return;
                }

                stringBuilder.Append($"Winning Number is {_winningNumber}.");
                if (winners.Any()) {
                    foreach (var winner in winners) {
                        stringBuilder.Append($"{winner.Name} - {winner.WonSum} ");
                    }

                    stringBuilder.Append(" Winner is: ");
                } else {
                    stringBuilder.Append("No winners");
                }

                if (stringBuilder.ToString().Any()) {
                    AivaClient.Instance.TwitchClient.SendMessage(
                        AivaClient.Instance.Channel,
                        stringBuilder.ToString(),
                        AivaClient.DryRun);
                }

                _isRunning = false;
            }
        }

        public void StopRoulette() {
            _shouldStop = true;
            AivaClient.Instance.TwitchClient.OnChatCommandReceived -= ChatMessageReceived;
            registeredUsers = null;
        }

        private void SpinTheWheel() {
            _winningNumber = _randomGenerator.Next(minNumber, maxNumber);
            var wonItem = _tableLayout.AllNumbers.FirstOrDefault(w => w.Number == _winningNumber);

            foreach(var user in registeredUsers) {
                if(user.BetType == BetTypes.Number) {
                    if(_winningNumber == user.BetValue) {
                        user.IsWon = true;
                        user.WonSum = user.BetValue * 36;
                    }
                } else if(user.BetType == BetTypes.Black && !wonItem.IsRed) {
                    user.IsWon = true;
                    user.WonSum = user.BetValue * 2;
                } else if (user.BetType == BetTypes.Red && wonItem.IsRed) {
                    user.IsWon = true;
                    user.WonSum = user.BetValue * 2;
                } else if (user.BetType == BetTypes.Even && wonItem.IsEven) {
                    user.IsWon = true;
                    user.WonSum = user.BetValue * 2;
                } else if (user.BetType == BetTypes.Odd && !wonItem.IsEven) {
                    user.IsWon = true;
                    user.WonSum = user.BetValue * 2;
                }

                if(user.IsWon) {
                    _databaseCurrencyHandler.Add.Add(user.UserId, user.WonSum);
                }
            }
        }

        private void ChatMessageReceived(object sender, OnChatCommandReceivedArgs e) {
            // check if correct command
            if(e.Command.CommandText.ToLower() != "roulette") {
                return;
            }

            // check if arguments are valid
            if(e.Command.ArgumentsAsList.Count < 2) {
                return;
            }

            if(int.TryParse(e.Command.ArgumentsAsList[0], out int rouletteBet)) {
                var userId = Convert.ToInt32(e.Command.ChatMessage.UserId);

                // check if already registered
                if(registeredUsers.Any(u => u.UserId == userId)) {
                    return;
                }

                // check if user has enough currency
                if (!_databaseCurrencyHandler.HasUserEnoughCurrency(userId, rouletteBet)) {
                    return;
                } else {
                    _databaseCurrencyHandler.Remove.Remove(userId, rouletteBet);
                }

                var betType = GetBetType(e.Command.ArgumentsAsList[1]);
                if (betType == BetTypes.Unknown) return;

                var user = new User {
                    Name = e.Command.ChatMessage.DisplayName,
                    UserId = userId,
                    BetValue = rouletteBet,
                    BetType = betType
                };

                registeredUsers.Add(user);
            }
        }

        private BetTypes GetBetType(string type) {
            if(int.TryParse(type, out int number)) {
                return BetTypes.Number;
            }

            switch(type.ToLower()) {
                case "odd":
                    return BetTypes.Odd;
                case "even":
                    return BetTypes.Even;
                case "red":
                    return BetTypes.Red;
                case "black":
                    return BetTypes.Black;
                default:
                    return BetTypes.Unknown;
            }
        }
    }
}
