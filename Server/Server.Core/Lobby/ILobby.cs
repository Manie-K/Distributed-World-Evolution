using System.Net.Sockets;

namespace Server.Core.Lobby
{
    public interface ILobby
    {
        /// <summary>
        /// Unique identifier for the lobby. Read-only after lobby creation.
        /// </summary>
        public int LobbyId { get; }
        public string Name { get; set; }
        public int MaxPlayers { get; set; }
        public int MapId { get; set; }


        public bool AddClient(TcpClient client);
        public bool RemoveClient(TcpClient client);
        public void Run();
    }
}
