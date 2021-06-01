namespace YouTube.DAL.Models
{
    public class Song : Entity
    {
        public string Name { get; set; }
        public string VideoId { get; set; }

        public Song(string name, string videoId)
        {
            Name = name.Replace(" ", "_");
            VideoId = videoId;
        }
    }
}