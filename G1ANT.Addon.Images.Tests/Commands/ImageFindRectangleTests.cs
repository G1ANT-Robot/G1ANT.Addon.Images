/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Images
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using System.Collections.Generic;

using G1ANT.Engine;
using NUnit.Framework;
using System.Reflection;
using G1ANT.Language;
using System;
using G1ANT.Addon.Images.Tests.Properties;

namespace G1ANT.Addon.Images.Tests
{
    [TestFixture]
    public class ImageFindRectangleTests
    {
        [SetUp]
        public void Init()
        {
            Language.Addon addon = Language.Addon.Load(@"G1ANT.Addon.Net.dll");
        }
        [OneTimeSetUp]
        public void Initialize()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
            scripter = new Scripter();
        }

        private Scripter scripter;

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void FindAllTest()
        {
            int expectedRectanglesCount = 8;
            string imageWithRectanglesPath = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.Rectangles8), "png");
            scripter.Text = ($"image.findrectangles {SpecialChars.Text}{imageWithRectanglesPath}{SpecialChars.Text}");
            scripter.Run();
            Assert.AreEqual(expectedRectanglesCount, scripter.Variables.GetVariableValue<List<Object>>("result").Count);
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void WidthFilterTest()
        {
            int widthTreshold = 180*100/1900;
            int expectedRectanglesCount = 2;
            int expectedRectanglesCount2 = 6;
            string imageWithRectanglesPath2 = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.Rectangles8), "png");
            string imageWithRectanglesPath = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.Rectangles8), "png");

            scripter.Text = ($@"image.findrectangles {SpecialChars.Text}{imageWithRectanglesPath}{SpecialChars.Text} maxwidth {widthTreshold}
                                image.findrectangles {SpecialChars.Text}{imageWithRectanglesPath2}{SpecialChars.Text} minwidth {widthTreshold} result {SpecialChars.Variable} result2");
            scripter.Run();
            Assert.AreEqual(expectedRectanglesCount, scripter.Variables.GetVariableValue<List<Object>>("result").Count);
            Assert.AreEqual(expectedRectanglesCount2, scripter.Variables.GetVariableValue<List<Object>>("result2").Count);
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void HeigthFilterTest()
        {
            int maxHeight = 250*100/850;
            int minHeight = 80*100/850;
            int expectedRectanglesCount = 5;
            string imageWithRectanglesPath = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.Rectangles8), "png");

            scripter.RunLine($"image.findrectangles {SpecialChars.Text}{imageWithRectanglesPath}{SpecialChars.Text} maxheight {maxHeight} minheight {minHeight}");
            Assert.AreEqual(expectedRectanglesCount, scripter.Variables.GetVariableValue<List<Object>>("result").Count);
        }
    }
}
