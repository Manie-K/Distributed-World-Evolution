using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Core.Lobby;
using Server.Shared.Logging;

namespace Server.Core
{
    //@FranciszekGwarek
    internal class Server
    {
        private readonly LobbyManager lobbyManager;
        private ILogger logger;

        public Server()
        {
            lobbyManager = new LobbyManager(new Logger());
            logger = new Logger();
        }

        public void Start(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();

            logger.Log("Main server started...", LogLevel.Info);

            while(true)
            {
                TcpClient client = listener.AcceptTcpClient();
                //Here we have a few options:

                //1. ThreadPool.QueueUserWorkItem(HandleClientConnection, client);
                //2. Convert it into async method
                Task.Factory.StartNew(() => HandleClientConnection(client), TaskCreationOptions.LongRunning); //3.
            }
        }

        void HandleClientConnection(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int byteCount = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, byteCount).Trim();

            //Creating new lobby
            if (message.StartsWith("CREATE"))
            {
                int lobbyId = lobbyManager.CreateAndInitialiseLobby();
                try
                {
                    lobbyManager.AddUserToLobby(lobbyId, client);
                }
                catch (Exception ex)
                {
                    logger.Log(ex, LogLevel.Error);
                    client.Close();
                }
            }

            //Joining existing lobby
            else if (message.StartsWith("JOIN"))
            {
                string[] parts = message.Split(' ');
                if (parts.Length >= 2 && int.TryParse(parts[1], out int joinId))
                {
                    try
                    {
                        lobbyManager.AddUserToLobby(joinId, client);
                    }
                    catch (Exception ex)
                    {
                        logger.Log(ex, LogLevel.Error);
                        client.Close();
                    }
                }
            }
            else
            {
                byte[] error = Encoding.UTF8.GetBytes("Invalid command. Use CREATE or JOIN <id>.\n");
                stream.Write(error, 0, error.Length);
                client.Close();
            }
        }
    }
}
