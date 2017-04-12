using System.Resources;

namespace Config {
    public class Language {
        private static ResourceManager _Instance;
        public static ResourceManager Instance {
            get {
                if (_Instance == null)
                    _Instance = new System.Resources.ResourceManager("Config.Languages.Text", System.Reflection.Assembly.GetExecutingAssembly());


                return _Instance;
            }
        }
    }
}
