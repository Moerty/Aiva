using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Testproject {
    class Program {
        static Aiva.Extensions.Songrequest.Player p;

        static void Main(string[] args) {

            // Make sure you set performDependencyCheck false
            Cef.Initialize();

            Core.AivaClient.Instance.AivaTwitchClient.SendMessage("Aiva started.");
            TwitchLib.TwitchApi.ValidationAPIRequest("pcwfd4rhcevonwdjw6kdh1g5f8bz1g");
            TwitchLib.TwitchApi.ValidClientId("10n39mbfftkcy2kg1jkzmm62yszdcg");

            // https://www.youtube.com/watch?v=KbNXnxwMOqU
            p = new Extensions.Songrequest.Player();

            p.Browser.BrowserInitialized += Browser_BrowserInitialized;
            p.ChangeSong(new Extensions.Songrequest.Song("5_SLU1ByyKg", "aeffchaen") {
                VideoID = "5_SLU1ByyKg",
            }, true);



            Console.ReadKey();
        }

        private static void Browser_BrowserInitialized(object sender, EventArgs e) {
            p.ChangeSong(new Extensions.Songrequest.Song("KbNXnxwMOqU", "aeffchaen") {
                VideoID = "KbNXnxwMOqU",
            }, true);
        }
    }
}