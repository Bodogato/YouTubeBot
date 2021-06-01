using Microsoft.EntityFrameworkCore;

using YouTube.DAL.Models;

namespace YouTube.DAL
{

    public class BotContext : DbContext
    {
        public BotContext(DbContextOptions<BotContext> options) : base(options)
        {
        }

        public DbSet<Playlist> Plalists { get; set; }

        public DbSet<PlaylistsWithSongs> PlaylistsWithSongs { get; set; }

        public DbSet<Song> Songs { get; set; }
    }
}