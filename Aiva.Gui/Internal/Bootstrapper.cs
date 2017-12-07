using CefSharp;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Aiva.Gui.Internal {
    public static class Bootstrapper {
        public static void LoadCefSharp() {
            AppDomain.CurrentDomain.AssemblyResolve += CefSharpResolver;
            InitCefSharp();
            YoutubePlayerLib.Bootstrapper.ConfigureCefSharp();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitCefSharp() {
            var settings = new CefSettings {
                // Set BrowserSubProcessPath based on app bitness at runtime
                BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                   Environment.Is64BitProcess ? "x64" : "x86",
                                                   "CefSharp.BrowserSubprocess.exe")
            };

            var schema = new CefCustomScheme {
                SchemeName = YoutubePlayerLib.Cef.CefSharpSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new YoutubePlayerLib.Cef.CefSharpSchemeHandlerFactory()
            };

            settings.RegisterScheme(schema);

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }

        // Will attempt to load missing assembly from either x86 or x64 subdir
        private static Assembly CefSharpResolver(object sender, ResolveEventArgs args) {
            if (args.Name.StartsWith("CefSharp")) {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                return File.Exists(archSpecificPath)
                           ? Assembly.LoadFile(archSpecificPath)
                           : null;
            }

            return null;
        }
    }
}