using System;
using System.Drawing;

using G1ANT.Language;

namespace G1ANT.Language.Images
{
    [Command(Name = "image.sharpen", Tooltip = "This command sharpens image")]
    public class ImageSharpenCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Path of picture to get sharpened.")]
            public TextStructure Path { get; set; }

            [Argument(Tooltip = "Saving path. If not specified, input path will be used.")]
            public TextStructure OutputPath { get; set; }

             
        }
        public ImageSharpenCommand(AbstractScripter scripter) : base(scripter)
        { }
        public void Execute(Arguments arguments)
        {
            string savingPath = arguments.OutputPath?.Value ?? arguments.Path.Value;
            string savingPathArgumentName = arguments.OutputPath != null ? nameof(arguments.OutputPath) : nameof(arguments.Path);
            Bitmap image = null;
            using (image = Imaging.OpenImageFile(arguments.Path.Value, nameof(arguments.Path)))
            {
                SharpenImage(image, arguments.Path.Value, nameof(arguments.Path));
                Imaging.SaveImageFile(image, savingPath, savingPathArgumentName);
            }
        }

        private void SharpenImage(Bitmap image, string path, string pathArgumentName)
        {
            try
            {
                Imaging.SharpenImage(image);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not sharpen image file '{path}' specified in {pathArgumentName} argument. Message: {ex.Message}", ex);
            }
        }
    }
}
