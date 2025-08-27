using Server.Core.Lobby;

namespace Server.Core
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("[DEBUG]: Debug console for Core project, independent from UI project.");
            Task.Run(() => Server.Instance.Start(args));

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSingleton(Server.Instance.lobbyManager);

            var app = builder.Build();

            app.MapControllers();

            //TODO: change to config
            app.Run("https://localhost:5001");
        }
    }
}