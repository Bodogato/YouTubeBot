namespace YouTube.DAL.Models
{
    public class PlaylistsWithSongs : Entity
    {
        public int SongId { get; set; }
        public int PlaylistId { get; set; }

        public PlaylistsWithSongs(int songId, int playlistId)
        {
            SongId = songId;
            PlaylistId = playlistId;
        }
    }
}