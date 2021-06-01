using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using YouTube.DAL;
using YouTube.DAL.Models;

namespace YouTubeBot.Business.Bot.Commands
{
    [TelegramBotCommand("/getAllSongsFromPlaylist")]
    public class GetAllSongsFromPlaylist : ICommand
    {
        private readonly ILogger<SearchCommand> _logger;
        private readonly ITelegramBotClient _bot;
        private readonly BotContext _database;

        public GetAllSongsFromPlaylist(ILogger<SearchCommand> logger, ITelegramBotClient bot, BotContext database)
        {
            _logger = logger;
            _bot = bot;
            _database = database;

            Name = "/getAllSongsFromPlaylist";
            CallBackPrefix = "show_playlist_name:";
        }

        public string Name { get; }
        public string CallBackPrefix { get; }

        public async Task ExecuteCommand(Update eventArgs)
        {
            var chatId = eventArgs.Message.Chat.Id;

            var playlistName = eventArgs.Message.Text.Remove(0, Name.Length + 1);

            await GetAllSongsFromPlaylistName(playlistName, chatId);
        }

        private async Task GetAllSongsFromPlaylistName(string name, long chatId)
        {
            var url = "https://www.youtube.com/watch_videos?video_ids=";

            var pl = await _database.Plalists.FirstOrDefaultAsync(e => Equals(e.Name, name));

            if (pl == null)
            {
                _logger.LogWarning($"{typeof(GetAllSongsFromPlaylist)} Playlist {name} not found");
                await _bot.SendTextMessageAsync(chatId, $"There are no {name} playlist");

                return;
            }

            var allSongsIds = await _database.PlaylistsWithSongs
                .Where(e => e.PlaylistId == pl.Id)
                .Select(e => e.SongId).ToListAsync();

            var songs = new List<Song>();

            foreach (var songId in allSongsIds)
            {
                var song = await _database.Songs.FirstOrDefaultAsync(e => e.Id == songId);

                if (song != null)
                {
                    _logger.LogInformation($"{song.Name} {song.VideoId}");
                    url += $"{song.VideoId},";
                    songs.Add(song);
                }
            }


            var buttons =
                songs.Select(song => new List<InlineKeyboardButton>
                {
                     InlineKeyboardButton.WithUrl(song.Name,$"https://www.youtube.com/watch?v={song.VideoId}")
                });

            await _bot.SendTextMessageAsync(
                chatId,
                $"There are {songs.Count} songs from playlist <a href=\"{url}\">{pl.Name}</a>",
                replyMarkup: new InlineKeyboardMarkup(buttons),
                parseMode: ParseMode.Html);
        }

        public async Task CallBackHandler(Update eventArgs)
        {
            _logger.LogInformation(eventArgs.CallbackQuery.Data);

            var data = eventArgs.CallbackQuery.Data.Replace(CallBackPrefix, "");
            var chatId = eventArgs.CallbackQuery.Message.Chat.Id;

            await GetAllSongsFromPlaylistName(data, chatId);
        }
    }
}