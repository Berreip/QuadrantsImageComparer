namespace QuadrantsImageComparerLib.Models
{
    public interface IQuadrantConfig
    {
    }

    public sealed class QuadrantConfig : IQuadrantConfig
    {
        public QuadrantConfig(int numberOfQuadrantRows, int numberOfQuadrantColumns)
        {
            NumberOfQuadrantRows = numberOfQuadrantRows;
            NumberOfQuadrantColumns = numberOfQuadrantColumns;
        }

        public int NumberOfQuadrantRows { get; }
        public int NumberOfQuadrantColumns { get; }
        public ImageAoi Aoi { get; set; }
    }
}