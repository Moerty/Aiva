using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aiva.Core.Test {
    [TestClass]
    public class TextTest {
        [TestMethod]
        public void Text_LoadFile() {
            var result = new Config.Text();

            Assert.IsNotNull(Config.Text.Instance);
        }
    }
}