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
using System.Drawing.Imaging;
using G1ANT.Addon.Images.Enums;
using G1ANT.Addon.Images.Factories;

namespace G1ANT.Language.Images
{
    [Command(Name = "image.smoothingfilter", Tooltip = "This command applies smoothing filter to a specified image")]
    public class ImageFilterCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Filter name")]
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
            var savingPathArgumentName = arguments.OutputPath != null ? nameof(arguments.OutputPath) : nameof(arguments.Path);

            SmoothingFilter filterName;
            var filter = Enum.TryParse(arguments.Filter.Value, out filterName) ? new SmoothingFilterFactory().GetSmoothingFilter(filterName) : throw new ArgumentException("Incorrect filter name");

            using (var image = Imaging.OpenImageFile(arguments.Path.Value, nameof(arguments.Path)))
            {
                var preprocessedImage = PreprocessImageBeforeFiltering(image);
                filter.ApplyInPlace(preprocessedImage);
                Imaging.SaveImageFile(preprocessedImage, savingPath, savingPathArgumentName);
            }
        }

        private Bitmap PreprocessImageBeforeFiltering(Bitmap image)
        {
            if (image.PixelFormat == PixelFormat.Format24bppRgb)
                return image;

            try
            {
                if (image.PixelFormat == PixelFormat.Format16bppGrayScale || Image.GetPixelFormatSize(image.PixelFormat) > 32)
                    throw new NotSupportedException("Unsupported image format");

                return AForge.Imaging.Image.Clone(image, PixelFormat.Format24bppRgb);
            }
            finally
            {
                image.Dispose();
            }
        }
    }
}
