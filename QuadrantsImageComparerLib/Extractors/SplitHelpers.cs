using System.Drawing;
using QuadrantsImageComparerLib.Helpers;

namespace QuadrantsImageComparerLib.Extractors
{
    /// <summary>
    /// Classe d'aide pour les découpage et redimensionnement d'images
    /// </summary>
    public static class SplitHelpers
    {
        /// <summary>
        /// Crop et resize une image EN EN GENERANT SYSTEMATIQUEMENT une nouvelle (qui doit donc être disposée)
        /// </summary>
        /// <returns></returns>
        public static Bitmap CropAndResizeImage(this IImageQuadrantInfo imageQuadrantInfo)
        {
            return imageQuadrantInfo.ShouldCrop
                ? CropAndResizeImage(imageQuadrantInfo.SourceImage, imageQuadrantInfo.Aoi, imageQuadrantInfo.ResizeFromQuadrant)
                : ResizeImage(imageQuadrantInfo.SourceImage, imageQuadrantInfo.ResizeFromQuadrant);
        }

        /// <summary>
        /// Crop and resize a image in once
        /// </summary>
        public static Bitmap CropAndResizeImage(Bitmap original, Rectangle targetCropPart, Size targetSize)
        {
            var croppedImage = new Bitmap(targetSize.Width, targetSize.Height);
            using (var graphics = Graphics.FromImage(croppedImage))
            {
                graphics.DrawImage(original, new Rectangle(new Point(), targetSize), targetCropPart, GraphicsUnit.Pixel);
            }
            return croppedImage;
        }

        /// <summary>
        /// Resize IMAGE 
        /// </summary>
        /// <returns>a NEW resized image</returns>
        public static Bitmap ResizeImage(Bitmap original, Size targetSize)
        {
            var croppedImage = new Bitmap(targetSize.Width, targetSize.Height);
            using (var graphics = Graphics.FromImage(croppedImage))
            {
                graphics.DrawImage(original, new Rectangle(new Point(), targetSize));
            }
            return croppedImage;
        }
    }
}
