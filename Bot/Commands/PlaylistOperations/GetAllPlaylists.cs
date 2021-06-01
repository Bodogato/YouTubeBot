using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using YouTube.DAL;

namespace YouTubeBot.Business.Bot.Commands.PlaylistOperations
{
    [TelegramBotCommand("/getAllPlaylists")]
    public class GetAllPlaylists : ICommand
    {
        private readonly ILogger<SearchCommand> _logger;
        private readonly ITelegramBotClient _bot;
        private readonly BotContext _database;

        public GetAllPlaylists(ILogger<SearchCommand> logger, ITelegramBotClient bot, BotContext database)
        {
            _logger = logger;
            _bot = bot;
            _database = database;

            Name = "/getAllPlaylists";
            CallBackPrefix = "none:";
        }

        public string Name { get; }
        public string CallBackPrefix { get; }

        public async Task ExecuteCommand(Update eventArgs)
        {
            var chatId = eventArgs.Message.Chat.Id;
            var allPlaylists = await _database.Plalists.ToListAsync();

            var buttons = allPlaylists.Select(playlist => new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = playlist.Name,
                    CallbackData = $"show_playlist_name:{playlist.Name}"
                }
            }).ToList();

            await _bot.SendTextMessageAsync(chatId, "There are all play-lists", replyMarkup: new InlineKeyboardMarkup(buttons));
        }

        public Task CallBackHandler(Update eventArgs)
        {
            return Task.CompletedTask;
        }
    }
}