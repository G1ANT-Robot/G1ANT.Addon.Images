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

namespace G1ANT.Addon.Images
{
    [Command(Name = "image.sharpen", Tooltip = "This command sharpens a specified image")]
    public class ImageSharpenCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Path to an image to be sharpened")]
            public TextStructure Path { get; set; }

            [Argument(Tooltip = "Path to a resulting file; if not specified, input path will be used and the original file will be replaced")]
            public TextStructure OutputPath { get; set; }
        }

        public ImageSharpenCommand(AbstractScripter scripter) : base(scripter) { }

        public void Execute(Arguments arguments)
        {
            using (var image = arguments.Path.OpenImage())
            {
                image
                    .Sharpen()
                    .Save(arguments.OutputPath?.Value ?? arguments.Path.Value);
            }
        }
    }
}
