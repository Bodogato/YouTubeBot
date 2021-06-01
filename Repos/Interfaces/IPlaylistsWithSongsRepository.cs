namespace YouTube.DAL.Repos.Interfaces
{
    public interface IPlaylistsWithSongsRepository : IRepository
    {
        void AddSongToPlaylist(string songName, string playlistName);
        void RemoveSongFromPlaylist(string songName, string playlistName);
    }
}