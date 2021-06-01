using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using YouTube.DAL;

namespace YouTubeBot.Business.Bot.Commands
{
    [TelegramBotCommand("/getAllVideos")]
    public class GetAllVideos : ICommand
    {
        private readonly ILogger<SearchCommand> _logger;
        private readonly ITelegramBotClient _bot;
        private readonly BotContext _database;

        public GetAllVideos(ILogger<SearchCommand> logger, ITelegramBotClient bot, BotContext database)
        {
            _logger = logger;
            _bot = bot;
            _database = database;

            Name = "/getAllVideos";
            CallBackPrefix = "none:";
        }

        public string Name { get; }
        public string CallBackPrefix { get; }

        public async Task ExecuteCommand(Update eventArgs)
        {
            var chatId = eventArgs.Message.Chat.Id;

            var songs = await _database.Songs.ToListAsync();
            var sb = new StringBuilder();
            int c = 1;

            foreach (var song in songs)
            {
                sb.Append($"\n{c}. {song.Name}\n");
                c++;
            }

            var buttons =
                songs.Select(song => new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithUrl(song.Name,$"https://www.youtube.com/watch?v={song.VideoId}")
                });

            await _bot.SendTextMessageAsync(chatId, $"Your library {sb}", replyMarkup: new InlineKeyboardMarkup(buttons));
            await _database.SaveChangesAsync();
        }

        public Task CallBackHandler(Update eventArgs)
        {
            return Task.CompletedTask;
        }
    }
}