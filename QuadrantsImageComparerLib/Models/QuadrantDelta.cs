namespace QuadrantsImageComparerLib.Models
{
    /// <summary>
    /// The quadrant computation result for 2 images
    /// </summary>
    public interface IQuadrantDelta
    {
        Array2D Red { get; }
        Array2D Green { get; }
        Array2D Blue { get; }
    }

    public class QuadrantDelta : IQuadrantDelta
    {
        public QuadrantDelta(Array2D red, Array2D green, Array2D blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public Array2D Red { get; }
        public Array2D Green { get; }
        public Array2D Blue { get; }
    }
}