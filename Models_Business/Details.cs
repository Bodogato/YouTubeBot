namespace YouTubeBot.Business.Models
{
    public class Details
    {
        public string Duration { get; set; }
        public string Dimension { get; set; }
        public string Definition { get; set; }
        public string Caption { get; set; }

        public Details(string duration, string dimension, string definition, string caption)
        {
            this.Duration = duration;
            this.Dimension = dimension;
            this.Definition = definition;
            this.Caption = caption;
        }
    }
}