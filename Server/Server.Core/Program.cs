using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server.Core
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("[DEBUG]: Debug console for Core project, independent from UI project.");
            Server server = new Server();
            server.Start(args);
        }
    }
}