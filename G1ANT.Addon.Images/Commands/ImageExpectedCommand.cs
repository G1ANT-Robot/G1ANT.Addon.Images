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

using G1ANT.Language.Images;

namespace G1ANT.Language.Images
{
    [Command(Name = "image.expected", Tooltip = "This command checks if `image1` is exactly the same as `image2` (or is displayed somewhere on the screen) and returns a true/false result.")]
    public class ImageExpectedCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Path to a file with an image be found")]
            public TextStructure Image1 { get; set; }

            [Argument(Tooltip = "Path to a image file in which `image1` will be searched. If not specified, `image1` will be searched on the screen")]
            public TextStructure Image2 { get; set; }

            [Argument(Tooltip = "Narrows the search area to a rectangle specified by coordinates in the `x0⫽y0⫽x1⫽y1` format, where `x0⫽y0` and `x1⫽y1` are the pixel coordinates in the top left corner and the bottom right corner of the rectangle, respectively")]
            public RectangleStructure ScreenSearchArea { get; set; } = new RectangleStructure(SystemInformation.VirtualScreen);

            [Argument(Tooltip = "Specifies whether the search should be done relatively to the active window")]
            public BooleanStructure Relative { get; set; } = new BooleanStructure(true);

            [Argument(Tooltip = "Tolerance threshold (0-1 range); the default 0 means it has to be a 100% match")]
            public FloatStructure Threshold { get; set; } = new FloatStructure(0);

            [Argument]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

            [Argument(Required = true, DefaultVariable = "timeoutimageexpected", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public override TimeSpanStructure Timeout { get; set; }


        }
        public ImageExpectedCommand(AbstractScripter scripter) : base(scripter)
        { }
        public void Execute(Arguments arguments)
        {
            if (arguments.Threshold.Value < 0 || arguments.Threshold.Value > 1)
            {
                throw new ArgumentOutOfRangeException("Threshold must be a value from 0 to 1.");
            }

            using (Bitmap bitmap1 = Imaging.OpenImageFile(arguments.Image1.Value, nameof(arguments.Image1)))
            using (Bitmap bitmap2 = (string.IsNullOrEmpty(arguments.Image2?.Value)) ?
                                RobotWin32.GetPartOfScreen(
                                    Imaging.ParseRectanglePositionFromArguments(arguments.ScreenSearchArea.Value, arguments.Relative.Value),
                                    bitmap1.PixelFormat) :
                                Imaging.OpenImageFile(arguments.Image2.Value, nameof(arguments.Image2)))
            {


                bool found = Rectangle.Empty != Imaging.IsImageInImage(bitmap1, bitmap2, (double)arguments.Threshold.Value);
                Scripter.Variables.SetVariableValue(arguments.Result.Value, new BooleanStructure(found));
            }

        }
    }
}
