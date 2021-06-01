using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using YouTubeBot.Business.Bot.Commands;

namespace YouTubeBot.Business.Bot
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ILogger<UpdateHandler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<ICommand> _commands;

        public UpdateHandler(ILogger<UpdateHandler> logger, IServiceProvider serviceProvider, ITelegramBotClient client)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _commands = serviceProvider.GetServices(typeof(ICommand)).Select(o => (ICommand)o);
        }

        public async Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;

            switch (update.Type)
            {
                case UpdateType.Message:
                    if (!string.IsNullOrEmpty(update.Message.Text))
                    {
                        await CmdHandler(botClient, scopedServiceProvider, update);
                    }

                    break;

                case UpdateType.CallbackQuery:
                    await CallBackHandler(scopedServiceProvider, update);
                    break;
            }
        }

        public Task HandleError(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "error");

            return Task.CompletedTask;
        }

        public UpdateType[]? AllowedUpdates =>
            new[] { UpdateType.Message, UpdateType.CallbackQuery, UpdateType.InlineQuery };

        private async Task CmdHandler(ITelegramBotClient botClient, IServiceProvider serviceProvider, Update update)
        {
            ICommand command;

            try
            {
                command = _commands.FirstOrDefault(cmd => cmd.Name.StartsWith(update.Message.Text.Split(' ')[0]));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error");
                throw;
            }

            var x = command?.ExecuteCommand(update);

            try
            {
                if (x != null)
                {
                    await x;
                }
                else
                {
                    _logger.LogWarning($"Unknown command {update.Message.Text}");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error due to {update.Message.Text} command processing");
            }
        }

        private async Task CallBackHandler(IServiceProvider serviceProvider, Update update)
        {
            ICommand command;

            try
            {
                var prefixStarts = update.CallbackQuery.Data.Split(':')[0];
                _logger.LogInformation(prefixStarts);

                command = _commands.FirstOrDefault(cmd => cmd.CallBackPrefix.StartsWith(prefixStarts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error");
                throw;
            }

            var x = command?.CallBackHandler(update);

            try
            {
                if (x != null)
                {
                    await x;
                }
                else
                {
                    _logger.LogWarning($"Unknown callback {update.CallbackQuery.Data}");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error due to {update.CallbackQuery.Data} callback processing");
            }
        }
    }
}