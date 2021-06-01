using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

using YouTube.DAL;
using YouTube.DAL.Models;

namespace YouTubeBot.Business.Bot.Commands.PlaylistOperations
{
    [TelegramBotCommand("/createPl")]
    public class CreatePlaylist : ICommand
    {
        private readonly ILogger<SearchCommand> _logger;
        private readonly ITelegramBotClient _bot;
        private readonly BotContext _database;

        public CreatePlaylist(ILogger<SearchCommand> logger, ITelegramBotClient bot, BotContext database)
        {
            _logger = logger;
            _bot = bot;
            _database = database;

            Name = "/createPl";
            CallBackPrefix = "none:";
        }

        public string Name { get; }
        public string CallBackPrefix { get; }

        public async Task ExecuteCommand(Update eventArgs)
        {
            var chatId = eventArgs.Message.Chat.Id;

            var playlistName = eventArgs.Message.Text.Remove(0, Name.Length + 1);

            await _bot.SendTextMessageAsync(chatId, $"New playlist was created - {playlistName}");
            await _database.Plalists.AddAsync(new Playlist(playlistName));
            await _database.SaveChangesAsync();
        }

        public Task CallBackHandler(Update eventArgs)
        {
            return Task.CompletedTask;
        }
    }
}