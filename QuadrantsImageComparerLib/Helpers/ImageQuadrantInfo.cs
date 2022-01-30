using System.Drawing;

namespace QuadrantsImageComparerLib.Helpers
{
    /// <summary>
    /// Group information about a source image,
    /// the target output size (given by quadrants definitions)
    /// and the Area Of Interest (aoi)
    /// </summary>
    public interface IImageQuadrantInfo
    {
        Rectangle Aoi { get; }
        Bitmap SourceImage { get; }
        
        /// <summary>
        /// The number of qudrant define the given size
        /// </summary>
        Size ResizeFromQuadrant { get;  }

        bool ShouldCrop { get; }
    }

    public sealed class ImageQuadrantInfo : IImageQuadrantInfo
    {
        /// <inheritdoc />
        public Rectangle Aoi { get; }

        /// <inheritdoc />
        public Bitmap SourceImage { get; }

        /// <inheritdoc />
        public Size ResizeFromQuadrant { get; }
        
        public bool ShouldCrop { get;  }

        public ImageQuadrantInfo(Rectangle aoi, Bitmap sourceImage, Size resizeFromQuadrant)
        {
            Aoi = aoi;
            SourceImage = sourceImage;
            ResizeFromQuadrant = resizeFromQuadrant;
            ShouldCrop = BitmapHelpers.ShouldCrop(aoi, sourceImage.Width, sourceImage.Height);
        }

    }
}