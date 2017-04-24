using CefSharp;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Aiva.Core.Client;
using Aiva.Core.Database;

namespace Aiva.Bot {
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application {
        private void Application_Startup(object sender, EventArgs e) {
            // Check first Start
            if (File.Exists("ConfigFiles\\general.ini")) {
                // Start Application
                StartMainForm();
            } else {
                var firstStart = new Views.FirstStart.MainStart();
                firstStart.Closing += FirstStart_Closed;
                firstStart.Show();
            }
        }

        private static void SetUpDependencys() {
            // Set Twitch Client
            AivaClient.Client.AivaTwitchClient.SendMessage("Aiva started");
            Extensions.Bankheist.BankheistHandler.RegisterBankheist(); // Register Bankheist

            // CefSharp x64 or x86
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            //Any CefSharp references have to be in another method with NonInlining
            // attribute so the assembly rolver has time to do it's thing.
            InitializeCefSharp();
        }

        private static void StartMainForm() {
            SetUpDependencys();
            var main = new Views.MainWindow();
            main.Closing += Main_Closed;
            main.Closed += ActiveUsersHandler.OnExistProgram;
            main.Closed += UserHandler.UpdateUser.OnExistProgramm;

            main.Show();
        }

        private void FirstStart_Closed(object sender, EventArgs e) {
            if (File.Exists("ConfigFiles\\general.ini")) {
                StartMainForm();
            }
        }

        private static void Main_Closed(object sender, EventArgs e) {
            // Close CefSharp
            CefSharp.Cef.Shutdown();
        }

        #region CefSharp Config
        /// <summary>
        /// Will attempt to load missing assembly from either x86 or x64 subdir
        /// Required by CefSharp to load the unmanaged dependencies when running using AnyCPU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
            if (args.Name.StartsWith("CefSharp")) {
                var assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                var archSpecificPath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                return File.Exists(archSpecificPath)
                           ? System.Reflection.Assembly.LoadFile(archSpecificPath)
                           : null;
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeCefSharp() {
            var settings = new CefSettings {

                // Set BrowserSubProcessPath based on app bitness at runtime
                BrowserSubprocessPath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                           Environment.Is64BitProcess ? "x64" : "x86",
                                           "CefSharp.BrowserSubprocess.exe")
            };

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }

        #endregion CefSharp Config
    }
}
