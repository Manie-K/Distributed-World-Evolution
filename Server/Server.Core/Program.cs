using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Core.Data;
using Server.Core.Lobby;
using Server.Core.Services;

namespace Server.Core
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("[DEBUG]: Debug console for Core project, independent from UI project.");

            if (AppDomain.CurrentDomain.GetAssemblies()
                    .Any(a => a.FullName?.StartsWith("Microsoft.EntityFrameworkCore.Design") == true))
            {
                return;
            }

            bool dbReady = false;
            while (!dbReady)
            {
                try
                {
                    using var context = new ApplicationDBContext();
                    context.Database.Migrate();
                    dbReady = true;
                }
                catch (Npgsql.NpgsqlException)
                {
                    Console.WriteLine("[INFO]: Waiting for database...");
                    Thread.Sleep(2000);
                }
            }

            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton<Server>();
            builder.Services.AddSingleton<LobbyManager>();
            builder.Services.AddSingleton<LoggerService>();
            builder.Services.AddHostedService(provider => provider.GetRequiredService<LoggerService>());

            var host = builder.Build();
            await host.StartAsync();

            var server = host.Services.GetRequiredService<Server>();
            await server.StartAsync(args);

            await host.WaitForShutdownAsync();
        }
    }
}