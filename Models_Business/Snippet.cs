namespace YouTubeBot.Business.Models
{
    public class Snippet
    {

        public string Title { get; set; }
        public string Descripption { get; set; }
        public string ChannelTitle { get; set; }
        public string PublishTime { get; set; }

        public Snippet(string title, string description, string channel, string time)
        {
            this.Title = title;
            this.Descripption = description;
            this.ChannelTitle = channel;
            this.PublishTime = time;
        }
    }

}
