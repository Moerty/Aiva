using System.Resources;

namespace Aiva.Core.Config {
    public class LanguageConfig {
        private static ResourceManager _Instance;
        public static ResourceManager Instance {
            get {
                if (_Instance == null)
                    _Instance = new ResourceManager("Aiva.Core.Config.Languages.Text", System.Reflection.Assembly.GetExecutingAssembly());


                return _Instance;
            }
        }
    }
}
