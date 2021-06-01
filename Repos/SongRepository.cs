using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

using YouTube.DAL.Models;
using YouTube.DAL.Repos.Interfaces;

namespace YouTube.DAL.Repos
{
    public class SongRepository : ISongRepository
    {
        private readonly BotContext _botContext;

        public SongRepository(BotContext botContext)
        {
            _botContext = botContext;
        }

        public async Task AddNewSong(Song song)
        {
            await _botContext.Songs.AddAsync(song);
        }

        public async Task<IEnumerable<Song>> GetAllSongs()
        {
            var allSongs = await _botContext.Songs.ToListAsync();
            return allSongs;
        }
    }
}