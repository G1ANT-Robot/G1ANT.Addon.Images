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
    [Command(Name = "image.find", Tooltip = "This command finds a specified image in another image (or in a part of the screen/entire screen) and returns the coordinates of the matching image")]
    public class ImageFindCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Path to a file with an image be found")]
            public TextStructure Image1 { get; set; }

            [Argument(Tooltip = "Path to a image file in which `image1` will be searched; if not specified, `image1` will be searched on the screen")]
            public TextStructure Image2 { get; set; }

            [Argument(Tooltip = "Narrows the search area to a rectangle specified by coordinates in the `x0⫽y0⫽x1⫽y1` format, where `x0⫽y0` and `x1⫽y1` are the pixel coordinates in the top left corner and the bottom right corner of the rectangle, respectively")]
            public RectangleStructure ScreenSearchArea { get; set; } = new RectangleStructure(SystemInformation.VirtualScreen);

            [Argument(Tooltip = "Specifies whether the search should be done relatively to the active window")]
            public BooleanStructure Relative { get; set; } = new BooleanStructure(true);

            [Argument(Tooltip = "Tolerance threshold (0-1 range); the default 0 means it has to be a 100% match")]
            public FloatStructure Threshold { get; set; } = new FloatStructure(0);

            [Argument(Tooltip = "If specified, the resulting point will be placed in the center of the matching area")]
            public BooleanStructure CenterResult { get; set; } = new BooleanStructure(true);

            [Argument(Tooltip = "Value that will be added to the result's X coordinate")]
            public IntegerStructure OffsetX { get; set; } = new IntegerStructure(0);

            [Argument(Tooltip = "Value that will be added to the result's Y coordinate")]
            public IntegerStructure OffsetY { get; set; } = new IntegerStructure(0);

            [Argument(Tooltip = "Name of a variable where the command's result will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

            [Argument(Required = true, DefaultVariable = "timeoutimagefind", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public override TimeSpanStructure Timeout { get; set; }
        }

        public ImageFindCommand(AbstractScripter scripter) : base(scripter) { }

        public void Execute(Arguments arguments)
        {
            if (arguments.Threshold.Value < 0 || arguments.Threshold.Value > 1)
            {
                throw new ArgumentOutOfRangeException("Threshold must be a value from 0 to 1.");
            }

            using (var template = arguments.Image1.OpenImage())
            using (var source = !string.IsNullOrEmpty(arguments.Image2?.Value) ? arguments.Image2.OpenImage() : arguments.ScreenSearchArea.GetScreenshot(arguments.Relative))
            {
                var foundRectangle = source.MatchTemplate(template, arguments.Threshold);

                if (foundRectangle == Rectangle.Empty)
                {
                    throw new ArgumentException("Image was not found in specified search area.");
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
