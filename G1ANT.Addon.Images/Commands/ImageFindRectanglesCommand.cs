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
using System.Drawing;
using System.IO;

namespace G1ANT.Language.Images
{
    [Command(Name = "image.findrectangles", Tooltip = "This command finds objects separated by a black background in a specified image and returns a list of their coordinates, width and height")]
    public class ImageFindRectanglesCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Path to an image file with objects to be counted")]
            public TextStructure Path { get; set; }

            [Argument(Tooltip = "Name of a variable where the command's result will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

            [Argument(Tooltip = "By default, this argument inverts a specified image (makes it a negative), so standard, white-background images can be processed. For black-background images, set this argument to `false`")]
            public BooleanStructure Invert { get; set; } = new BooleanStructure(true);

            [Argument(Tooltip = "Minimal width of an image area to be processed")]
            public IntegerStructure MinWidth { get; set; }

            [Argument(Tooltip = "Maximal width of an image area to be processed")]
            public IntegerStructure MaxWidth { get; set; }

            [Argument(Tooltip = "Minimal height of an image area to be processed")]
            public IntegerStructure MinHeight { get; set; }

            [Argument(Tooltip = "Maximal height of an image area to be processed")]
            public IntegerStructure MaxHeight { get; set; }

             
        }
        public ImageFindRectanglesCommand(AbstractScripter scripter) : base(scripter)
        { }
        public void Execute(Arguments arguments)
        {
            try
            {
                List<System.Drawing.Rectangle> foundRectangles = new List<System.Drawing.Rectangle>();
                using (Bitmap image = (Bitmap)Image.FromFile(arguments.Path.Value))
                {
                    foundRectangles = AForgeWrapper.FindRectangles(
                        image,
                        arguments.Invert.Value,
                        arguments.MinWidth?.Value,
                        arguments.MaxWidth?.Value,
                        arguments.MinHeight?.Value,
                        arguments.MaxHeight?.Value);
                }                   

                List<Structure> results = new List<Structure>();
                foreach (var foundRectangle in foundRectangles)
                {
                    results.Add(new RectangleStructure(foundRectangle));
                }
                Scripter.Variables.SetVariableValue(arguments.Result.Value, new ListStructure(results));
            }
            catch (Exception ex)
            {               
                throw new ApplicationException($"Specified directory doesn't exist. Message: {ex.Message}", ex);
            }
        }
    }
}


