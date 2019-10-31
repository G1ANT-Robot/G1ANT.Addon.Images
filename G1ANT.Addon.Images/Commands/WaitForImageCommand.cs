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
using System.Drawing;
using System.Windows.Forms;
using G1ANT.Language;

namespace G1ANT.Addon.Images
{
    [Command(Name = "waitfor.image", Tooltip = "This command waits for a specified image to appear on the screen and returns the coordinates of the matching image")]
    public class WaitForImageCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Path to a file with an awaited image")]
            public TextStructure Image { get; set; }

            [Argument(Tooltip = "Specifies whether the search should be done relatively to the active window")]
            public BooleanStructure Relative { get; set; } = new BooleanStructure(true);

            [Argument(Tooltip = "Tolerance threshold (0-1 range); the default 0 means it has to be a 100% match")]
            public FloatStructure Threshold { get; set; } = new FloatStructure(0);

            [Argument(Tooltip = "Narrows the search area to a rectangle specified by coordinates in the `x0⫽y0⫽x1⫽y1` format, where `x0⫽y0` and `x1⫽y1` are the pixel coordinates in the top left corner and the bottom right corner of the rectangle, respectively")]
            public RectangleStructure ScreenSearchArea { get; set; } = new RectangleStructure(new Rectangle(0,0,SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height));

            [Argument(Tooltip = "If specified, the resulting point will be placed in the center of the matching area")]
            public BooleanStructure CenterResult { get; set; } = new BooleanStructure(true);

            [Argument(Tooltip = "Value that will be added to the result's X coordinate")]
            public IntegerStructure OffsetX { get; set; } = new IntegerStructure(0);

            [Argument(Tooltip = "Value that will be added to the result's Y coordinate")]
            public IntegerStructure OffsetY { get; set; } = new IntegerStructure(0);

            [Argument(Required = true, DefaultVariable = "timeoutimagefind", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public override TimeSpanStructure Timeout { get; set; }

            [Argument(Tooltip = "Name of a variable where the command's result will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");            
        }

        public WaitForImageCommand(AbstractScripter scripter) : base(scripter) { }

        public void Execute(Arguments arguments)
        {
            if (arguments.Threshold.Value < 0 || arguments.Threshold.Value > 1)
            {
                throw new ArgumentOutOfRangeException("Threshold must be a value from 0 to 1.");
            }

            using (var template = arguments.Image.OpenImage())
            {
                var timeout = (int)arguments.Timeout.Value.TotalMilliseconds;
                var start = Environment.TickCount;
                var foundRectangle = Rectangle.Empty;

                while (Math.Abs(Environment.TickCount - start) < timeout && Scripter.Stopped == false && foundRectangle == Rectangle.Empty)
                {
                    using (var source = arguments.ScreenSearchArea.GetScreenshot(arguments.Relative))
                    {
                        foundRectangle = source.MatchTemplate(template, arguments.Threshold);
                        Application.DoEvents();
                    }
                }

                if (foundRectangle == Rectangle.Empty)
                {
                    throw new TimeoutException("Image was not found in specified search area.");
                }
                else
                {
                    var foundPoint = foundRectangle.GetPoint(arguments.CenterResult, arguments.OffsetX, arguments.OffsetY);
                    Scripter.Variables.SetVariableValue(arguments.Result.Value, new PointStructure(foundPoint));
                }
            }
        }
    }
}
