using System.Net.Sockets;
using SharedLibrary.DTOs.LobbyDTO;
using SharedLibrary.Helpers;
using SharedLibrary.Logging;

namespace Server.Core.Lobby
{
    public interface ILobby
    {
        /// <summary>
        /// Unique identifier for the lobby. Read-only after lobby creation.
        /// </summary>
        public int LobbyId { get; }

        /// <summary>
        /// Read-only name of the lobby.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Read-only map identifier for the lobby. Used in client.
        /// </summary>
        public int MapID { get; }

        /// <summary>
        /// Max number of players allowed in the lobby. Read-only after lobby creation.
        /// </summary>  
        public int MaxPlayers { get; }


        /// <summary>
        /// Event triggered when the lobby is closed.
        /// </summary>
        public event Action OnLobbyClosed;


        /// <summary>
        /// Adds a client to the lobby and returns a unique identifier for the user world entity.
        /// </summary>
        /// <param name="client">TCP client to be added.</param>
        /// <param name="username">Name of user's entity.</param>
        /// <returns>ID of created WorldEntity. </returns>
        public Guid AddClient(TcpClient client, string username);

        /// <summary>
        /// Removes client from the lobby. Removes his world entity as well.
        /// </summary>
        /// <param name="client">TCP client to be removed.</param>"
        /// <returns>True if client was removed, false otherwise.</returns>
        public bool RemoveClient(TcpClient client);

        /// <summary>
        /// Checks if a position in the lobby is free (no world entity occupies it).
        /// </summary>
        /// <param name="position">Position to be checked.</param>
        /// <returns>True if the position is unoccupied, false otherwise.</returns>
        public bool IsPositionFree(Position2D position);

        /// <summary>
        /// Adds a module to the list of allowed modules in the lobby.
        /// </summary>
        /// <param name="moduleId">ID of the module to add.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool AddAllowedModule(int moduleId);
        
        /// <summary>
        /// Creates a world entity in the lobby.
        /// </summary>
        /// <param name="entity"> Entity to be created. </param>
        /// <returns> True if the operation was successfull, false otherwise. </returns>
        public bool AddWorldEntity(WorldEntity entity);

        /// <summary>
        /// Destroys a world entity in the lobby.
        /// </summary>
        /// <param name="entity"> Entity to be destroyed. </param>
        /// <returns> True if the operation was successfull, false otherwise. </returns>
        public bool DestroyWorldEntity(WorldEntity entity);

        /// <summary>
        /// Starts the main loop of the lobby which handles game logic and client communication.
        /// </summary>
        public void Run();

        /// <summary>
        /// Converts the lobby to a LobbyDTO for data transfer.
        /// </summary>
        /// <returns> DataTransferObject representing this lobby </returns>
        public LobbyDTO ToDTO();
    }
}
