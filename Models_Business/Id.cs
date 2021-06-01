namespace YouTubeBot.Business.Models
{
    public class Id
    {
        public string VideoId { get; set; }

        public Id(string videoId)
        {
            this.VideoId = videoId;
        }
    }

}