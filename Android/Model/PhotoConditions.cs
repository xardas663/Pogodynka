namespace Weather.Model
{
    public class PhotoConditions
    {
        public string ImagePath { get; set; } = "nearly null";
        public int TempMax { get; set; }
        public int TempMin { get; set; }
        public int FeelsLikeMax { get; set; }
        public int FeelsLikeMin { get; set; }
        public int Rain { get; set; }
    }
}