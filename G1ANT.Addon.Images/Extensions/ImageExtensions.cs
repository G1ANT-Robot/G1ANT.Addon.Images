using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using G1ANT.Language;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace G1ANT.Addon.Images
{
    public static class ImageExtensions
    {
        public static Bitmap OpenImage(this TextStructure filePath)
        {
            try
            {
                using (var image = new Image<Bgr, byte>(filePath.Value))
                {
                    return image.Bitmap;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not open the image file '{filePath.Value}'. Message: {ex.Message}", ex);
            }
        }

        public static Bitmap GetScreenshot(this RectangleStructure screenArea, BooleanStructure relative)
        {
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
                var similarityThreshold = 1.0 - threshold.Value;
                
                using (var templateImage = new Image<Bgr, byte>(template))
                using (var sourceImage = new Image<Bgr, byte>(source))
                using (var match = sourceImage.MatchTemplate(templateImage, TemplateMatchingType.CcoeffNormed))
                {
                    match.MinMax(out var minValues, out var maxValues, out var minLocations, out var maxLocations);
                    return maxValues[0] >= similarityThreshold ? new Rectangle(maxLocations[0], templateImage.Size) : Rectangle.Empty;
                }
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
                using (var sourceImage = new Image<Bgr, byte>(source))
                using (var gaussBlur = new Mat())
                {
                    CvInvoke.GaussianBlur(sourceImage.Mat, gaussBlur, new Size(0, 0), 10);
                    CvInvoke.AddWeighted(sourceImage.Mat, 1.5, gaussBlur, -0.5, 0, sourceImage.Mat);
                    return sourceImage.Bitmap;
                }
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
                using (var sourceImage = new Image<Gray, byte>(source))
                using (var blobs = new CvBlobs())
                using (var blobDetector = new CvBlobDetector())
                {
                    if (invert.Value)
                        CvInvoke.BitwiseNot(sourceImage, sourceImage);

                    blobDetector.Detect(sourceImage, blobs);

                    return blobs.Select(b => b.Value.BoundingBox).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not find the rectangle. Message: {ex.Message}", ex);
            }
        }    
    }
}
