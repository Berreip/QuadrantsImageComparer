using System.Drawing;
using QuadrantsImageComparerLib.Models;

namespace QuadrantsImageComparerLib
{
    /// <summary>
    /// Allow to process image comparison using quadrant configuration
    /// </summary>
    public static class QuadrantComparer
    {
        public static IQuadrantDelta ComputeDelta(Bitmap image1, Bitmap image2, QuadrantConfig quadrantConfig)
        {
            
            var red = new int[quadrantConfig.NumberOfQuadrantRows, quadrantConfig.NumberOfQuadrantColumns];
            var green = new int[quadrantConfig.NumberOfQuadrantRows, quadrantConfig.NumberOfQuadrantColumns];
            var blue = new int[quadrantConfig.NumberOfQuadrantRows, quadrantConfig.NumberOfQuadrantColumns];
            return new QuadrantDelta(new Array2D(red), new Array2D(green), new Array2D(blue));
        }
    }

}