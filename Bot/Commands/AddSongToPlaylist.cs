using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

using YouTube.DAL;
using YouTube.DAL.Models;

namespace YouTubeBot.Business.Bot.Commands
{
    [TelegramBotCommand("/addSongToPlaylist")]
    public class AddSongToPlaylist : ICommand
    {
        private readonly ILogger<SearchCommand> _logger;
        private readonly ITelegramBotClient _bot;
        private readonly BotContext _database;

        public AddSongToPlaylist(ILogger<SearchCommand> logger, ITelegramBotClient bot, BotContext database)
        {
            _logger = logger;
            _bot = bot;
            _database = database;

            Name = "/addSongToPlaylist";
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
                await _bot.SendTextMessageAsync(chatId, $"No playlist with name {newData[0]}");
                _logger.LogWarning($"Playlist {newData[0]} not found");
                return;
            }
            if (song == null)
            {
                await _bot.SendTextMessageAsync(chatId, $"No song with name {newData[1]}");
                _logger.LogWarning($"Song {newData[1]} not found");
                return;
            }

            await _database.PlaylistsWithSongs.AddAsync(new PlaylistsWithSongs(song.Id, pl.Id));
            await _bot.SendTextMessageAsync(chatId, $"Playlist {newData[0]} was updated by song {newData[1]}");
            await _database.SaveChangesAsync();
        }

        public Task CallBackHandler(Update eventArgs)
        {
            return Task.CompletedTask;
        }
    }
}