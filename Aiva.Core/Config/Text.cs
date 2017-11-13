using System.Resources;

namespace Aiva.Core.Config {
    public class Text {
        private static ResourceManager _Instance;

        public static ResourceManager Instance {
            get {
                return _Instance ?? (_Instance = new ResourceManager("Aiva.Core.ConfigFiles.AivaText", System.Reflection.Assembly.GetExecutingAssembly()));
            }
        }

        public Text() {
            _Instance = new ResourceManager("Aiva.Core.ConfigFiles.AivaText", System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}