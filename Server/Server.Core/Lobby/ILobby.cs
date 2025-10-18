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

        /// <summary>
        /// Adds a client to the lobby and returns a unique identifier for the user world entity.
        /// </summary>
        /// <param name="client">TCP client.</param>
        /// <param name="username">Name of user's entity.</param>
        /// <returns></returns>
        public Guid AddClient(TcpClient client, string username);
        public bool RemoveClient(TcpClient client);
        public void Run();
    }
}
