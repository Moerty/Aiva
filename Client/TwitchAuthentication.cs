using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Client {
    public class TwitchAuthentication {
        HttpListener listener = new HttpListener();
        string prefix = "http://localhost:12345/";



        private static readonly TwitchAuthentication instance = new TwitchAuthentication();

        public static TwitchAuthentication Instance {
            get {
                return instance;
            }
        }

        private TwitchAuthentication() {
            listener.Prefixes.Add(prefix);
        }

        public async Task<AuthenticationModel> GetAuthenticationValuesAsync() {
            return await Task.Run(() => GetAuthenticationValues());
        }

        public AuthenticationModel GetAuthenticationValues() {
            try {
                listener.Start();
            }
            catch(HttpListenerException ex) {
                return null;
            }

            AuthenticationModel Values = null;
            while(listener.IsListening) {
                var context = listener.GetContext();

                if (context.Request.QueryString.HasKeys()) {
                    if (context.Request.RawUrl.Contains("access_token")) {
                        Uri myUri = new Uri(context.Request.Url.OriginalString);
                        string scope = HttpUtility.ParseQueryString(myUri.Query).Get("scope");
                        string access_token = HttpUtility.ParseQueryString(myUri.Query).Get(0).Replace("access_token=", "");

                        if(!String.IsNullOrEmpty(scope) && !String.IsNullOrEmpty(access_token)) {
                            //listener.Stop();
                            Values = GetModel(access_token, scope);
                        }
                    }
                }

                byte[] b = Encoding.UTF8.GetBytes(File.ReadAllText(@"D:\kill\ConsoleApp1\token.html"));
                context.Response.StatusCode = 200;
                context.Response.KeepAlive = false;
                context.Response.ContentLength64 = b.Length;

                var output = context.Response.OutputStream;
                output.Write(b, 0, b.Length);
                context.Response.Close();

                if(Values != null) {
                    return Values;
                }

            }

            return null;
        }

        private TwitchAuthentication.AuthenticationModel GetModel(string token, string scopes) {
            return new TwitchAuthentication.AuthenticationModel {
                Token = token,
                Scopes = scopes
            };
        }

        public void SendRequestToBrowser() {
            Thread.Sleep(500);

            string urlS = getUrl();
            Uri uri = new Uri(urlS);
            //Uri.es

            //System.Diagnostics.Process.Start((Uri.EscapeDataString(urlS)));
            System.Diagnostics.Process.Start(urlS);
        }

        private static string getUrl() {
            var sb = new StringBuilder();
            sb.Append("https://api.twitch.tv/kraken/oauth2/authorize");
            sb.Append("?response_type=token");
            sb.Append("&client_id=1vvme3nbvmh7ylljb35mb7xl3dokdv");
            sb.Append("&redirect_uri=http://localhost:12345");
            sb.Append("&scope=user_read+user_blocks_edit+user_blocks_read+user_follows_edit+channel_read+channel_editor+channel_commercial+channel_stream+channel_subscriptions+user_subscriptions+channel_check_subscription+chat_login+channel_feed_read+channel_feed_edit");
            return sb.ToString();
        }



        public class AuthenticationModel {
            public string Token { get; set; }
            public string Scopes { get; set; }
        }
    }
}
