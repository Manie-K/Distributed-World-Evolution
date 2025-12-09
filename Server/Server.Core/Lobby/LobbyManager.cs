using Server.Core.Exceptions;
using Server.Core.Services;
using SharedLibrary.DTOs.LobbyDTO;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using System.Net.Sockets;

namespace Server.Core.Lobby
{
    /// <summary>
    /// Singleton class that manages multiple lobbies.
    /// </summary>
    public class LobbyManager : ILobbyManager
    {
        private readonly Dictionary<int, ILobby> lobbies;
        private readonly LoggerService loggerService;
        
        private readonly object lobbyLock;
        
        private int lobbyCounter;

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public LobbyManager(LoggerService loggerService)
        {
            this.loggerService = loggerService;
            this.lobbyLock = new object();

            lobbyCounter = 0;
            lobbies = new Dictionary<int, ILobby>();
            Lobby.OnLog += OnLog_Delegate;
        }

        #endregion


        #region ILobbyManager Implementation

        /// <inheritdoc/>
        public int CreateAndInitializeLobby(string name, int maxPlayers, int mapId, bool[][] walkableTiles, bool[][] fertileTiles, IEnumerable<int> modulesIDs)
        {
            int lobbyId;
            
            lock (lobbyLock)
            {
                lobbyId = lobbyCounter++;
                
                lobbies[lobbyId] = Lobby.CreateLobby(lobbyId, name, maxPlayers, mapId, walkableTiles, fertileTiles, modulesIDs, ModuleService.Instance);
                lobbies[lobbyId].OnLobbyClosed += () =>
                {
                    lobbies.Remove(lobbyId);
                    loggerService.Log($"Lobby {lobbyId} closed and removed from LobbyManager.", LogLevelEnum.Info);
                };

                Task.Factory.StartNew(() => lobbies[lobbyId].Run(), TaskCreationOptions.LongRunning);
            }

            return lobbyId;
        }

        /// <inheritdoc/>
        public bool AddUserToLobby(int lobbyId, TcpClient client, string username, out Guid userEntityId)
        {
            lock (lobbyLock)
            {
                if (lobbies.TryGetValue(lobbyId, out ILobby? lobby))
                {
                    if(lobby is not null)
                    {
                        userEntityId = lobby.AddClient(client, username);
                        if (userEntityId != Guid.Empty)
                        {
                            loggerService.Log($"Client added to lobby {lobbyId}.", LogLevelEnum.Info);
                            return true;
                        }

                        loggerService.Log($"Client not added to lobby {lobbyId}. Lobby is full.", LogLevelEnum.Info);
                        return false;
                    }
                    else
                    {
                        throw new LobbyNotFoundException($"Lobby with ID {lobbyId} is null.");
                    }
                }
            }

            throw new LobbyNotFoundException($"Lobby with ID {lobbyId} does not exist.");
        }

        /// <inheritdoc/>
        public void RemoveUserFromLobby(int lobbyId, TcpClient client)
        {
            lock (lobbyLock)
            {
                if (lobbies.TryGetValue(lobbyId, out ILobby? lobby))
                {
                    if (lobby is not null)
                    {
                        lobby.RemoveClient(client);
                        loggerService.Log($"Client removed from lobby {lobbyId}.", LogLevelEnum.Info);
                        return;
                    }
                    else
                    {
                        throw new LobbyNotFoundException($"Lobby with ID {lobbyId} is null.");
                    }
                }
            }

            throw new LobbyNotFoundException($"Lobby with ID {lobbyId} does not exist.");
        }

        /// <inheritdoc/>
        public LobbyDTO GetLobbyData(int lobbyId)
        {
            lock (lobbyLock)
            {
                if (lobbies.TryGetValue(lobbyId, out ILobby? lobby))
                {
                    if (lobby is not null)
                    {
                        return lobby.ToDTO();
                    }
                    else
                    {
                        throw new LobbyNotFoundException($"Lobby with ID {lobbyId} does not exist or is null.");
                    }
                }
                throw new LobbyNotFoundException($"Lobby with ID {lobbyId} does not exist.");
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public List<LobbyDTO> GetAllLobbiesData()
        {
            lock (lobbyLock)
            {
                return lobbies.Values.Select(l => l.ToDTO()).ToList();
            }
        }

        #endregion


        #region Logging

        /// OnLog event handler to route log messages to the logger service.
        private void OnLog_Delegate(object? sender, OnLogEventArgs e)
        {
            loggerService.Log(e.Content, e.LogLevel, sender);
        }

        #endregion

    }
}
