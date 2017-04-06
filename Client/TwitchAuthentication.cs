using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client {
    public class TwitchAuthentication {

        //private TwitchAuthentication _Instance;
        //public TwitchAuthentication Instance {
        //    get {
        //        if (_Instance == null)
        //            _Instance = new TwitchAuthentication();

        //        return _Instance;
        //    }
        //    private set {
        //        _Instance = value;
        //    }
        //}


        private static readonly TwitchAuthentication instance = new TwitchAuthentication();

        public static TwitchAuthentication Instance {
            get {
                return instance;
            }
        }

        private TwitchAuthentication() {

        }



        public TwitchAuthentication GetAuthenticationValues() {

            //Startup s = new Startup();
            //IDisposable app;

            //using (app = WebApp.Start<Startup>("http://localhost:12345")) {
            //    while(true) {
            //        Task.Delay(500);
            //    }
            //}

            HttpListener Listener = new HttpListener();

            Listener.Prefixes.Add("Listener.Prefixes.Add(„http://localhost:9090/");
            Listener.Start();
            Listener.BeginGetContext(new AsyncCallback(ListenerCallback), Listener);


            return null;
        }

        public TwitchAuthentication.AuthenticationModel GetModel(string token, string scopes) {
            return new TwitchAuthentication.AuthenticationModel {
                Token = token,
                Scopes = scopes
            };
        }


        public class Startup {
            public AuthenticationModel Model;

            

            public void Configuration(IAppBuilder app) {
                app.Run(context => {
                    var token = context.Request.Query.Get("access_token");
                    var scopes = context.Request.Query.Get("scope");

                    //Task<AuthenticationModel> task = Task<TwitchAuthentication>.GetModel(token, scopes);

                    Model = GetInternModel();

                    AuthenticationModel GetInternModel()
                    {
                        return TwitchAuthentication.Instance.GetModel(token, scopes);
                    }


                    context.Response.ContentType = "text/plain";
                    return context.Response.WriteAsync("Close this window pls.");
                });
            }
        }

        public class AuthenticationModel {
            public string Token { get; set; }
            public string Scopes { get; set; }
        }
    }
}
