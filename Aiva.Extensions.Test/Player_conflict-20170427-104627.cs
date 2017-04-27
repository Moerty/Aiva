using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aiva.Extensions.Test {
    [TestClass]
    public class Player {
        [TestMethod]
        public void TestPlayer() {

            var p = new Aiva.Extensions.Songrequest.Player();

            p.Browser.BrowserInitialized += Browser_BrowserInitialized;
        }

        private void Browser_BrowserInitialized(object sender, EventArgs e) {
            throw new NotImplementedException();
        }
    }
}
