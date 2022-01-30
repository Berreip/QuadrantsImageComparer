using System;
using System.Collections.Generic;
using System.Drawing;

namespace QuadrantsImageComparerLib.Models
{
    /// <summary>
    /// The quadrant computation result for 2 images:
    ///  the image 2 is computed by quadrant (split for instance in 10x12 quadrants) and then the mean of pixel values for each
    /// quadrant is subtracted to the mean of same quadrants of image 1.
    /// Depending on the number of quadrant, the comparison will be closer to a bitwise comparison.
    /// Note that if the ratio don't match, a warning will be issued BUT the comparison will be done
    /// </summary>
    public interface IQuadrantDelta : IDisposable
    {
        Array2D Red { get; }
        Array2D Green { get; }
        Array2D Blue { get; }
        
        /// <summary>
        /// Warning kind (like ratio that don't fit)
        /// </summary>
        IReadOnlyCollection<WarningKind> Warnings { get; }

        Bitmap QuadrantImg1 { get; }
        Bitmap QuadrantImg2 { get; }
    }

    public sealed class QuadrantDelta : IQuadrantDelta
    {
        public Array2D Red { get; }
        public Array2D Green { get; }
        public Array2D Blue { get; }
        public IReadOnlyCollection<WarningKind> Warnings { get; }
        public Bitmap QuadrantImg1 { get; }
        public Bitmap QuadrantImg2 { get; }

        public QuadrantDelta(Array2D red, Array2D green, Array2D blue, IReadOnlyCollection<WarningKind> warnings, Bitmap quadrantImg1, Bitmap quadrantImg2)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Warnings = warnings;
            QuadrantImg1 = quadrantImg1;
            QuadrantImg2 = quadrantImg2;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            QuadrantImg1?.Dispose();
            QuadrantImg2?.Dispose();
        }
    }
}