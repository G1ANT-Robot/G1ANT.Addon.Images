/**
*    Copyright(C) G1ANT Robot Ltd, All rights reserved
*    Solution G1ANT.Addon.Images, Project G1ANT.Addon.Images
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using G1ANT.Addon.Images;
using G1ANT.Addon.Images.Factories;

namespace G1ANT.Language.Images
{
    [Command(Name = "image.applyfilter", Tooltip = "This command applies smoothing filter to a specified image")]
    public class ImageFilterCommand : Command
    {
        private readonly SmoothingFilterFactory smoothingFilterFactory = new SmoothingFilterFactory();
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Filter name. Possible values: Mean, Median, ConservativeSmoothing, BilateralSmoothing, AdaptiveSmoothing")]
            public TextStructure Filter { get; set; }

            [Argument(Required = true, Tooltip = "Path to an image")]
            public TextStructure Path { get; set; }

            [Argument(Tooltip = "Path to a resulting file; if not specified, input path will be used and the original file will be replaced")]
            public TextStructure OutputPath { get; set; }
        }

        public ImageFilterCommand(AbstractScripter scripter) : base(scripter)
        { }

        public void Execute(Arguments arguments)
        {
            var savingPath = arguments.OutputPath?.Value ?? arguments.Path.Value;
            var filter = smoothingFilterFactory.GetSmoothingFilter(arguments.Filter.Value);

            using (var image = arguments.Path.OpenImage())
            {
                filter.ApplyInPlace(image);
                image.Save(savingPath);
            }
        }
    }
}
