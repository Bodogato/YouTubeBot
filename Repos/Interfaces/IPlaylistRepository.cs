using System.Collections.Generic;
using System.Threading.Tasks;

using YouTube.DAL.Models;

namespace YouTube.DAL.Repos.Interfaces
{
    public interface IPlaylistRepository : IRepository
    {
        Task AddNewPlaylist(Playlist playlist);
        Task DeletePlaylist(string playlistName);
        Task<List<Playlist>> GetAllPlaylists();
    }
}