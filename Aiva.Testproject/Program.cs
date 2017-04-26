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
            CefSharp.Cef.Initialize();
            ChromiumWebBrowser b = new ChromiumWebBrowser();
            while (!CefSharp.Cef.IsInitialized) {
                Console.WriteLine($"{DateTime.Now.TimeOfDay.ToString()} : Waiting....");
            }
            p = new Aiva.Extensions.Songrequest.Player();


            Aiva.Extensions.Songrequest.Song s = new Extensions.Songrequest.Song("https://www.youtube.com/watch?v=T5UsrAxid74", "aeffchaen");
            //p.SetVideoID("62Mr0elsf0s");

            p.ChangeSong(s, true);

            Console.ReadKey();
        }
    }
}