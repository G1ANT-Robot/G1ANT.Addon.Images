using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

using G1ANT.Engine;
using G1ANT.Language;

using NUnit.Framework;
using System.Reflection;
using G1ANT.Addon.Images.Tests.Properties;

namespace G1ANT.Addon.Images.Tests
{
    [TestFixture]
    public class ImageFindTests
    {
        private List<string> paths = new List<string>();
        private const string TextChar = SpecialChars.Text;
        private System.Diagnostics.Process testerApp;
        private const int OneSecond = 1000;
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
            string path = @"C:\ble\ble\ble.png";
            Scripter scripter = new Scripter();
scripter.InitVariables.Clear();
            scripter.Text = $"image.find image1 {SpecialChars.Text}{path}{SpecialChars.Text}";

            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.Run();
            });
            Assert.IsInstanceOf<DirectoryNotFoundException>(exception.GetBaseException());

        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void FindImageInImageTest()
        {
            int size = 20;
            Point offset = new Point(3, 11);

            string image1 = Assembly.GetExecutingAssembly().UnpackResourceToFile(nameof(Resources.greenRectangle), "bmp");
            string image2 = Assembly.GetExecutingAssembly().UnpackResourceToFile(nameof(Resources.greenInRed), "bmp");

            Scripter scripter = new Scripter();
scripter.InitVariables.Clear();

            scripter.Text = $"image.find image1 {SpecialChars.Text}{image1}{SpecialChars.Text} image2 {SpecialChars.Text}{image2}{SpecialChars.Text} offsetx {offset.X} offsety {offset.Y}";
            scripter.Run();

            var result = scripter.Variables.GetVariableValue<Point>("result");

            Assert.AreEqual(373, result.X);
            Assert.AreEqual(113, result.Y);
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void FindImageInImageWithThresholdTest()
        {
            Color color2 = Color.FromArgb(0, 20, 255);
            string image1 = Assembly.GetExecutingAssembly().UnpackResourceToFile(nameof(Resources.greenRectangle), "bmp");
            string image2 = Assembly.GetExecutingAssembly().UnpackResourceToFile(nameof(Resources.greenInRed), "bmp");
            paths.Add(image1);
            paths.Add(image2);

            Scripter scripter = new Scripter();
scripter.InitVariables.Clear();

            scripter.Text = $"image.find image1 {SpecialChars.Text}{image1}{SpecialChars.Text} image2 {SpecialChars.Text}{image2}{SpecialChars.Text} threshold 0,1";
            scripter.Run();
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void FindImageInDifferentImageTest()
        {
            string image1 = Assembly.GetExecutingAssembly().UnpackResourceToFile(nameof(Resources.greenRectangle), "bmp");
            string image2 = Assembly.GetExecutingAssembly().UnpackResourceToFile(nameof(Resources.blackRectangle), "bmp");
            paths.Add(image1);
            paths.Add(image2);

            Scripter scripter = new Scripter();
scripter.InitVariables.Clear();

            scripter.Text = $"image.find image1 {SpecialChars.Text}{image1}{SpecialChars.Text} image2 {SpecialChars.Text}{image2}{SpecialChars.Text}";
            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.Run();
            });
            Assert.IsInstanceOf<ArgumentException>(exception.GetBaseException());
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void FindImageOnScreenTest()
        {
            string colorCode = "00000000";

            string image = Assembly.GetExecutingAssembly().UnpackResourceToFile(nameof(Resources.littleWhite), "bmp");
            
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
            scripter.Text = $"image.find image1 {SpecialChars.Text}{image}{SpecialChars.Text}";
            scripter.Run();

            Thread.Sleep(OneSecond * 2);
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void BadThresholdValueTest()
        {
            string image = Assembly.GetExecutingAssembly().UnpackResourceToFile(nameof(Resources.greenRectangle), "bmp");
            paths.Add(image);

            Scripter scripter = new Scripter();
scripter.InitVariables.Clear();

            scripter.Text = $"image.find image1 {SpecialChars.Text}{image}{SpecialChars.Text} threshold 6";

            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.Run();
            });
            Assert.IsInstanceOf<ArgumentOutOfRangeException>(exception.GetBaseException());
        }

        [TearDown]
        public void DeleteCreatedImages()
        {
            if (!testerApp?.HasExited ?? false)
            {
                testerApp.Kill();
            }
        }
    }
}
