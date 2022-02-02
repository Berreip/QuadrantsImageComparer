using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using QuadrantsImageComparerLib.Models;

// ReSharper disable ConvertToCompoundAssignment

namespace QuadrantsImageComparerLib.Helpers
{
    internal static class BitmapHelpers
    {
        public const double BITMAP_RATIO_EPSILON = 0.001;

        public static double GetRatio(this Bitmap bmp)
        {
            return bmp.Width / (double)bmp.Height;
        }

        /// <summary>
        /// returns true if we should crop
        /// </summary>
        public static bool ShouldCrop(Rectangle aoi, int imageWidth, int imageHeight)
        {
            if (aoi.Height > imageHeight || aoi.Width > imageWidth)
            {
                throw new ArgumentException("Aoi is bigger than the image size");
            }

            return aoi.Height != imageHeight ||
                   aoi.Width != imageWidth ||
                   aoi.X != 0 ||
                   aoi.Y != 0;
        }

        /// <summary>
        /// calcule la différence entre 2 images pixel par pixel
        /// </summary>
        public static (Array2D red, Array2D green, Array2D blue) ComputeDifference(Bitmap img1, Bitmap img2)
        {
            if (img1.Size != img2.Size)
            {
                throw new ArgumentException($"this method process only same size bitmaps img1: {img1.Size} vs img2: {img2.Size}");
            }

            if (img1.PixelFormat != img2.PixelFormat)
            {
                throw new ArgumentException($"this method process only same PixelFormat bitmaps img1: {img1.PixelFormat} vs img2: {img2.PixelFormat}");
            }

            unsafe
            {
                var bmpData1 = img1.LockBits(
                    new Rectangle(0, 0, img1.Width, img1.Height),
                    ImageLockMode.ReadWrite,
                    img1.PixelFormat);

                var bmpData2 = img2.LockBits(
                    new Rectangle(0, 0, img2.Width, img2.Height),
                    ImageLockMode.ReadWrite,
                    img2.PixelFormat);
                try
                {
                    if (bmpData1.Stride != bmpData2.Stride)
                    {
                        throw new ArgumentException(@"bmpData here should have the same stride");
                    }

                    var red = new int[img1.Height, img1.Width];
                    var green = new int[img1.Height, img1.Width];
                    var blue = new int[img1.Height, img1.Width];

                    var bytesPerPixel = Image.GetPixelFormatSize(img1.PixelFormat) / 8;
                    var heightInPixels = bmpData1.Height;
                    var widthInBytes = bmpData1.Width * bytesPerPixel;

                    var currentLineImg1 = (byte*)bmpData1.Scan0;
                    var currentLineImg2 = (byte*)bmpData2.Scan0;

                    var arrayHeight = 0;
                    for (var y = 0; y < heightInPixels; y++, currentLineImg1 += bmpData1.Stride, currentLineImg2 += bmpData2.Stride)
                    {
                        var arrayWidth = 0;
                        for (var x = 0; x < widthInBytes; x = x + bytesPerPixel)
                        {
                            blue[arrayHeight, arrayWidth] = currentLineImg1[x] - currentLineImg2[x]; //blue
                            green[arrayHeight, arrayWidth] = currentLineImg1[x + 1] - currentLineImg2[x + 1]; //green
                            red[arrayHeight, arrayWidth] = currentLineImg1[x + 2] - currentLineImg2[x + 2]; //red
                            arrayWidth++;
                        }

                        arrayHeight++;
                    }

                    return (new Array2D(red), new Array2D(green), new Array2D(blue));
                }
                catch (Exception e)
                {
                    Debug.Fail(e.ToString());
                    throw;
                }
                finally
                {
                    img1.UnlockBits(bmpData1);
                    img2.UnlockBits(bmpData2);
                }
            }
        }
    }
}