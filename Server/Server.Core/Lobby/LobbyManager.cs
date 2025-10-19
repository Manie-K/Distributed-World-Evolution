using Server.Core.Behaviours;
using Server.Core.Exceptions;
using Server.Core.Modules;
using Server.Core.Services;
using SharedLibrary;
using SharedLibrary.Logging;
using System.Net.Sockets;

namespace Server.Core.Lobby
{
    public class LobbyManager
    {
        public readonly Dictionary<int, ILobby> lobbies;
        private int lobbyCounter;

        public event EventHandler<OnLogEventArgs>? OnLog;

        public LobbyManager()
        {
            lobbyCounter = 0;
            lobbies = new Dictionary<int, ILobby>();
        }

        //TODO: add modules when they are implemented
        public int CreateAndInitialiseLobby(string name, int maxPlayers, int mapId, IEnumerable<int> modulesIDs)
        {
            int lobbyId;
            
            lock (lobbies)
            {
                lobbyId = lobbyCounter++;
                lobbies[lobbyId] = Lobby.CreateLobby(lobbyId, name, maxPlayers, mapId, modulesIDs, ModuleService.Instance);
                Task.Factory.StartNew(() => lobbies[lobbyId].Run(), TaskCreationOptions.LongRunning);
            }

            return lobbyId;
        }

        public bool AddUserToLobby(int lobbyId, TcpClient client, string username, out Guid userEntityID)
        {
            userEntityID = Guid.Empty;
            if (lobbies.TryGetValue(lobbyId, out ILobby? lobby))
            {
                if(lobby is not null)
                {
                    userEntityID = lobby.AddClient(client, username);
                    Log($"Client added to lobby {lobbyId}.", LogLevelEnum.Info);
                    return true;
                }
                else
                {
                    Log($"Lobby with ID {lobbyId} is null.", LogLevelEnum.Error);
                    throw new NullLobbyException($"Lobby with ID {lobbyId} is null.");
                }
            }

            Log($"Lobby with ID {lobbyId} does not exist.", LogLevelEnum.Warning);
            throw new NullLobbyException($"Lobby with ID {lobbyId} does not exist.");
        }

        public bool RemoveUserFromLobby(int lobbyId, TcpClient client)
        {
            if (lobbies.TryGetValue(lobbyId, out ILobby? lobby))
            {
                if (lobby is not null)
                {
                    lobby.RemoveClient(client);
                    Log($"Client removed from lobby {lobbyId}.", LogLevelEnum.Info);

                    return true;
                }
                else
                {
                    throw new NullLobbyException($"Lobby with ID {lobbyId} does not exist or is null.");
                }
            }

            Log($"Lobby with ID {lobbyId} does not exist.", LogLevelEnum.Warning);
            return false;
        }

        private void Log(Exception ex, LogLevelEnum level)
        {
            Log(ex.Message, level);
        }

        private void Log(string message, LogLevelEnum level)
        {
            OnLogEventArgs args = new OnLogEventArgs(message, level);

            OnLog?.Invoke(this, args);
        }
    }
}
