using Server.Shared.Logging;
using Server.Shared.Exceptions;
using System.Net.Sockets;

namespace Server.Core.Lobby
{
    internal class LobbyManager
    {
        private readonly Dictionary<int, ILobby> lobbies;
        private ILogger logger;
        private int lobbyCounter;

        public LobbyManager(ILogger logger)
        {
            lobbyCounter = 0;
            lobbies = new Dictionary<int, ILobby>();
            this.logger = logger;
        }

        //TODO: add modules when they are implemented
        public int CreateAndInitializeLobby()
        {
            int lobbyId;
            
            lock (lobbies)
            {
                lobbyId = lobbyCounter++;
                lobbies[lobbyId] = new Lobby(lobbyId, new Logger());
                lobbies[lobbyId].Run();
                Task.Factory.StartNew(() => lobbies[lobbyId].Run(), TaskCreationOptions.LongRunning);
            }

            return lobbyId;
        }

        public bool AddUserToLobby(int lobbyId, TcpClient client)
        {
            if (lobbies.TryGetValue(lobbyId, out ILobby? lobby))
            {
                if(lobby is not null)
                {
                    lobby.AddClient(client);
                    logger.Log($"Client added to lobby {lobbyId}.", LogLevel.Info);

                    return true;
                }
                else
                {
                    throw new NullLobbyException($"Lobby with ID {lobbyId} does not exist or is null.");
                }
            }

            logger.Log($"Lobby with ID {lobbyId} does not exist.", LogLevel.Warning);
            return false;
        }
    }
}
