using System;
using System.IO;
using System.Collections.Generic;

using NUnit.Framework;
using System.Reflection;
using G1ANT.Engine;
using G1ANT.Addon.Images.Tests.Properties;
using G1ANT.Language;

namespace G1ANT.Addon.Images.Tests
{
    [TestFixture]
    public class ImageExpectedTests
    {
        private List<string> paths = new List<string>();
        private const string TextChar = SpecialChars.Text;
        private System.Diagnostics.Process testerApp;
        [SetUp]
        public void Init()
        {
            Language.Addon addon = Language.Addon.Load(@"G1ANT.Addon.Net.dll");
        }
        [OneTimeSetUp]
        public void Initialize()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void NotExistingImageTest()
        {
            string path = @"C:\bagno\bagno\bagno\lol.bmp";

            Scripter scripter = new Scripter();
scripter.InitVariables.Clear();

            scripter.Text = $"image.expected image1 {SpecialChars.Text}{path}{SpecialChars.Text}";

            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.Run();
            });
            Assert.IsInstanceOf<DirectoryNotFoundException>(exception.GetBaseException());

        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void ExpectImageInImagesTest()
        {
            string image1 = Assembly.GetExecutingAssembly().UnpackResourceToFile(nameof(Resources.greenRectangle), "bmp");
            string image2 = Assembly.GetExecutingAssembly().UnpackResourceToFile(nameof(Resources.greenInRed), "bmp");

            Scripter scripter = new Scripter();
scripter.InitVariables.Clear();

            scripter.Text = $"image.expected image1 {SpecialChars.Text}{image1}{SpecialChars.Text} image2 {SpecialChars.Text}{image2}{SpecialChars.Text}";
            scripter.Run();

            var result = scripter.Variables.GetVariableValue<bool>("result");
            Assert.AreEqual(true, result);          
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void ExpectImageOnScreenTest()
        {
            string colorCode = "FFF0F8FF";
            testerApp = SDK.Tester.RunFormTester("Title TestApp");

            Scripter scripter = new Scripter();
scripter.InitVariables.Clear();
            scripter.RunLine("window TestApp");
           scripter.InitVariables.Add(nameof(colorCode), new TextStructure(colorCode));
            scripter.Text = $@"keyboard {TextChar}FocusOnControl tbColorRGB{TextChar}
				            keyboard {SpecialChars.KeyBegin}enter{SpecialChars.KeyEnd}
                            keyboard {TextChar}{SpecialChars.Variable}{nameof(colorCode)}{TextChar} 
                            keyboard {SpecialChars.KeyBegin}enter{SpecialChars.KeyEnd}";
            scripter.Run();

            string image1 = Assembly.GetExecutingAssembly().UnpackResourceToFile(nameof(Resources.FFF0F8FF), "bmp");
            scripter.RunLine($"image.expected image1 {SpecialChars.Text}{image1}{SpecialChars.Text}");
            var result = scripter.Variables.GetVariableValue<bool>("result");
            Assert.IsTrue(result);           
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void BadThresholdValueTest()
        {
            string image = Assembly.GetExecutingAssembly().UnpackResourceToFile(nameof(Resources.blackRectangle), "bmp");
            paths.Add(image);

            Scripter scripter = new Scripter();
scripter.InitVariables.Clear();

            scripter.Text = $"image.expected image1 {SpecialChars.Text}{image}{SpecialChars.Text} threshold -16";

            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.Run();
            });
            Assert.IsInstanceOf<ArgumentOutOfRangeException>(exception.GetBaseException());
        }

        [TearDown]
        public void DeleteCreatedImages()
        {
            if (testerApp != null && !testerApp.HasExited)
            {
                testerApp.Kill();
            }
        }
    }
}
