using System;
using System.Collections.Generic;
using System.Drawing;
using QuadrantsImageComparerLib.Dto;
using QuadrantsImageComparerLib.Extractors;
using QuadrantsImageComparerLib.Helpers;
using QuadrantsImageComparerLib.Models;

namespace QuadrantsImageComparerLib
{
    /// <summary>
    /// Allow to process image comparison using quadrant configuration
    /// </summary>
    public static class QuadrantComparer
    {
        /// <summary>
        /// Compute the delta between 2 images using a quadrant kind comparison:
        ///  the image 2 is computed by quadrant (split for instance in 10x12 quadrants) and then the mean of pixel values for each
        /// quadrant is subtracted to the mean of same quadrants of image 1.
        /// Depending on the number of quadrant, the comparison will be closer to a bitwise comparison.
        /// Note that if the ratio don't match, a warning will be issued BUT the comparison will be done
        /// </summary>
        public static IQuadrantDelta ComputeDelta(Bitmap image1, Bitmap image2, QuadrantConfig quadrantConfig)
        {
            var warnings = new List<WarningKind>(); 
            // check ratio first:
            if (Math.Abs(image1.GetRatio() - image2.GetRatio()) > BitmapHelpers.BITMAP_RATIO_EPSILON)
            {
                warnings.Add(WarningKind.RatioDoNotMatch);
            }
            
            var size = QuadrantConfigCalculator.ComputeSizeFromQuadrant(quadrantConfig.NumberOfQuadrantRows, quadrantConfig.NumberOfQuadrantColumns);
            var quadrantImg1 = ComputeQuadrantImg(image1, size, quadrantConfig);
            var quadrantImg2 = ComputeQuadrantImg(image2, size, quadrantConfig);

            var (red, green, blue) =  BitmapHelpers.ComputeDifference(quadrantImg1, quadrantImg2);
            return new QuadrantDelta(red, green, blue, warnings, quadrantImg1, quadrantImg2);
        }

        /// <summary>
        /// in practice, we use a little hack: we crop the image given the Aoi then we resize it to the number of quadrants row and columns:
        /// it allow us to use the approximation of image calculation instead of our own means (that could be done of course)
        /// </summary>
        private static Bitmap ComputeQuadrantImg(Bitmap img, Size targetSize, QuadrantConfig quadrantConfig)
        {
            var computedAoi = quadrantConfig.Aoi.ComputeAoi(img.Size);
            var quadrantInfo = new ImageQuadrantInfo(computedAoi, img, targetSize);
            return quadrantInfo.CropAndResizeImage();
        }
        
        /// <summary>
        /// Compute the delta between 2 images using a quadrant kind comparison:
        ///  the image 2 is computed by quadrant (split for instance in 10x12 quadrants) and then the mean of pixel values for each
        /// quadrant is subtracted to the mean of same quadrants of image 1.
        /// Depending on the number of quadrant, the comparison will be closer to a bitwise comparison.
        /// Note that if the ratio don't match, a warning will be issued BUT the comparison will be done
        /// </summary>
        public static IQuadrantDelta ComputeDelta(Bitmap image1, Bitmap image2, AoiInfoDto aoiInfo)
        {
            return ComputeDelta(image1, image2, new QuadrantConfig(aoiInfo.QuadrantRows, aoiInfo.QuadrantColumns)
            {
                Aoi = new ImageAoi
                {
                    AoiLeftPercentage = aoiInfo.AoiLeftPercentage,
                    AoiTopPercentage = aoiInfo.AoiTopPercentage,
                    AoiRightPercentage = aoiInfo.AoiRightPercentage,
                    AoiBottomPercentage = aoiInfo.AoiBottomPercentage,
                }
            });
        }
    }

}