using Microsoft.EntityFrameworkCore;
using Server.Core.Data;

namespace Server.Core
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("[DEBUG]: Debug console for Core project, independent from UI project.");

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

            Server.Instance.Start(args);
        }
    }
}