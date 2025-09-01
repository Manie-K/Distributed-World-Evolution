using System.Net.Sockets;

namespace Server.Core.Lobby
{
    public interface ILobby
    {
        /// <summary>
        /// Unique identifier for the lobby. Read-only after lobby creation.
        /// </summary>
        public int LobbyId { get; }

        public void AddClient(TcpClient client);
        public void Run();
    }
}
