using Server.Shared.Exceptions;
using SharedLibrary;
using SharedLibrary.LobbyDTO;
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
        public int CreateAndInitializeLobby()
        {
            int lobbyId;
            
            lock (lobbies)
            {
                lobbyId = lobbyCounter++;
                lobbies[lobbyId] = new Lobby(lobbyId);
                //lobbies[lobbyId].Run();
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
                    Log($"Client added to lobby {lobbyId}.", LogLevelEnum.Info);

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
