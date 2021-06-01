using System;

namespace YouTubeBot.Business.Bot
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TelegramBotCommand : Attribute
    {
        public TelegramBotCommand(string cmdName)
        {
            CommandName = cmdName;

            if (cmdName[0] != '/')
            {
                throw new ArgumentException("Command must start with dash '/'!");
            }
        }

        public string CommandName { get; }
    }
}