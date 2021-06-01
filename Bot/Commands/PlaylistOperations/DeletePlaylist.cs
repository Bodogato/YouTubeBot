using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

using YouTube.DAL;

namespace YouTubeBot.Business.Bot.Commands.PlaylistOperations
{
    [TelegramBotCommand("/deletePl")]
    public class DeletePlaylist : ICommand
    {
        private readonly ILogger<SearchCommand> _logger;
        private readonly ITelegramBotClient _bot;
        private readonly BotContext _database;

        public DeletePlaylist(ILogger<SearchCommand> logger, ITelegramBotClient bot, BotContext database)
        {
            _logger = logger;
            _bot = bot;
            _database = database;

            Name = "/deletePl";
            CallBackPrefix = "none:";
        }

        public string Name { get; }
        public string CallBackPrefix { get; }

        public async Task ExecuteCommand(Update eventArgs)
        {
            var chatId = eventArgs.Message.Chat.Id;

            var playlistName = eventArgs.Message.Text.Remove(0, Name.Length + 1);

            var playlist = await _database.Plalists.FirstOrDefaultAsync(e => Equals(e.Name, playlistName));

            if (playlist == null)
            {
                await _bot.SendTextMessageAsync(chatId, $"Playlist {playlistName} doesn't exist");
            }
            else
            {
                await _bot.SendTextMessageAsync(chatId, $"Playlist {playlistName} was deleted");
                _database.Plalists.Remove(playlist);
                await _database.SaveChangesAsync();
            }
        }

        public Task CallBackHandler(Update eventArgs)
        {
            return Task.CompletedTask;
        }
    }
}