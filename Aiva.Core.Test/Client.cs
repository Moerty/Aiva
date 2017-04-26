using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aiva.Core.Test {
    [TestClass]
    public class ClientTest {
        [TestMethod]
        public void Client_ClientConnection() {
            bool exptectedResult = true;

            var config = new Core.Config.Config();
            var client = new Aiva.Core.AivaClient();

            Assert.AreEqual(client.AivaTwitchClient.IsConnected, exptectedResult);
        }
    }
}
