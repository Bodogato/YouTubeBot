using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace YouTubeBot.Business.Bot.Commands
{
    [TelegramBotCommand("/getcircle")]
    public class GetSomeSite : ICommand
    {
        private readonly ILogger<SearchCommand> _logger;
        private readonly ITelegramBotClient _bot;

        public GetSomeSite(ILogger<SearchCommand> logger, ITelegramBotClient bot)
        {
            _logger = logger;
            _bot = bot;

            Name = "/getcircle";
            CallBackPrefix = "none:";
        }

        public string Name { get; }
        public string CallBackPrefix { get; }

        public async Task ExecuteCommand(Update eventArgs)
        {
            var chatId = eventArgs.Message.Chat.Id;
            var url = @"https://en.touhouwiki.net/wiki/Doujin_circles";

            await _bot.SendTextMessageAsync(chatId,
                "Here is your link",
                replyMarkup:
                new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("PRESS ME",
                    url)));
        }

        public Task CallBackHandler(Update eventArgs)
        {
            return Task.CompletedTask;
        }
    }
}