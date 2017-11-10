using TwitchLib.Extensions.Client;

namespace Aiva.Core.Client.Internal {
    public class Stream {
        /// <summary>
        /// Unmod a User
        /// </summary>
        /// <param name="username"></param>
        public static void UnmodUser(string username) {
            Core.AivaClient.Instance.AivaTwitchClient.Unmod(AivaClient.Instance.Channel, username);
        }

        /// <summary>
        /// Mod a User
        /// </summary>
        /// <param name="username"></param>
        public static void ModUser(string username) {
            Core.AivaClient.Instance.AivaTwitchClient.Mod(Core.AivaClient.Instance.Channel, username);
        }
    }
}