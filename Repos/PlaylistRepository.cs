using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

using YouTube.DAL.Models;
using YouTube.DAL.Repos.Interfaces;

namespace YouTube.DAL.Repos
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly BotContext _botContext;

        public PlaylistRepository(BotContext botContext)
        {
            _botContext = botContext;
        }

        public async Task AddNewPlaylist(Playlist playlist)
        {
            await _botContext.Plalists.AddAsync(playlist);
        }

        public async Task DeletePlaylist(string playlistName)
        {
            var pl = await _botContext.Plalists.FirstAsync(e => Equals(e.Name, playlistName));

            _botContext.Plalists.Remove(pl);
        }

        public async Task<List<Playlist>> GetAllPlaylists()
        {
            return await _botContext.Plalists.ToListAsync();
        }
    }
}