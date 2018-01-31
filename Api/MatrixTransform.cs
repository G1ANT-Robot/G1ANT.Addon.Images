using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1ANT.Language.Images
{
    class MatrixTransform
    {
        public Bitmap ApplyMatrixTransform(Bitmap b, int factor, int size, int[,] matrix)
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
    }
}
