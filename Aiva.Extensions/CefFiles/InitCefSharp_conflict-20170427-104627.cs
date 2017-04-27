//using CefSharp;
//using System;
//using System.IO;
//using System.Reflection;
//using System.Runtime.CompilerServices;

//namespace Aiva.Extensions.CefFiles {
//    public class InitCefSharp {
//        public static bool IsInit { get; private set; }

//        public static void Init() {
//            //Add Custom assembly resolver
//            AppDomain.CurrentDomain.AssemblyResolve += Resolver;

//            //Any CefSharp references have to be in another method with NonInlining
//            // attribute so the assembly rolver has time to do it's thing.
//            InitializeCefSharp();
//        }


//        [MethodImpl(MethodImplOptions.NoInlining)]
//        private static void InitializeCefSharp() {
//            var settings = new CefSettings() {

//                // Set BrowserSubProcessPath based on app bitness at runtime
//                BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
//                                                   Environment.Is64BitProcess ? "x64" : "x86",
//                                                   "CefSharp.BrowserSubprocess.exe")
//            };

//            // Make sure you set performDependencyCheck false
//            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

//            IsInit = true;
//        }

//        // Will attempt to load missing assembly from either x86 or x64 subdir
//        // Required by CefSharp to load the unmanaged dependencies when running using AnyCPU
//        private static Assembly Resolver(object sender, ResolveEventArgs args) {
//            if (args.Name.StartsWith("CefSharp")) {
//                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
//                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
//                                                       Environment.Is64BitProcess ? "x64" : "x86",
//                                                       assemblyName);

//                return File.Exists(archSpecificPath)
//                           ? Assembly.LoadFile(archSpecificPath)
//                           : null;
//            }

//            return null;
//        }
//    }
//}
