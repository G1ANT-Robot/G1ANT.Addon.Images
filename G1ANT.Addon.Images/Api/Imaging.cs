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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace G1ANT.Language.Images
{
    class Imaging
    {
        public static bool AreImagesEqual(Bitmap image1, Bitmap image2)
        {
            if (image1.Height != image2.Height || image1.Width != image2.Width)
            {
                return false;
            }
            try
            {
                for (int i = 0; i < image2.Width; i++)
                {
                    for (int j = 0; j < image2.Height; j++)
                    {
                        if (image1.GetPixel(i, j) != image2.GetPixel(i, j))
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static Rectangle IsImageInImage(Bitmap smallBmp, Bitmap bigBmp, double tolerance)
        {
            BitmapData smallData =
              smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            BitmapData bigData =
              bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int smallStride = smallData.Stride;
            int bigStride = bigData.Stride;

            int bigWidth = bigBmp.Width;
            int bigHeight = bigBmp.Height - smallBmp.Height + 1;
            int smallWidth = smallBmp.Width * 3;
            int smallHeight = smallBmp.Height;

            Rectangle location = Rectangle.Empty;
            int margin = Convert.ToInt32(255.0 * tolerance);

            unsafe
            {
                byte* pSmall = (byte*)(void*)smallData.Scan0;
                byte* pBig = (byte*)(void*)bigData.Scan0;

                int smallOffset = smallStride - smallBmp.Width * 3;
                int bigOffset = bigStride - bigBmp.Width * 3;

                bool matchFound = true;

                for (int y = 0; y < bigHeight; y++)
                {
                    for (int x = 0; x < bigWidth; x++)
                    {
                        byte* pBigBackup = pBig;
                        byte* pSmallBackup = pSmall;

                        for (int i = 0; i < smallHeight; i++)
                        {
                            int j = 0;
                            matchFound = true;
                            for (j = 0; j < smallWidth; j++)
                            {

                                int inf = pBig[0] - margin;
                                int sup = pBig[0] + margin;
                                if (sup < pSmall[0] || inf > pSmall[0])
                                {
                                    matchFound = false;
                                    break;
                                }

                                pBig++;
                                pSmall++;
                            }

                            if (!matchFound) break;

                            pSmall = pSmallBackup;
                            pBig = pBigBackup;

                            pSmall += smallStride * (1 + i);
                            pBig += bigStride * (1 + i);
                        }

                        if (matchFound)
                        {
                            location.X = x;
                            location.Y = y;
                            location.Width = smallBmp.Width;
                            location.Height = smallBmp.Height;
                            break;
                        }
                        else
                        {
                            pBig = pBigBackup;
                            pSmall = pSmallBackup;
                            pBig += 3;
                        }
                    }

                    if (matchFound) break;

                    pBig += bigOffset;
                }
            }

            bigBmp.UnlockBits(bigData);
            smallBmp.UnlockBits(smallData);

            return location;
        }

        public static Bitmap ApplyMatrixTransform(Bitmap b, int factor, int size, int[,] matrix)
        {
            Bitmap b2 = b;
            if (0 == factor)
                return null;

            Bitmap bDest = (Bitmap)b.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                                ImageLockMode.ReadWrite,
                                PixelFormat.Format24bppRgb);
            BitmapData bmDest = bDest.LockBits(new Rectangle(0, 0, bDest.Width, bDest.Height),
                               ImageLockMode.ReadWrite,
                               PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;

            System.IntPtr Scan0 = bmDest.Scan0;
            System.IntPtr SrcScan0 = bmData.Scan0;

            bool[,] tablica = new bool[b2.Height + size - 1, b2.Width + size - 1];
            for (int i = 0; i < b2.Height + size - 1; i++)
            {
                for (int j = 0; j < b2.Width + size - 1; j++)
                {
                    if (i < size / 2) { tablica[i, j] = true; continue; }
                    if (j < size / 2) { tablica[i, j] = true; continue; }

                    if (j >= b2.Width + size / 2) { tablica[i, j] = true; continue; }
                    if (i >= b2.Height + size / 2) { tablica[i, j] = true; continue; }
                    tablica[i, j] = false;
                }
            }

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;
                byte* pSrc2 = (byte*)(void*)SrcScan0;
                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - (size - 1);
                int nHeight = b.Height - (size - 1);
                int nPixel;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = 0;
                        int counter_true = 0;
                        for (int i = 0, i2 = size - 1, i3 = 0; i < size; i++, i3++)
                        {
                            for (int j = 0, j2 = size - 1, j3 = 0; j < 3 * size - 2; j += 3, j3++)
                            {
                                if (tablica[i3 + y, j3 + x] == false)
                                {
                                    nPixel += pSrc[j + i * stride] * matrix[i2, j2];
                                }
                                else
                                {
                                    counter_true++;
                                }
                                j2--;
                            }
                            i2--;
                        }
                        int divider = factor - counter_true;
                        if (divider <= 0) divider = 1;
                        nPixel /= divider;

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[3 * (size / 2) + stride * (size / 2)] = (byte)nPixel;

                        nPixel = 0;
                        counter_true = 0;

                        for (int i = 0, i2 = size - 1, i3 = 0; i < size; i++, i3++)
                        {
                            for (int j = 1, j2 = size - 1, j3 = 0; j < 3 * size - 1; j += 3, j3++)
                            {
                                if (tablica[i3 + y, j3 + x] == false)
                                {
                                    nPixel += pSrc[j + i * stride] * matrix[i2, j2];
                                }
                                else
                                {
                                    counter_true++;
                                }
                                j2--;
                            }
                            i2--;
                        }


                        int divider3 = factor - counter_true;
                        if (divider3 <= 0) divider3 = 1;
                        nPixel /= divider3;

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[3 * (size / 2) + 1 + stride * (size / 2)] = (byte)nPixel;

                        nPixel = 0;
                        counter_true = 0;

                        for (int i = 0, i2 = size - 1, i3 = 0; i < size; i++, i3++)
                        {
                            for (int j = 2, j2 = size - 1, j3 = 0; j < 3 * size; j += 3, j3++)
                            {
                                if (tablica[i3 + y, j3 + x] == false)
                                {
                                    nPixel += pSrc[j + i * stride] * matrix[i2, j2];
                                }
                                else
                                {
                                    counter_true++;
                                }
                                j2--;
                            }
                            i2--;
                        }
                        int divider2 = factor - counter_true;
                        if (divider2 <= 0) divider2 = 1;
                        nPixel /= divider2;

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[3 * (size / 2) + 2 + stride * (size / 2)] = (byte)nPixel;

                        p += 3;
                        pSrc += 3;
                    }
                    p2 += stride;
                    p = p2;
                    pSrc2 += stride;
                    pSrc = pSrc2;

                }
            }

            b.UnlockBits(bmData);
            bDest.UnlockBits(bmDest);
            return bDest;
        }

        public static void SharpenImage(Bitmap b)
        {
            AForgeWrapper.ApplyMatrix(b, new int[,] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } }, 1);
        }

        public static Image MergeBitmaps(List<Bitmap> bitmaps)
        {
            var width = bitmaps.Max(x => x.Width);
            var height = bitmaps.Sum(x => x.Height);
            Image img = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(img))
            {
                int posX = 0;
                int posY = 0;
                foreach (var bitmap in bitmaps)
                {
                    g.DrawImage(bitmap, new Rectangle(new Point(posX, posY), bitmap.Size));
                    posY += bitmap.Height;
                }
            }
            return img;
        }

        public static Rectangle ParseRectanglePositionFromArguments(Rectangle screenSearchArea, bool relative)
        {
            Rectangle result = screenSearchArea;
            if (result.X < 0 ||
                result.Y < 0 ||
                result.Width < 1 ||
                result.Height < 1)
                throw new ArgumentException("ScreenSearchArea argument's parts can't be negative. Both width and height must be bigger than zero.");

            if (relative)
            {
                IntPtr handle = RobotWin32.GetForegroundWindow();
                if (handle.ToInt32() == 0)
                    throw new ApplicationException("Cannot find foreground window.");
                RobotWin32.Rect foregroundWindowRect = new RobotWin32.Rect();
                if (RobotWin32.GetWindowRectangle(handle, ref foregroundWindowRect) == false)
                    throw new ApplicationException("Cannot get foreground window rect.");

                result = new Rectangle(result.X + foregroundWindowRect.Left,
                    result.Y + foregroundWindowRect.Top,
                    result.Width,
                    result.Height);
            }
            return result;
        }

        public static Bitmap OpenImageFile(string path, string argumentName)
        {
            Bitmap clone = null;
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        fileStream.CopyTo(ms);
                        clone = new Bitmap(new Bitmap(ms));
                        ms.Close();
                    }
                    fileStream.Close();
                }
                return clone;
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"Could not open image file '{path}' specified in {argumentName} argument. Message: {ex.Message}", ex);
            }
        }

        public static void CheckIfFileIsACorrectImage(string path, string argumentName)
        {
            try
            {
                Bitmap result = new Bitmap(path);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Could not open image file '{path}' specified in {argumentName} argument. Message: {ex.Message}");
            }
        }

        public static void SaveImageFile(Bitmap image, string path, string argumentName)
        {
            try
            {
                image.Save(path);
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        image.Save(memory, image.RawFormat, image.GetEncoderParameterList())
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not save image file '{path}' specified in {argumentName} argument. Message: {ex.Message}");
            }
        }
    }
}
