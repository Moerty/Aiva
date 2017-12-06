namespace Aiva.Models.Extensions {
    public static class Helpers {
        /// <summary>
        /// Add '!' to the command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string ModCommand(this string command) {
            return (command
                .StartsWith("!")
                    ? command
                    : $"!{command}")
                .TrimEnd('!'); // trim end cause writer can write something before automated '!'
        }
    }
}