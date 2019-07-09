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
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using G1ANT.Engine;
using G1ANT.Language;

using NUnit.Framework;
using G1ANT.Addon.Images.Tests.Properties;
using System.Reflection;
using System.Diagnostics;

namespace G1ANT.Addon.Images.Tests
{
    [TestFixture]
    public class WaitForImageTests
    {
        private const string TextChar = SpecialChars.Text;
        private const int OneSecond = 1000;
        Process testerApp = null;
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
            string path = @"C:\lolAndLol\lol.png";

            Scripter scripter = new Scripter();

            scripter.Text = $"waitfor.image image {SpecialChars.Text}{path}{SpecialChars.Text}";

            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.Run();
            });
            Assert.IsInstanceOf<DirectoryNotFoundException>(exception.GetBaseException());
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void WaitForExistingImageTest()
        {
            testerApp = SDK.Tester.RunFormTester("Title TestApp");

            Color color = Color.Gainsboro;
            string colorCode = "FFDCDCDC";

            RobotWin32.ShowWindow(testerApp.MainWindowHandle, RobotWin32.ShowWindowEnum.ShowNormal);

            Scripter scripter = new Scripter();
            scripter.InitVariables.Clear();
            scripter.RunLine("window TestApp");
           scripter.InitVariables.Add(nameof(colorCode), new TextStructure(colorCode));
            scripter.Text = $@"keyboard {TextChar}FocusOnControl tbColorRGB{TextChar}
				            keyboard {SpecialChars.KeyBegin}enter{SpecialChars.KeyEnd}
                            keyboard {TextChar}{SpecialChars.Variable}{nameof(colorCode)}{TextChar} 
                            keyboard {SpecialChars.KeyBegin}enter{SpecialChars.KeyEnd}";

            scripter.Run();

            string path1 = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.littleGrayRectangle), "bmp");

            scripter.RunLine($"waitfor.image image {SpecialChars.Text}{path1}{SpecialChars.Text}");
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void WaitForImageTest()
        {
            Color color = Color.Pink;
            string colorCode = "FFFFC0CB";

            string path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.smallPink), "bmp");

            Scripter scripter = new Scripter();

            testerApp = SDK.Tester.RunFormTester($"CenterOfScreen FocusOnControl tbColorRGB ChangeColor {colorCode} ");
            RobotWin32.ShowWindow(testerApp.MainWindowHandle, RobotWin32.ShowWindowEnum.ShowNormal);

            scripter.Text = $"waitfor.image image {SpecialChars.Text}{path}{SpecialChars.Text} timeout 8000";
            scripter.Run();
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void WaitForNotExistingImageTest()
        {
            string path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.purpleInYellow), "bmp");

            Scripter scripter = new Scripter();

            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.RunLine($"waitfor.image image {SpecialChars.Text}{path}{SpecialChars.Text} timeout 1000");
            });
            Assert.IsInstanceOf<TimeoutException>(exception.GetBaseException());
        }

        [Test, Timeout(ImagesTests.TestsTimeout)]
        public void BadThresholdValueTest()
        {
            string image = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.smallBlack), "bmp");
            Scripter scripter = new Scripter();
            scripter.Text = $"waitfor.image image {SpecialChars.Text}{image}{SpecialChars.Text} threshold 54";
            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.Run();
            });
            Assert.IsInstanceOf<ArgumentOutOfRangeException>(exception.GetBaseException());
        }

        [TearDown]
        public void Cleanup()
        {
            if (testerApp != null && !testerApp.HasExited)
            {
                testerApp.Kill();
            }
        }
    }
}
