namespace BlazorGolf.Core.Models
{
    public class Tee
    {
        public string Name { get; set; } = String.Empty;
        public int Par { get; set; } = 0;
        public int Slope { get; set; } = 0;
        public double Rating { get; set; } = 0.0;
        public double BogeyRating { get; set; } = 0.0;
        public double FrontNineRating { get; set; } = 0.0;
        public int FrontNineSlope { get; set; } = 0;
        public double BackNineRating { get; set; } = 0.0;
        public int BackNineSlope { get; set; } = 0;
    }
}
