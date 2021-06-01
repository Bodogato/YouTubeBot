using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

using YouTube.DAL;

namespace YouTubeBot.Business.Bot.Commands
{
    [TelegramBotCommand("/deleteSongFromPlaylist")]
    public class DeleteSongFromPlaylist : ICommand
    {
        private readonly ILogger<SearchCommand> _logger;
        private readonly ITelegramBotClient _bot;
        private readonly BotContext _database;

        public DeleteSongFromPlaylist(ILogger<SearchCommand> logger, ITelegramBotClient bot, BotContext database)
        {
            _logger = logger;
            _bot = bot;
            _database = database;

            Name = "/deleteSongFromPlaylist";
            CallBackPrefix = "none:";
        }

        public string Name { get; }
        public string CallBackPrefix { get; }

        public async Task ExecuteCommand(Update eventArgs)
        {
            var chatId = eventArgs.Message.Chat.Id;

            var newData = eventArgs.Message.Text.Remove(0, Name.Length + 1).Split(' ');

            var pl = await _database.Plalists.FirstOrDefaultAsync(e => Equals(e.Name, newData[0]));
            var song = await _database.Songs.FirstOrDefaultAsync(e => e.Name.Equals(newData[1]));

            if (pl == null)
            {
                _logger.LogWarning($"Playlist {newData[0]} not found");
                await _bot.SendTextMessageAsync(chatId, $"No playlist with name {newData[0]}");
                return;
            }

            if (song == null)
            {
                _logger.LogWarning($"Song {newData[1]} not found");
                await _bot.SendTextMessageAsync(chatId, $"No song with name {newData[0]}");
                return;
            }

            _database.PlaylistsWithSongs.Remove(await _database.PlaylistsWithSongs.FirstOrDefaultAsync(e => e.PlaylistId == pl.Id && e.SongId == song.Id));

            await _bot.SendTextMessageAsync(chatId, $"{pl.Name} updated");
            await _database.SaveChangesAsync();

        }

        public Task CallBackHandler(Update eventArgs)
        {
            return Task.CompletedTask;
        }
    }
}