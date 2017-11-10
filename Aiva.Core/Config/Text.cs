using System.Resources;

namespace Aiva.Core.Config {
    public class Text {
        private static ResourceManager _Instance;

        public static ResourceManager Instance {
            get {
                if (_Instance == null)
                    _Instance = new ResourceManager("Aiva.Core.ConfigFiles.AivaText", System.Reflection.Assembly.GetExecutingAssembly());

                return _Instance;
            }
        }

        public Text() {
            _Instance = new ResourceManager("Aiva.Core.ConfigFiles.AivaText", System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}