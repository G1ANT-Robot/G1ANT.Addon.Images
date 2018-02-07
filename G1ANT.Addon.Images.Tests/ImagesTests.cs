/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Images
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using G1ANT.Language;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace G1ANT.Addon.Images.Tests
{
    public class ImagesTests
    {
        public const int TestsTimeout = 20000;


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        public static Process StartFormTester(string arguments)
        {
            string location = null;//TODO typeof(Robot.FormTester.G1ANTRobotFormTester).Assembly.Location;
            var process = Process.Start(location, arguments);
            while (process.MainWindowHandle == IntPtr.Zero)
            {
                System.Threading.Thread.Sleep(20);
            }
            process.WaitForInputIdle();
            Thread.Sleep(5000);
            while (IsIconic(process.MainWindowHandle))
            {
                RobotWin32.ShowWindow(process.MainWindowHandle, RobotWin32.ShowWindowEnum.Show);
            }
            process.WaitForInputIdle();
            return process;
        }
    }
}
