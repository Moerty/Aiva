using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiva.Extensions.Models;
using System.Windows;
using CefSharp;
using System.IO;

namespace Aiva.Extensions.Songrequest
{
    public class Player2
    {
        private bool _browserLoaded = false;
        private bool _iframePlayerLoaded = false;
        private bool _startupSettingsRun = false;

        public int Volume { get; set; }

        bool haventRun = true;

        public ChromiumWebBrowser Browser;

        public Models.Songrequest.YoutubePlayerState PlayerState;


        public Player2()
        {
            SetupBrowser();
        }

        //public static readonly DependencyProperty VideoIdProperty =
        //    DependencyProperty.Register("VideoId", typeof(string), typeof(Player2), new PropertyMetadata("", VideoIdChanged));


        
        private void SetupBrowser()
        {
            CefSharp.Cef.Initialize();
            Browser = new ChromiumWebBrowser();
            Browser.LoadingStateChanged += Browser_LoadingStateChanged;
            var bound = new CefFiles.BoundObject();
            bound.PlayerLoadingDone += JavascriptReady;
            bound.PlayerPlayingChanged += BoundOnPlayerPlayingChanged;
            Browser.RegisterJsObject("bound", bound);

            Browser.BrowserInitialized += Browser_BrowserInitialized;

            
            //Browser.Address = "@custom://CefFiles/YouTube/CefPlayer.html";
        }

        private void Browser_BrowserInitialized(object sender, EventArgs e)
        {


            //Browser.Load("https://www.youtube.com/embed/bKXIUQbardg?autoplay=1");

            Browser.Load(@"custom://CefFiles/YouTube/CefPlayer.html");
            //Browser.LoadHtml(File.ReadAllText(@".\CefFiles\YouTube\CefPlayer.html"));
            Volume = 100;

            SetAutoPlay(true);
            SetVideoId("bKXIUQbardg");
        }

        private void BoundOnPlayerPlayingChanged(object sender, Models.Songrequest.YoutubePlayerState e)
        {
            Application.Current.Dispatcher.Invoke(() => PlayerState = e);
        }

        private void JavascriptReady(object sender, EventArgs e)
        {
            _iframePlayerLoaded = true;
            Application.Current.Dispatcher.Invoke(() => { CheckForStartupSettings(); });
        }

        private void Browser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            if (Browser != null) {
                Browser.LoadingStateChanged -= Browser_LoadingStateChanged;
                _browserLoaded = true;
                //Application.Current.Dispatcher.Invoke(() => { CheckForStartupSettings(); });
                CheckForStartupSettings();
            }
        }

        /// <summary>
        /// checks if everything is up to set default settings on iframe player.
        /// </summary>
        private void CheckForStartupSettings()
        {
            if (_iframePlayerLoaded && _browserLoaded && !_startupSettingsRun) {
                _startupSettingsRun = true;
                SetAutoPlay(false);
                SetVideoId("");
                SetVolume(Volume);
            } else if (_iframePlayerLoaded && _browserLoaded && _startupSettingsRun) {
                Console.WriteLine(string.Format("Trying to call CheckForStartupSettings after already being called once!"));
            }
        }

        public void SetAutoPlay(bool autoPlay)
        {
            if (IsloadingDone()) {
                var script = $"autoPlay = {autoPlay};";
                Browser.ExecuteScriptAsync(script);
            }
        }

        private bool IsloadingDone()
        {
            return _startupSettingsRun;
        }

        public void SetVideoId(string videoId)
        {
            if (haventRun) {
                haventRun = false;
            }

            if (IsloadingDone()) {
                Browser.ExecuteScriptAsync("setVideoId", videoId);
            }
        }

        private void SetVolume(int volume)
        {
            if (IsloadingDone())
                Browser.ExecuteScriptAsync("setVolume", volume);
        }
    }
}
