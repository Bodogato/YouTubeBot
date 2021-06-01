namespace YouTube.DAL.Models
{
    public class Playlist : Entity
    {
        public string Name { get; set; }

        public Playlist(string name)
        {
            Name = name;
        }
    }
}