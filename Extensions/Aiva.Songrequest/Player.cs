using System;
using CefSharp.OffScreen;
using CefSharp;

namespace Songrequest {
    public class Player {
        public static ChromiumWebBrowser Browser;
        private ChromiumWebBrowser browser;

        const string youtube = "https://www.youtube.com/embed/";
        public bool autostart = false;
        public bool IsInit = false;
        private bool IsMusicPlaying = false;

        //Until https://bitbucket.org/chromiumembedded/cef/issues/1995/ is resolved it's nessicary to
        //deal with the spawning of the crashpad process here as it's not possible to configure which exe it uses
        //int exitCode = Cef.ExecuteProcess();

        public Player(bool? autostart) {
            this.browser = new ChromiumWebBrowser();

            if (!Cef.IsInitialized)
                using (var cefSettings = new CefSettings()) {
                    Cef.Initialize(cefSettings, performDependencyCheck: true, browserProcessHandler: null);
                }

            this.autostart = autostart == true ? true : false;

            this.browser.BrowserInitialized += Browser_BrowserInitialized;
        }

        private void Browser_BrowserInitialized(object sender, EventArgs e) {
            IsInit = true;
        }

        public void ChangeSong(string videoid) {
            this.browser.Load(youtube + videoid + (autostart ? "?autoplay=1" : ""));
            IsMusicPlaying = true;
        }

        public void MusicOnOff() {
            if (IsMusicPlaying) {
                browser.GetBrowser().GetHost().SendMouseClickEvent(0, 0, MouseButtonType.Left, false, 1, CefEventFlags.None);
                System.Threading.Thread.Sleep(100);
                browser.GetBrowser().GetHost().SendMouseClickEvent(0, 0, MouseButtonType.Left, true, 1, CefEventFlags.None);
                IsMusicPlaying = false;
            }
        }
    }
}
