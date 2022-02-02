using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using QicRecVisualizer.WpfCore.Images;
using QuadrantsImageComparerLib.Models;

namespace QicRecVisualizer.Services.Helpers
{
    public static class BitmapHeatMap
    {
        public static BitmapImage RefreshBitmap(Array2D initialMatrix, int thresholdValueInt)
        {
            using (var bmp = new Bitmap(initialMatrix.Columns, initialMatrix.Rows))
            {
                // colorize:
                unsafe
                {
                    var bitmapData = bmp.LockBits(
                        new Rectangle(0, 0, bmp.Width, bmp.Height),
                        ImageLockMode.ReadWrite,
                        bmp.PixelFormat);
                    try
                    {
                        var bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                        var heightInPixels = bitmapData.Height;
                        var widthInBytes = bitmapData.Width * bytesPerPixel;

                        var currentLine = (byte*)bitmapData.Scan0;
                        // set every pixel value
                        var arrayHeightRow = 0;
                        for (var y = 0; y < heightInPixels; y++, currentLine += bitmapData.Stride)
                        {
                            var arrayWidthColum = 0;
                            for (var x = 0; x < widthInBytes; x = x + bytesPerPixel)
                            {
                                var color = MatrixColorProvider.GetColor(Math.Abs(initialMatrix.GetValue(arrayHeightRow, arrayWidthColum)) - thresholdValueInt);
                                currentLine[x] = color.B; //blue
                                currentLine[x + 1] = color.G; //green
                                currentLine[x + 2] = color.R; //red
                                currentLine[x + 3] = color.A; //transparency
                                arrayWidthColum++;
                            }

                            arrayHeightRow++;
                        }
                    }
                    finally
                    {
                        bmp.UnlockBits(bitmapData);
                    }
                }

                return bmp.GetBitmapImage();
            }
        }
    }
}