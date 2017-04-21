using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Dashboard {
    public class Games {
        /// <summary>
        /// Get Twitch Games
        /// </summary>
        /// <returns></returns>
        public static List<string> GetTwitchGames() {
            var games = TwitchLib.TwitchApi.Games.GetGamesByPopularity(100);

            var returnList = new List<string>();
            foreach (var game in games) {
                returnList.Add(game.Game.Name);
            }

            return returnList;
        }

        /// <summary>
        /// Get Twitch Games async
        /// </summary>
        /// <returns></returns>
        public async static Task<List<string>> GetTwitchGamesAsync() {
            return await Task.Run(() => GetTwitchGames());
        }

        /// <summary>
        /// Change the Game
        /// </summary>
        /// <param name="game">Game Title</param>
        public static async void ChangeGame(string game) {
            await TwitchLib.TwitchApi.Streams.UpdateStreamGameAsync(game, Core.Client.AivaClient.Client.Channel);
        }
    }
}
