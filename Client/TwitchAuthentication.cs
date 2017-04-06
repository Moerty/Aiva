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
        private static readonly TwitchAuthentication instance = new TwitchAuthentication();
        public static TwitchAuthentication Instance {
            get {
                return instance;
            }
        }

        HttpListener listener = new HttpListener();
        string prefix = "http://localhost:12345/";

        private TwitchAuthentication() {
            listener.Prefixes.Add(prefix);
        }

        /// <summary>
        /// Get Twitch Authentication Async
        /// </summary>
        /// <returns></returns>
        public async Task<AuthenticationModel> GetAuthenticationValuesAsync() {
            return await Task.Run(() => GetAuthenticationValues());
        }

        /// <summary>
        /// Start the listener
        /// </summary>
        /// <returns>returns IsListening Value</returns>
        private bool StartListener() {
            try {
                listener.Start();
            }
            catch (HttpListenerException ex) {
                throw new Exception("Cant start listener for TwitchAuthentication");
            }

            return listener.IsListening;
        }

        /// <summary>
        /// Stop the listener
        /// </summary>
        private void StopListener() {
            listener.Stop();
        }

        public AuthenticationModel GetAuthenticationValues() {

            StartListener();

            AuthenticationModel Values = null;
            while(listener.IsListening) {
                var context = listener.GetContext();

                if (context.Request.QueryString.HasKeys()) {
                    if (context.Request.RawUrl.Contains("access_token")) {
                        Uri myUri = new Uri(context.Request.Url.OriginalString);
                        string scope = HttpUtility.ParseQueryString(myUri.Query).Get("scope");
                        string access_token = HttpUtility.ParseQueryString(myUri.Query).Get(0).Replace("access_token=", "");

                        if(!String.IsNullOrEmpty(scope) && !String.IsNullOrEmpty(access_token)) {
                            Values = GetModel(access_token, scope);
                        }
                    }
                }

                byte[] b = Encoding.UTF8.GetBytes(GetResponse());
                context.Response.StatusCode = 200;
                context.Response.KeepAlive = false;
                context.Response.ContentLength64 = b.Length;

                var output = context.Response.OutputStream;
                output.Write(b, 0, b.Length);
                context.Response.Close();

                if(Values != null) {
                    StopListener();
                    return Values;
                }

            }

            return null;
        }

        /// <summary>
        /// Creates the Response for TwitchOAuth
        /// </summary>
        /// <returns>Response</returns>
        private string GetResponse() {
            StringBuilder builder = new StringBuilder();

            builder.Append("<html>");
            builder.Append(Environment.NewLine);
            builder.Append("<head>");
            builder.Append(Environment.NewLine);
            builder.Append("<title>BlackBoxBot Twitch Oauth</title>");
            builder.Append(Environment.NewLine);
            builder.Append("<script language=\"JavaScript\">");
            builder.Append(Environment.NewLine);
            builder.Append("if(window.location.hash) {");
            builder.Append(Environment.NewLine);
            builder.Append("window.location.href = window.location.href.replace(\"/#\",\"?=\");");
            builder.Append(Environment.NewLine);
            builder.Append("}");
            builder.Append(Environment.NewLine);
            builder.Append("</script>");
            builder.Append(Environment.NewLine);
            builder.Append("</head>");
            builder.Append(Environment.NewLine);
            builder.Append("<body>You can close this tab</body>");
            builder.Append(Environment.NewLine);
            builder.Append("</html>");

            return builder.ToString();
        }

        /// <summary>
        /// Creates the Model to return
        /// </summary>
        /// <param name="token">Twitch Token</param>
        /// <param name="scopes">Twitch Scopes</param>
        /// <returns></returns>
        private TwitchAuthentication.AuthenticationModel GetModel(string token, string scopes) {
            return new TwitchAuthentication.AuthenticationModel {
                Token = token,
                Scopes = scopes
            };
        }

        /// <summary>
        /// Starts the Request for the User to authorize for twitch
        /// </summary>
        public void SendRequestToBrowser() {
            Thread.Sleep(500);

            string urlS = getUrl();
            Uri uri = new Uri(urlS);

            System.Diagnostics.Process.Start(urlS);
        }

        /// <summary>
        /// Returns the URL which we call to create a oauth token
        /// </summary>
        /// <returns></returns>
        private static string getUrl() {
            var sb = new StringBuilder();
            sb.Append("https://api.twitch.tv/kraken/oauth2/authorize");
            sb.Append("?response_type=token");
            sb.Append("&client_id=1vvme3nbvmh7ylljb35mb7xl3dokdv");
            sb.Append("&redirect_uri=http://localhost:12345");
            sb.Append("&scope=user_read+user_blocks_edit+user_blocks_read+user_follows_edit+channel_read+channel_editor+channel_commercial+channel_stream+channel_subscriptions+user_subscriptions+channel_check_subscription+chat_login+channel_feed_read+channel_feed_edit");
            return sb.ToString();
        }

        /// <summary>
        /// Model
        /// </summary>
        public class AuthenticationModel {
            public string Token { get; set; }
            public string Scopes { get; set; }
        }
    }
}
