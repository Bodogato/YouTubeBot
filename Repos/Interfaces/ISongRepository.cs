using System.Collections.Generic;
using System.Threading.Tasks;

using YouTube.DAL.Models;

namespace YouTube.DAL.Repos.Interfaces
{
    public interface ISongRepository : IRepository
    {
        Task AddNewSong(Song song);
        Task<IEnumerable<Song>> GetAllSongs();
    }
}