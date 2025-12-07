using Server.Core.Exceptions;
using Server.Core.Services;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using System.CodeDom;
using System.Net.Sockets;

namespace Server.Core.Lobby
{
    public class LobbyManager
    {
        private readonly Dictionary<int, ILobby> lobbies;
        private int lobbyCounter;
        private readonly object lobbyLock = new object();

        public event EventHandler<OnLogEventArgs>? OnLog;

        public LobbyManager()
        {
            lobbyCounter = 0;
            lobbies = new Dictionary<int, ILobby>();
        }

        public int CreateAndInitializeLobby(string name, int maxPlayers, int mapId, bool[][] walkableTiles, bool[][] fertileTiles, IEnumerable<int> modulesIDs)
        {
            int lobbyId;
            
            lock (lobbyLock)
            {
                lobbyId = lobbyCounter++;
                lobbies[lobbyId] = Lobby.CreateLobby(lobbyId, name, maxPlayers, mapId, walkableTiles, fertileTiles, modulesIDs, ModuleService.Instance);
                lobbies[lobbyId].OnLobbyClosed += () =>
                {
                    lock (lobbyLock)
                    {
                        lobbies.Remove(lobbyId);
                        Log($"Lobby {lobbyId} closed and removed from LobbyManager.", LogLevelEnum.Info);
                    }
                };

                Task.Factory.StartNew(() => lobbies[lobbyId].Run(), TaskCreationOptions.LongRunning);
            }

            return lobbyId;
        }

        public bool AddUserToLobby(int lobbyId, TcpClient client, string username, out Guid userEntityID)
        {
            userEntityID = Guid.Empty;
            lock (lobbyLock)
            {
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
            }

            Log($"Lobby with ID {lobbyId} does not exist.", LogLevelEnum.Warning);
            throw new NullLobbyException($"Lobby with ID {lobbyId} does not exist.");
        }

        public bool RemoveUserFromLobby(int lobbyId, TcpClient client)
        {
            lock (lobbyLock)
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
            }

            Log($"Lobby with ID {lobbyId} does not exist.", LogLevelEnum.Warning);
            return false;
        }

        public Lobby GetLobby(int lobbyId)
        {
            lock (lobbyLock)
            {
                if (lobbies.TryGetValue(lobbyId, out ILobby? lobby))
                {
                    if (lobby is not null)
                    {
                        return (Lobby)lobby;
                    }
                    else
                    {
                        throw new NullLobbyException($"Lobby with ID {lobbyId} does not exist or is null.");
                    }
                }
                throw new NullLobbyException($"Lobby with ID {lobbyId} does not exist.");
            }
        }

        public void RemoveUserFromLobbies(TcpClient client)
        {
            lock (lobbyLock)
            {
                foreach (var lobby in lobbies.Values)
                {
                    if (lobby.IsClientPresent(client))
                    {
                        lobby.RemoveClient(client);
                    }
                }
            }
        }

        public void SendMessageToLobbyWithClient(TcpClient client, MessageBase message)
        {
            lock (lobbyLock)
            {
                foreach (var lobby in lobbies.Values)
                {
                    lobby.HandleClientMessage(client, message);
                }
            }
        }

        public List<ILobby> GetAllLobbies()
        {
            lock (lobbyLock)
            {
                return lobbies.Values.ToList();
            }
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
