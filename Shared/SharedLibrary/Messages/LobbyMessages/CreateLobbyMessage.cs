using System.Text.Json;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains information needed to create a new game lobby.
    /// </summary>
    public class CreateLobbyMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.CreateLobby;

        /// <summary>
        /// Username of the player creating the lobby.
        /// </summary>
        public string UserName { get; init; }

        /// <summary>
        /// Lobby name to be created.
        /// </summary>
        public string LobbyName { get; init; }
        
        /// <summary>
        /// Maximum number of players allowed in the lobby.
        /// </summary>
        public int MaxPlayers { get; init; }
        
        /// <summary>
        /// Map ID for the lobby.
        /// </summary>
        public int MapID { get; init; }
        
        /// <summary>
        /// IDs of modules to be included in the lobby.
        /// </summary>
        public IEnumerable<int> ModuleIDs { get; init; }

        /// <summary>
        /// Represents world tiles' walking availability.
        /// </summary>
        public bool[][] WalkableTiles { get; init; }

        /// <summary>
        /// Represents world tiles' plant availability.
        /// </summary>
        public bool[][] FertileTiles { get; init; }

        /// <summary>
        /// Constructor.
        /// </summary
        /// <param name="lobbyName"> Name of the lobby to be created. </param>
        /// <param name="username"> Username of the player creating the lobby. </param>
        /// <param name="maxPlayers"> Maximum number of players allowed in the lobby. </param>  
        /// <param name="mapID"> ID of the map for the lobby. </param>
        /// <param name="moduleIDs"> IDs of modules to be included in the lobby. </param>
        /// <param name="walkableTiles"> 2D array representing walkable tiles in the world. </param>
        /// <param name="fertileTiles"> 2D array representing fertile tiles in the world. </param>
        public CreateLobbyMessage(string lobbyName, string username, int maxPlayers, int mapID, IEnumerable<int> moduleIDs, bool[][] walkableTiles, bool[][] fertileTiles)
        {
            LobbyName = lobbyName;
            UserName = username;
            MaxPlayers = maxPlayers;
            MapID = mapID;
            ModuleIDs = moduleIDs;
            WalkableTiles = walkableTiles;
            FertileTiles = fertileTiles;
        }
        
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                UserName = this.UserName,
                LobbyName = this.LobbyName,
                MaxPlayers = this.MaxPlayers,
                MapID = this.MapID,
                ModuleIDs = this.ModuleIDs,
                WalkableTiles = this.WalkableTiles,
                FertileTiles = this.FertileTiles
            };

            return JsonSerializer.Serialize(payload);
        }

    }

}
