using Newtonsoft.Json;

namespace QuadrantsImageComparerLib.Dto
{
    /// <summary>
    /// Represent a DTO object for matrix set
    /// </summary>
    [JsonObject("QuadrantDiffDto")]
    public sealed class QuadrantDiffDto
    {
        [JsonProperty("Threshold")]
        public int Threshold { get; set; }

        [JsonProperty("Red")]
        public int[,] Red { get; set; }
        
        [JsonProperty("Green")]
        public int[,] Green { get; set; }
        
        [JsonProperty("Blue")]
        public int[,] Blue { get; set; }

        [JsonProperty("AoiInfo")]
        public AoiInfoDto AoiInfo { get; set; }
    }

    [JsonObject("AoiInfo")]
    public sealed class AoiInfoDto
    {
        [JsonProperty("AoiBottomPercentage")]
        public int AoiBottomPercentage { get; set; }
        
        [JsonProperty("AoiLeftPercentage")]
        public int AoiLeftPercentage { get; set; }
        
        [JsonProperty("AoiTopPercentage")]
        public int AoiTopPercentage { get; set; }
        
        [JsonProperty("AoiRightPercentage")]
        public int AoiRightPercentage { get; set; }

        [JsonProperty("QuadrantRows")]
        public int QuadrantRows { get; set; }
        
        [JsonProperty("QuadrantColumns")]
        public int QuadrantColumns { get; set; }
    }
}