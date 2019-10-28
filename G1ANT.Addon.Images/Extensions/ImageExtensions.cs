using AForge.Imaging;
using G1ANT.Language;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Image = AForge.Imaging.Image;

namespace G1ANT.Addon.Images
{
    public static class ImageExtensions
    {
        public static Bitmap OpenFile(this TextStructure filePath)
        {
            try
            {
                using (var bitmap = new Bitmap(filePath.Value))
                {
                    return Image.Clone(bitmap, PixelFormat.Format24bppRgb);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not open the image file '{filePath.Value}'. Message: {ex.Message}", ex);
            }
        }

        public static Bitmap OpenFileOrGetScreenshot(this TextStructure filePath, RectangleStructure screenArea, BooleanStructure relative)
        {
            if (!string.IsNullOrEmpty(filePath?.Value))
                return filePath.OpenFile();

            try
            {
                var screenSearchArea = Imaging.ParseRectanglePositionFromArguments(screenArea.Value, relative.Value);
                return RobotWin32.GetPartOfScreen(screenSearchArea, PixelFormat.Format24bppRgb);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get the screenshot image. Message: {ex.Message}", ex);
            }
        }

        public static Rectangle MatchTemplate(this Bitmap source, Bitmap template, FloatStructure threshold)
        {
            try
            {
                var similarityThreshold = (float)(1.0 - threshold.Value);
                var templateMatching = new ExhaustiveTemplateMatching(similarityThreshold);
                var matchings = templateMatching.ProcessImage(source, template);                
                return matchings.Select(m => m.Rectangle).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not match the template. Message: {ex.Message}", ex);
            }
        }
    }
}
