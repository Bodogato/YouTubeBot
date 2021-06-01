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
    [TelegramBotCommand("/search")]
    public class SearchCommand : ICommand
    {
        private readonly ILogger<SearchCommand> _logger;
        private readonly ITelegramBotClient _bot;
        private readonly BotContext _database;
        private readonly string _apiUrl;
        public SearchCommand(IConfiguration configuration, ILogger<SearchCommand> logger, ITelegramBotClient bot, BotContext database)
        {
            _logger = logger;
            _bot = bot;
            _database = database;
            _apiUrl = configuration.GetSection("API")["ApiUrl"];

            Name = "/search";
            CallBackPrefix = "video_to_add_id:";
        }

        public string Name { get; }
        public string CallBackPrefix { get; }


        private async Task<IEnumerable<Showcase>> GetTopFiveVideos(string videoName)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(_apiUrl + $"/Youtube/First_5?SearchQuery={videoName}");

            var responseText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Showcase>>(responseText)!.Take(5);
        }

        public async Task ExecuteCommand(Update eventArgs)
        {
            var chatId = eventArgs.Message.Chat.Id;

            var videoName = eventArgs.Message.Text.Replace(Name, "");
            var showCases = await GetTopFiveVideos(videoName);

            var sb = new StringBuilder();
            var buttons = new List<List<InlineKeyboardButton>>();

            foreach (var showCase in showCases)
            {
                sb.AppendLine($"- {showCase.Snippet.Title}\n");

                buttons.Add(new List<InlineKeyboardButton>
                {
                    new()
                    {
                        Text = showCase.Snippet.Title,
                        CallbackData =
                            $"{CallBackPrefix}{showCase.Id.VideoId}" +
                            "|" +
                            $"{(showCase.Snippet.Title.Length > 10 ? showCase.Snippet.Title.Substring(0, 10) : showCase.Snippet.Title)}"
                    }
                });
            }

            var markup = new InlineKeyboardMarkup(buttons);

            await _bot.SendTextMessageAsync(chatId, $"Choose video to add to the latest playlist {videoName}\n", replyMarkup: markup);
        }

        public async Task CallBackHandler(Update eventArgs)
        {
            var data = eventArgs.CallbackQuery.Data.Split('|');
            var vidId = data[0].Split(':')[1];
            var vidName = data[1];

            await _bot.SendTextMessageAsync(eventArgs.CallbackQuery.From.Id, $"Video with name - {vidName} was added to library");

            await _database.Songs.AddAsync(new Song(vidName, vidId));
            await _database.SaveChangesAsync();
        }
    }
}