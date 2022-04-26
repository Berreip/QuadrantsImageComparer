using QuadrantsImageComparerLib.Dto;
using QuadrantsImageComparerLib.Models;

namespace QuadrantsImageComparerLib.Helpers
{
    public static class QuadrantDiffDtoExtensions
    {
        /// <summary>
        /// Compare the first quadrant diff to the second and returns the resulting diff
        /// </summary>
        public static DifferenceDiff IsValidAgainstWithDifference(this QuadrantDiffDto refQuadrantDiffDto, QuadrantDiffDto diffToCompare)
        {
            return new DifferenceDiff(refQuadrantDiffDto, diffToCompare);
        }
    }

    /// <summary>
    /// The difference between two quadrant diff dto
    /// </summary>
    public sealed class DifferenceDiff
    {
        /// <summary>
        /// True if the quandrant count does not match (and so no validity comparison could be done)
        /// </summary>
        public bool IsNotTheSameQuadrantCount { get; }
        
        /// <summary>
        /// True if the diff is valid with the current Threshold
        /// </summary>
        public bool IsValidWithCurrentThreshold { get; }

        /// <summary>
        /// The current threshold
        /// </summary>
        public int CurrentThreshold { get; }
        
        public Array2D Red { get; }
        public Array2D Green { get; }
        public Array2D Blue { get; }

        public DifferenceDiff(QuadrantDiffDto refQuadrantDiffDto, QuadrantDiffDto diffToCompare)
        {
            CurrentThreshold = refQuadrantDiffDto.Threshold;
            
            var redRef = new Array2D(refQuadrantDiffDto.Red);
            var greenRef = new Array2D(refQuadrantDiffDto.Green);
            var blueRef = new Array2D(refQuadrantDiffDto.Blue);

            if (refQuadrantDiffDto.Red.Length != diffToCompare.Red.Length)
            {
                IsNotTheSameQuadrantCount = true;
                Red = redRef;
                Green = greenRef;
                Blue = blueRef;
                return;
            }
            
            // compute the difference
            var isValidRed = redRef.EqualsArrayAndGetDifference(diffToCompare.Red, refQuadrantDiffDto.Threshold, out var redDiff);
            var isValidGreen = greenRef.EqualsArrayAndGetDifference(diffToCompare.Green, refQuadrantDiffDto.Threshold, out var greenDiff);
            var isValidBlue = blueRef.EqualsArrayAndGetDifference(diffToCompare.Blue, refQuadrantDiffDto.Threshold, out var blueDiff);
            IsValidWithCurrentThreshold = isValidRed && isValidGreen && isValidBlue;
            Red = new Array2D(redDiff);
            Green = new Array2D(greenDiff);
            Blue = new Array2D(blueDiff);
        }
    }
}