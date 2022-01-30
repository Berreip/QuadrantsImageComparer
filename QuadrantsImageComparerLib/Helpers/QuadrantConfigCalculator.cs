using System;
using System.Drawing;
using QuadrantsImageComparerLib.Core;
using QuadrantsImageComparerLib.Models;

namespace QuadrantsImageComparerLib.Helpers
{
    public static class QuadrantConfigCalculator
    {
        public static Rectangle ComputeAoi(this ImageAoi imageAoi, Size imgSize)
        {
            var percentageBottom = MathExt.Clamp(imageAoi.AoiBottomPercentage, 0, 100);
            var percentageTop = MathExt.Clamp(imageAoi.AoiTopPercentage, 0, 100 - percentageBottom); // cap top percentage with bottom
            var percentageRight = MathExt.Clamp(imageAoi.AoiRightPercentage, 0, 100);
            var percentageLeft = MathExt.Clamp(imageAoi.AoiLeftPercentage, 0, 100 - percentageRight); // cap top percentage with right
            
            var topPosition = (int)Math.Round((imgSize.Height * percentageTop)/100d);
            var rectangleHeight =  imgSize.Height - topPosition - (int)Math.Round((imgSize.Height * percentageBottom)/100d);
            var leftPosition = (int)Math.Round((imgSize.Width * percentageLeft)/100d);
            var rectangleWidth =  imgSize.Width - leftPosition - (int)Math.Round((imgSize.Width * percentageRight)/100d);
            return new Rectangle(leftPosition, topPosition, rectangleWidth, rectangleHeight);
        }
        
        public static Size ComputeSizeFromQuadrant(int quadrantRows, int quadrantColumns)
        {
            return new Size(
                MathExt.Clamp(quadrantColumns, 1, QuadrantLibConstants.MAXIMUM_ALLOWED_QUADRANTS),  // cap each between 1 and the maximum allowed number of quadrant
                MathExt.Clamp(quadrantRows, 1, QuadrantLibConstants.MAXIMUM_ALLOWED_QUADRANTS)); // cap each between 1 and the maximum allowed number of quadrant
        }
    }
}