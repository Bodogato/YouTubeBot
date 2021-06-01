using System.Threading.Tasks;

using Telegram.Bot.Types;

namespace YouTubeBot.Business.Bot.Commands
{
    public interface ICommand
    {
        public string Name { get; }
        public string CallBackPrefix { get; }
        public Task ExecuteCommand(Update eventArgs);
        public Task CallBackHandler(Update eventArgs);
    }
}