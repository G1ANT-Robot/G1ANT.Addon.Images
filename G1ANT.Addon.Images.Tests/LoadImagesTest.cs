using System;
using NUnit.Framework;

namespace G1ANT.Addon.Images.Tests
{
    [TestFixture]
    public class WatsonLoadTests
    {
        [Test]
        public void LoadImagesAddon()
        {
            Language.Addon addon = Language.Addon.Load("G1ANT.Addon.Images.dll");
            Assert.IsNotNull(addon);
            Assert.IsTrue(addon.Commands.Count > 0);
        }
    }
}
