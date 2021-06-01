using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using YouTube.DAL;
using YouTube.DAL.Models;

using YouTubeBot.Business.Models;


namespace YouTubeBot.Business.Bot.Commands
{
     [TelegramBotCommand("/help")]
    public class Help : ICommand
    {
        private readonly ILogger<SearchCommand> _logger;
        private readonly ITelegramBotClient _bot;

        public Help(ILogger<SearchCommand> logger, ITelegramBotClient bot)
        {
            _logger = logger;
            _bot = bot;

            Name = "/help";
            CallBackPrefix = "none:";
        }

            public string Name { get; }
            public string CallBackPrefix { get; }

            public async Task ExecuteCommand(Update eventArgs)
            {
                var chatId = eventArgs.Message.Chat.Id;

                await _bot.SendTextMessageAsync(chatId, @"
                /deleteSongFromPlaylist playlist_name song_name
                /getAllSongsFromPlaylist playlist_name
                /addSongToPlaylist playlist_name song_name
                /getAllVideos
                /getcircle

                /createPlaylist playlist_name
                /deletePlaylist playlist_name");
            }

         public Task CallBackHandler(Update eventArgs)
         {
             return Task.CompletedTask;
         }
    }
}
