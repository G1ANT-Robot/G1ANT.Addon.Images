/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Images
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
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
