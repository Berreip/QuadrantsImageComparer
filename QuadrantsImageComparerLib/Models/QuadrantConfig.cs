namespace QuadrantsImageComparerLib.Models
{
    public interface IQuadrantConfig
    {
    }

    public sealed class QuadrantConfig : IQuadrantConfig
    {
        public int AoiBottomPercentage { get; set; }
        public int AoiLeftPercentage { get; set; }
        public int AoiTopPercentage { get; set; }
        public int AoiRightPercentage { get; set; }
        public int NumberOfQuadrantRows { get; set; }
        public int NumberOfQuadrantColumns { get; set; }
    }
}