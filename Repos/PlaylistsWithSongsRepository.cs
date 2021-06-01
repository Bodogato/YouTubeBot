using System;
using System.Linq;

using YouTube.DAL.Models;

namespace YouTube.DAL.Repos
{
    public class PlaylistsWithSongsRepository
    {
        private readonly BotContext _botContext;

        public PlaylistsWithSongsRepository(BotContext botContext)
        {
            _botContext = botContext;
        }

        public void AddSongToPlaylist(string songName, string playlistName)
        {
            var pl = _botContext.Plalists.FirstOrDefault(e => Equals(e.Name, playlistName)) ?? throw new ArgumentNullException("_botContext.Plalists.FirstOrDefault(e => Equals(e.Name, playlistName))");
            var sn = _botContext.Songs.FirstOrDefault(e => Equals(e.Name, songName)) ?? throw new ArgumentNullException("_botContext.Songs.FirstOrDefault(e => Equals(e.Name, songName))");

            _botContext.PlaylistsWithSongs.AddAsync(new PlaylistsWithSongs(sn.Id, pl.Id)
            );
        }

        public void RemoveSongFromPlaylist(string songName, string playlistName)
        {
            var pl = _botContext.Plalists.FirstOrDefault(e => Equals(e.Name, playlistName)) ?? throw new ArgumentNullException("_botContext.Plalists.FirstOrDefault(e => Equals(e.Name, playlistName))");
            var sn = _botContext.Songs.FirstOrDefault(e => Equals(e.Name, songName)) ?? throw new ArgumentNullException("_botContext.Songs.FirstOrDefault(e => Equals(e.Name, songName))");

            var ent = _botContext.PlaylistsWithSongs
                .FirstOrDefault(e => e.PlaylistId == pl.Id && e.SongId == sn.Id);


            _botContext.PlaylistsWithSongs.Remove(ent);
        }
    }
}