namespace QuadrantsImageComparerLib.Models
{
    /// <summary>
    /// Structure image (to avoid nullability and default to zero each percentage))
    /// </summary>
    public struct ImageAoi
    {
        public int AoiBottomPercentage { get; set; }
        public int AoiLeftPercentage { get; set; }
        public int AoiTopPercentage { get; set; }
        public int AoiRightPercentage { get; set; }
    }
}