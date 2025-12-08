using SharedLibrary.DTOs.LobbyDTO;
using SharedLibrary.Messages;
using System.Net.Sockets;

namespace Server.Core.Lobby
{
    public interface ILobbyManager
    {
        /// <summary>
        /// Creates and initializes a new lobby with the specified parameters.   
        /// </summary>
        /// <param name="name"> Lobby name. </param>
        /// <param name="maxPlayers"> Maximum number of players in the lobby. </param>
        /// <param name="mapId"> ID of lobby's map. </param>
        /// <param name="walkableTiles"> Walkable tiles of the lobby. </param>
        /// <param name="fertileTiles"> Feritle tiles of the lobby. </param>
        /// <param name="modulesIDs"> IDs of modules presented in the lobby. </param>
        /// <returns> Lobby ID. </returns>
        public int CreateAndInitializeLobby(string name, int maxPlayers, int mapId, bool[][] walkableTiles, bool[][] fertileTiles, IEnumerable<int> modulesIDs);

        /// <summary>
        /// Adds a user to the specified lobby.
        /// </summary>
        /// <param name="lobbyId"> ID of the lobby to add the user to. </param>
        /// <param name="client"> TcpClient of the user. </param>
        /// <param name="username"> Username of the user. </param>
        /// <param name="userEntityID"> Output parameter to receive the user's entity ID. </param>
        /// <returns> True if the user was added; otherwise, false (lobby is full). </returns>
        public bool AddUserToLobby(int lobbyId, TcpClient client, string username, out Guid userEntityId);

        /// <summary>
        /// Removes a user from the specified lobby.
        /// </summary>
        /// <param name="lobbyId"> ID of the lobby to remove the user from
        /// <paramref name="client"/> TcpClient of the user to remove. </param>
        public void RemoveUserFromLobby(int lobbyId, TcpClient client);

        /// <summary>
        /// Retrieves the data of the specified lobby.
        /// </summary>
        /// <param name="lobbyId"> ID of the lobby to retrieve data for. </param>
        /// <returns> LobbyDTO containing the lobby data. </returns>
        public LobbyDTO GetLobbyData(int lobbyId);

        /// <summary>
        /// Retrieves the data of all existing lobbies.
        /// </summary>
        /// <returns> List of LobbyDTOs containing data of all lobbies. </returns
        public List<LobbyDTO> GetAllLobbiesData();

        /// <summary>
        /// Removes a user from all lobbies they are possibly in.
        /// </summary>
        /// <param name="client"> TcpClient of the user to remove. </param>
        public void RemoveUserFromLobbies(TcpClient client);

        /// <summary>
        /// Sends message to lobbby where the client is.
        /// </summary>
        /// <param name="client"> TcpClient of the user to send message to lobby. </param>
        /// <param name="message"> Message to send. </param>
        public void SendMessageToLobbyWithClient(TcpClient client, MessageBase message);

    }
}
