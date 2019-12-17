using AForge.Imaging;
using AForge.Imaging.Filters;
using G1ANT.Language;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Image = AForge.Imaging.Image;

namespace G1ANT.Addon.Images
{
    public static class ImageExtensions
    {
        public static Bitmap OpenImage(this TextStructure filePath)
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

        public static Bitmap GetScreenshot(this RectangleStructure screenArea, BooleanStructure relative)
        {
            if (screenArea.Value.Width < 1 || screenArea.Value.Height < 1)
                throw new ArgumentException("ScreenSearchArea argument's parts can't be negative. Both width and height must be bigger than zero.");

            try
            {
                var screenSearchArea = screenArea.Value;

                if (relative.Value)
                    screenSearchArea = screenArea.Value.GetRelativeArea();

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

        public static Bitmap Sharpen(this Bitmap source)
        {
            try
            {
                var sharpenFilter = new Sharpen()
                {
                    Kernel = new int[,] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } },
                    Threshold = 1
                };

                return sharpenFilter.Apply(source);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not sharpen the image. Message: {ex.Message}", ex);
            }
        }

        public static List<Rectangle> FindRectangles(this Bitmap source, BooleanStructure invert)
        {
            try
            {
                var blobCounter = new BlobCounter();

                if (invert.Value)
                {
                    new Invert().ApplyInPlace(source);
                }
                
                blobCounter.ProcessImage(source);

                return blobCounter.GetObjectsRectangles().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not find the rectangle. Message: {ex.Message}", ex);
            }
        }
    }
}
