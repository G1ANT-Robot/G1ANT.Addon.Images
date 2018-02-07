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
    [Command(Name = "image.findrectangles")]
    public class ImageFindRectanglesCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true)]
            public TextStructure Path { get; set; }

            [Argument]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

            [Argument]
            public BooleanStructure Invert { get; set; } = new BooleanStructure(true);

            [Argument]
            public IntegerStructure MinWidth { get; set; }

            [Argument]
            public IntegerStructure MaxWidth { get; set; }

            [Argument]
            public IntegerStructure MinHeight { get; set; }

            [Argument]
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


