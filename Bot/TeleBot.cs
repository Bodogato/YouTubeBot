using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace YouTubeBot.Business.Bot
{
    public class TeleBot : BackgroundService
    {
        private readonly ITelegramBotClient _client;

        private readonly IUpdateHandler _updateHandler;
        private readonly ILogger<TeleBot> _logger;

        public TeleBot(ITelegramBotClient client, IUpdateHandler updateHandler, ILogger<TeleBot> logger)
        {
            _client = client;
            _updateHandler = updateHandler;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _client.ReceiveAsync(_updateHandler, stoppingToken);
        }
    }
}