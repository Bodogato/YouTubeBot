using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Linq;

using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

using YouTube.DAL;

using YouTubeBot.Business.Bot;
using YouTubeBot.Business.Bot.Commands;

namespace YouTubeBot.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var botToken = Configuration.GetSection("Bot")["Token"];
            services.AddControllers();

            // ================ bot dependencies ================ //
            services.AddSingleton<IUpdateHandler, UpdateHandler>();
            services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(botToken));
            services.AddHostedService<TeleBot>();

            var types = typeof(ICommand).Assembly.GetTypes()
                .Where(type => typeof(ICommand).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var type in types)
            {
                services.AddScoped(typeof(ICommand), type);
            }

            // ================ bot dependencies ================ //


            services.AddDbContext<BotContext>(options =>
                options.UseSqlServer(@"Server=bestdbever.database.windows.net;Database=DaployDB;User=trickster;Password=bestdb12!;Integrated Security=False;"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DatabaseManagementService.MigrationInitialisation(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
