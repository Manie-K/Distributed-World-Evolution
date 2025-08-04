using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Core.Lobby;
using Server.Shared.Logging;
using Server.Shared.Messages;

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

            logger.Log("Server started...", LogLevel.Info);

            while(true)
            {
                TcpClient client = listener.AcceptTcpClient();
                //Here we have a few options:

                //1. ThreadPool.QueueUserWorkItem(HandleClientConnection, client);
                //2. Convert it into async method
                Task.Factory.StartNew(() => HandleClientConnection(client), TaskCreationOptions.LongRunning); //3.
                logger.Log("New client joined server.", LogLevel.Info);
                MessageManager.SendMessage(client, new DefaultMessage("Welcome to the server!"));
            }
        }

        void HandleClientConnection(TcpClient client)
        {
            MessageBase message = MessageManager.ReceiveMessage(client);

            //Creating new lobby
            if (message.MessageType == MessageType.CreateLobby)
            {
                CreateLobbyMessage createLobbyMessage = (CreateLobbyMessage)message;

                //TODO: validate message; transfer data from message to lobby
                int lobbyId = lobbyManager.CreateAndInitializeLobby();
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
            else if (message.MessageType == MessageType.JoinLobby)
            {
                JoinLobbyMessage joinLobbyMessage = (JoinLobbyMessage)message;

                try
                {
                    lobbyManager.AddUserToLobby(joinLobbyMessage.LobbyID, client);
                }
                catch (Exception ex)
                {
                    logger.Log(ex, LogLevel.Error);
                    client.Close();
                }
            }

            //Else
            else
            {
                client.Close();
            }

        }
    }
}
