using System;
using QuadrantsImageComparerLib.Core;

namespace QicRecVisualizer.Services.Helpers
{
    public static class RectangleCropCalculator
    {
        public static (int TopPosition, int RectangleHeight) GetNewRectangleHeightAndTopPosition(double percentageBottom, double percentageTop, int imageCurrentHeight)
        {
            percentageBottom = MathExt.Clamp(percentageBottom, 0, 100);
            percentageTop = MathExt.Clamp(percentageTop, 0, 100 - percentageBottom); // cap top percentage with bottom
            
            var topPosition = (int)Math.Round((imageCurrentHeight * percentageTop)/100);
            var rectangleHeight =  imageCurrentHeight - topPosition - (int)Math.Round((imageCurrentHeight * percentageBottom)/100);
            return (topPosition, rectangleHeight);
        }
        
        public static (int LeftPosition, int RectangleWidth) GetNewRectangleWidthAndLeftPosition(double percentageRight, double percentageLeft, int imageCurrentWidth)
        {
            percentageRight = MathExt.Clamp(percentageRight, 0, 100);
            percentageLeft = MathExt.Clamp(percentageLeft, 0, 100 - percentageRight); // cap top percentage with bottom
            
            var topPosition = (int)Math.Round((imageCurrentWidth * percentageLeft)/100);
            var rectangleHeight =  imageCurrentWidth - topPosition - (int)Math.Round((imageCurrentWidth * percentageRight)/100);
            return (topPosition, rectangleHeight);
        }
    }
}